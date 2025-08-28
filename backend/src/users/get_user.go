package users

import (
	"context"

	"github.com/gofiber/fiber/v3"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/bson"
)

func get_user(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {

		users := client.Database("tradehub").Collection("users")

		cursor ,err := users.Find(context.Background(), bson.M{})
		if err != nil {
			return err
		}

		defer cursor.Close(context.Background())

		var results []bson.M
		if err = cursor.All(context.Background(), &results); err != nil {
			return err
		}
		return c.JSON(results)
	}
}
