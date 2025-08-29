package main

import (
	"context"
	"fmt"
	"log"
	"os"

	"github.com/gofiber/fiber/v3"
	"github.com/joho/godotenv"
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

	app.Get("/users", users.GetUser(client))
	// app.Post("/users", create_user)
	// app.Patch("/users/:id", update_user)
	// app.Delete("/users/:id", delete_users)

	port := os.Getenv("PORT")
	if port == "" {
		port = "6000"
	}

	log.Fatal(app.Listen("0.0.0.0:" + port))
}
