package users

import (
	"context"
	"time"

	"github.com/gofiber/fiber/v3"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)


func PostUser(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {
		usersColelction := client.Database("tradehub").Collection("users")

		var user Users
		if err := c.Bind().Body(&user); err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
			  "error": "cannot parse JSON",
			})
		}

		user.ID = primitive.NewObjectID()
		user.CreatedAt = time.Now()

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
