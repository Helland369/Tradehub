package main

import (
	"context"
	"fmt"
	"log"
	"os"

	"github.com/gofiber/fiber/v3"
	"github.com/gofiber/fiber/v3/middleware/cors"
	"github.com/joho/godotenv"
	jwtware "github.com/saveblush/gofiber3-contrib/jwt"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"

	"github.com/Helland369/Tradehub/src/users"
)

func main() {
	
	if err := godotenv.Load(".env"); err != nil {
		log.Fatal("Error loading .env file", err)
	}

	MONGODB := os.Getenv("MONGODB")
	clientOptions := options.Client().ApplyURI(MONGODB)
	client, err := mongo.Connect(context.Background(), clientOptions)

	if err != nil {
		log.Fatal(err)
	}

	if err = client.Ping(context.Background(), nil); err != nil {
		log.Fatal(err)
	}

	fmt.Println("Connected to mongodb!")

	app := fiber.New()

	app.Use(cors.New(cors.Config{
		AllowOrigins: []string{"http://localhost:5173"},
		AllowMethods: []string{"GET", "POST", "PUT", "DELETE", "OPTIONS"},
		AllowHeaders: []string{"origin", "Content-Type", "Accept", "Authorization"},
		AllowCredentials: true,
	}))

	jwtMiddleWare := jwtware.New(jwtware.Config{
		SigningKey: jwtware.SigningKey{Key: []byte("secret")},
	})

	app.Post("/login_users", users.LoginUser(client))
	app.Post("/create_users", users.CreateUser(client))
	
	protected := app.Group("/api", jwtMiddleWare)

	protected.Get("/profile", users.Profile(client))
	protected.Post("/edit_user", users.EditUser(client))
	protected.Post("/create_listing", users.CreateListing(client))
	
	port := os.Getenv("PORT")
	if port == "" {
		port = "3000"
	}

	log.Fatal(app.Listen("0.0.0.0:" + port))
}
