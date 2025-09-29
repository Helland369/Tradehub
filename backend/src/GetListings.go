package src

import (
	"context"
	"time"

	"github.com/Helland369/Tradehub/src/users"
	"github.com/gofiber/fiber/v3"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
)


func GetListings(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {
		collection := client.Database("tradehub").Collection("listings")

		ctx, cancle := context.WithTimeout(context.Background(), 10*time.Second)
		defer cancle()

		var listings []users.Listing

		cursor, err := collection.Find(ctx, bson.M{})
		if err != nil {
			return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
				"error": "Failed to fetch listings",
			})
		}
		defer cursor.Close(ctx)

		if err := cursor.All(ctx, &listings); err != nil {
			return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
				"error": "Failed to parse listings",
			})
		}

		return c.JSON(listings)
	}
}
