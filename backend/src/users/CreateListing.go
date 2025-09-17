package users

import (
	"context"

	"github.com/gofiber/fiber/v3"
	"github.com/golang-jwt/jwt/v5"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)

func CreateListing(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {
		var body Listing
		if err := c.Bind().Body(&body); err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
				"error": "cannot parse json",
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

		newListing := Listing{
			ID:            primitive.NewObjectID(),
			Title:         body.Title,
			Description:   body.Description,
			Category:      body.Category,
			Condition:     body.Condition,
			Images:        body.Images,
			StartingPrice: body.StartingPrice,
			BuyPrice:      body.BuyPrice,
			CurrentBid:    0,
			SellerID:      objID,
			Bids:          body.Bids,
			IsAuction:     body.IsAuction,
			CreatedAt:     body.CreatedAt,
			UpdatedAt:     body.UpdatedAt,
			EndTime:       body.EndTime,
			Status:        body.Status,
			Watchers:      body.Watchers,
			Location:      body.Location,
		}

		listings := client.Database("tradehub").Collection("listings")
		_, err = listings.InsertOne(context.TODO(), newListing)
		if err != nil {
			return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
				"error": "failed to create listing",
			})
		}

		return c.JSON(fiber.Map{
			"message": "listing created successfully",
			"listing": newListing,
		})
	}
}
