package src

import (
	"context"
	"time"

	"github.com/Helland369/Tradehub/src/users"
	"github.com/gofiber/fiber/v3"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)


func GetListingsById(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {
		collection := client.Database("tradehub").Collection("listings")

		id := c.Params("id")
		objID, err := primitive.ObjectIDFromHex(id)
		if err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
				"error": "Invalid ID",
			})
		}

		ctx, cancle := context.WithTimeout(context.Background(), 10*time.Second)
		defer cancle()

		var listing users.Listing
		err = collection.FindOne(ctx, bson.M{"_id": objID}).Decode(&listing)
		if err != nil {
			return c.Status(fiber.StatusNotFound).JSON(fiber.Map{
				"error": "Listing not found",
			})
		}

		return c.JSON(listing)
	}
}
