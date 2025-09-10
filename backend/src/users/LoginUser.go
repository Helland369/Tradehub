package users

import (
	"context"
	"time"

	"github.com/gofiber/fiber/v3"
	"github.com/golang-jwt/jwt/v5"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"golang.org/x/crypto/bcrypt"
)

func chechPasswordHash(password string, hash string) bool {
	err := bcrypt.CompareHashAndPassword([]byte(hash), []byte(password))
	return err == nil
}

func LoginUser(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {
		var body struct {
			UserName string `json:"userName"`
			Password string `json:"password"`
		}

		if err := c.Bind().Body(&body); err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
				"error": "cannot parse JSON!",
			})
		}
		
		users := client.Database("tradehub").Collection("users")

		var user Users
		err := users.FindOne(context.TODO(), bson.M{"userName": body.UserName}).Decode(&user)

		if err != nil {
			return c.Status(fiber.StatusUnauthorized).JSON(fiber.Map{
				"error": "Invalid login credentials",
			})
		}

		if !chechPasswordHash(body.Password, user.PasswordHash) {
			return c.Status(fiber.StatusUnauthorized).JSON(fiber.Map{
				"error": "Invalid login credentials",
			})
		}

		claims := jwt.MapClaims{
			"userName": user.UserName,
			"_ID": user.ID,
			"exp":      time.Now().Add(time.Hour * 72).Unix(),
		}

		token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)

		t, err := token.SignedString([]byte("secret"))
		if err != nil {
			return c.SendStatus(fiber.StatusInternalServerError)
		}
		
		return c.JSON(fiber.Map{"token": t})
	}
}
