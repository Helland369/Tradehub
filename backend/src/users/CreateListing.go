package users

import (
	"context"
	"fmt"
	"os"
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

		if _, err := os.Stat("./uploads"); os.IsNotExist(err) {
			if err := os.MkdirAll("./uploads", os.ModePerm); err != nil {
				return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
					"error": "failed to create uploads directory",
				})
			}
		}
		
		form, err := c.MultipartForm()
		if err != nil {
			return  c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
				"error": "failed to read multipart form",
			})
		}

		files := form.File["images"]
		var imagePaths []string
		for _, file := range files {
			path := fmt.Sprintf("./uploads/%s", file.Filename)
			if err := c.SaveFile(file, path); err != nil {
				return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
					"error": "failed to save image",
				})
			}
			imagePaths = append(imagePaths, path)
		}

		newListing := Listing{
			ID:            primitive.NewObjectID(),
			Title:         title,
			Description:   description,
			Category:      category,
			Condition:     condition,
			Images:        imagePaths,
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
