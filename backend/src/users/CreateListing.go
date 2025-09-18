package users

import (
	"context"
	"fmt"
	"strconv"
	"time"

	"github.com/gofiber/fiber/v3"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)

func CreateListing(client *mongo.Client) fiber.Handler {
	return func(c fiber.Ctx) error {

		title := c.FormValue("title")
		description := c.FormValue("description")
		category := c.FormValue("category")
		condition := c.FormValue("condition")
		var startingPrice float64
		startingPriceStr := c.FormValue("startingPrice")
		if startingPriceStr != "" {
			val, _ := strconv.ParseFloat(startingPriceStr, 64)
			startingPrice = val
		}
		buyPriceStr := c.FormValue("buyPrice")
		var buyPrice *float64
		if buyPriceStr != "" {
			val, _ := strconv.ParseFloat(buyPriceStr, 64)
			buyPrice = &val
		}
		isAuction := c.FormValue("isAuction") == "true"
		var endTime time.Time
		endTimeStr := c.FormValue("endTime")
		if endTimeStr != "" {
			parsed, err := time.Parse("2006-01-02T15:04", endTimeStr)
			if err != nil {
				return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
					"error": "invalid date format, expected YYYY-MM-DDTHH:MM",
				})
			}
			endTime = parsed
		}
		location := c.FormValue("location")

		fileHadler, err := c.FormFile("images")
		var imagePath string
		if err == nil && fileHadler != nil {
			imagePath = fmt.Sprintf("./uploades/%s", fileHadler.Filename)
			if err := c.SaveFile(fileHadler, imagePath); err != nil {
				return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
					"error": "failed to save image",
				})
			}
		}

		newListing := Listing{
			ID:            primitive.NewObjectID(),
			Title:         title,
			Description:   description,
			Category:      category,
			Condition:     condition,
			Images:        []string{imagePath},
			StartingPrice: startingPrice,
			BuyPrice:      buyPrice,
			CurrentBid:    0,
			IsAuction:     isAuction,
			EndTime:       endTime,
			Location:      location,
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
