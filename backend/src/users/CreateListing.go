package users

import (
	"context"
	"fmt"
	"os"
	"strconv"
	"time"

	"github.com/gofiber/fiber/v3"
	"go.mongodb.org/mongo-driver/bson"
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

		v := c.Locals("userId")
		idStr, ok := v.(string)
		if !ok || idStr == "" {
			return c.Status(fiber.StatusUnauthorized).JSON(fiber.Map{
				"error": "unauthorized",
			})
		}
		userId, err := primitive.ObjectIDFromHex(idStr)
		if err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
				"error": "invalid user id",
			})
		}

		if _, err := os.Stat("./uploads"); os.IsNotExist(err) {
			if err := os.MkdirAll("./uploads", os.ModePerm); err != nil {
				return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
					"error": "failed to create uploads directory",
				})
			}
		}

		form, err := c.MultipartForm()
		if err != nil {
			return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
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
			UserID:        userId,
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
			CreatedAt:     time.Now().UTC(),
			UpdatedAt:     time.Now().UTC(),
		}

		listings := client.Database("tradehub").Collection("listings")
		_, err = listings.InsertOne(context.TODO(), newListing)
		if err != nil {
			return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
				"error": "failed to create listing",
			})
		}

		usersColl := client.Database("tradehub").Collection("users")
		
		_, _ = usersColl.UpdateOne(
			context.TODO(),
			bson.M{"_id": userId, "selling": bson.M{"$type": 10}},
			bson.M{"$set": bson.M{"selling": bson.A{}}})

		res, err := usersColl.UpdateOne(
			context.TODO(),
			bson.M{"_id": userId},
			bson.M{"$addToSet": bson.M{"selling": newListing.ID}},
		)
		if err != nil {
			return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
				"error": "failed to attach listing to user",
				"detail": err.Error(),
			})
		}

		if res.MatchedCount == 0 {
			return c.Status(fiber.StatusNotFound).JSON(fiber.Map{
				"error": "user not found",
			})
		}
		
		return c.JSON(fiber.Map{
			"message": "listing created successfully",
			"listing": newListing,
		})
	}
}
