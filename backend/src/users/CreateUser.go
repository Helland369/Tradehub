package users

import (
	"context"
	"time"

	"github.com/gofiber/fiber/v3"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
	"golang.org/x/crypto/bcrypt"
)

func hashPassword(password string) (string, error) {
	bytes, err := bcrypt.GenerateFromPassword([]byte(password), 16)
	return string(bytes), err
}

func CreateUser(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {
		usersColelction := client.Database("tradehub").Collection("users")

		var user Users
		if err := c.Bind().Body(&user); err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
			  "error": "cannot parse JSON",
			})
		}

		hash, _ := hashPassword(user.PasswordHash)
		
		user.ID = primitive.NewObjectID()
		user.CreatedAt = time.Now()
		user.PasswordHash = hash
		
		result, err := usersColelction.InsertOne(context.Background(), user)
		if err != nil {
			return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
			  "error": "failed to insert user",
			})
		}
		
		return c.Status(fiber.StatusCreated).JSON(fiber.Map{
		  "insertedID": result.InsertedID,
		  "user":       user,
	  })
	}
}
