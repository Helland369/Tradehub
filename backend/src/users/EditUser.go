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

		update := bson.M{
			"$set": bson.M{
				"Fname": body.Fname,
				"lname": body.Lname,
				"email": body.Email,
				"userName": body.UserName,
				"street": body.Street,
				"city": body.City,
				"zip": body.Zip,
				"country": body.Country,
				"phone": body.Phone,
				"password": body.Password,
			},
		}
		
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
