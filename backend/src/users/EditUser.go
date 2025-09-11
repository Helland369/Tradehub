package users

import (
	"context"

	"github.com/gofiber/fiber/v3"
	"github.com/golang-jwt/jwt/v5"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)

func EditUser(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {
		var body struct {
			ID string `json:"id"`
			Fname string `json:"fname"`
			Lname string `json:"lname"`
			Email string `json:"email"`
			UserName string `json:"userName"`
			Street string `json:"street"`
			City string `json:"city"`
			Zip string `json:"zip"`
			Country string `json:"country"`
			Phone string `json:"phone"`
			Password string `json:"password"`
		}

		if err := c.Bind().Body(&body); err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
				"error": "cannot parse JSON",
			})
		}

		userToken := c.Locals("user").(*jwt.Token)
		claims := userToken.Claims.(jwt.MapClaims)
		usrID := claims["id"].(string)
		
		objID, err := primitive.ObjectIDFromHex(usrID)
		if err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
				"error": "invalid user ID",
			})
		}
		
		users := client.Database("tradehub").Collection("users")

		updateFields := bson.M{}

		if body.Fname != "" {
			updateFields["fname"] = body.Fname
		}
		if body.Lname != "" {
			updateFields["lname"] = body.Lname
		}
		if body.Email != "" {
			updateFields["email"] = body.Email
		}
		if body.UserName != "" {
			updateFields["userName"] = body.UserName
		}
		if body.Street != "" {
			updateFields["address.street"] = body.Street
		}
		if body.City != "" {
			updateFields["address.city"] = body.City
		}
		if body.Zip != "" {
			updateFields["address.zip"] = body.Zip
		}
		if body.Country != "" {
			updateFields["address.country"] = body.Country
		}
		if body.Phone != "" {
			updateFields["phone"] = body.Phone
		}
		if body.Password != "" {
			hashed, _:= hashPassword(body.Password)
			updateFields["passwordHash"] = hashed
		}

		update := bson.M{"$set": updateFields}

		_, err = users.UpdateOne(context.TODO(), bson.M{"_id": objID}, update)
		if err != nil {
			return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
				"error": "failed to update user",
			})
		}

		return c.JSON(fiber.Map{
			"message": "user uppdated successfully",
		})
	}
}
