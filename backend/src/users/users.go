package users

import (
	"time"

	"go.mongodb.org/mongo-driver/bson/primitive"
)

type Address struct {
	Street    string    `json:"street"`
	City      string    `json:"city"`
	Zip       int       `json:"zip"`
	Country   string    `json:"country"`
}

type Users struct {
	ID             primitive.ObjectID    `json:"id,omitempty" bson:"_id"`
	Fname          string                `json:"fname" bson:"fname"`
	Lname          string                `json:"lname" bson:"lname"`
	Email          string                `json:"email" bson:"email"`
	UserName       string                `json:"userName" bson:"userName"`
	Address        Address               `json:"address" bson:"address"`
	Phone          string                `json:"phone" bson:"phone"`
	PasswordHash   string                `json:"password" bson:"passwordHash"`
	Selling        []primitive.ObjectID  `json:"selling" bson:"selling"`
	Purchases      []primitive.ObjectID  `json:"purchases" bson:"purchases"`
	CreatedAt      time.Time             `json:"createdAt" bson:"createdAt"`
	Role           string                `json:"role" bson:"role"`
}
