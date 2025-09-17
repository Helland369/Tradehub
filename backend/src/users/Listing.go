package users

import (
	"time"

	"go.mongodb.org/mongo-driver/bson/primitive"
)

type Bid struct {
	UserID    primitive.ObjectID `json:"userId" bson:"userId"`
	Amount    float64            `json:"amount" bson:"amount"`
	Timestamp time.Time          `json:"timestamp" bson:"timestamp"`
}

type ListingStatus string

const (
	StatusActive  ListingStatus = "active"
	StatusSold    ListingStatus = "sold"
	StatusExpired ListingStatus = "expired"
	StatusDraft   ListingStatus = "draft"
)

type Listing struct {
	ID            primitive.ObjectID   `json:"_id,omitempty" bson:"_id"`
	Title         string               `json:"title" bson:"title"`
	Description   string               `json:"description" bson:"description"`
	Category      string               `json:"category" bson:"category"`
	Condition     string               `json:"condition" bson:"condition"`
	Images        []string             `json:"images"`
	StartingPrice float64              `json:"startingPrice" bson:"startingPrice"`
	BuyPrice      *float64             `json:"buyPrice,omitempty" bson:"buyPrice,omitempty"`
	CurrentBid    float64              `json:"currentBid" bson:"currentBid"`
	SellerID      primitive.ObjectID   `json:"sellerId" bson:"sellerId"`
	Bids          []Bid                `json:"bids" bson:"bids"`
	IsAuction     bool                 `json:"isAuction" bson:"isAuction"`
	CreatedAt     time.Time            `json:"createdAt" bson:"createdAt"`
	UpdatedAt     time.Time            `json:"updatedAt" bson:"updatedAt"`
	EndTime       time.Time            `json:"endTime" bson:"endTime"`
	Status        ListingStatus        `json:"status" bson:"status"`
	Watchers      []primitive.ObjectID `json:"watchers" bson:"watchers"`
	Location      string               `json:"location" bson:"location"`
}
