package main

import (
	"fmt"
	"log"

	"github.com/gofiber/fiber"
)

func main() {
	fmt.Println("Hello Wrold")
	app := fiber.New()

	log.Fatal(app.Listen(":3000"))
}
