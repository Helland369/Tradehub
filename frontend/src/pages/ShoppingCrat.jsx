import { useEffect, useState } from "react";

function ShoppingCart() {
  const [items, setItems] = useState([]);

  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

  async function fetchItems() {
    try {
      const res = await fetch(
        "http://localhost:3000/api/get_all_shopping_cart",
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        },
      );

      const data = await res.json();

      if (res.ok) {
        setItems(data);
        setMessage("Shopping cars succesfully loaded!");
      } else {
        setMessage("Error: ", data.error || "Unknown error");
      }
    } catch (err) {
      console.log(err);
      setMessage("Something went wrong while loading the cart!");
    }
  }

  async function handleRemove(item) {
    try {
      const res = await fetch("http://localhost:3000/api/remove_from_cart", {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          itemId: item.id,
          quantity: 1,
        }),
      });

      if (!res.ok) {
        const data = await res.json().catch(() => ({}));
        setMessage(
          `Error removing item: ${data.error || data.title || "Unknown error"}`,
        );
        return;
      }

      setItems((prev) => prev.filter((item) => item.id !== itemId));
    } catch (err) {
      console.log(err);
      setMessage("Something went wrong when removing the item!");
    }
  }

  useEffect(() => {
    fetchItems();
  }, []);

  return (
    <div className="shopping-cart">
      <h2>Shopping cart</h2>

      {message && <p>{message}</p>}

      {items.length === 0 ? (
        <p>No items in cart</p>
      ) : (
        <ul className="shopping-cart-items">
          {items.map((item) => {
            const id = item.id;
            return (
              <li key={id} className="shopping-cart-item">
                <span className="item-title">
                  {item.title || "Untitled item"}
                </span>
                {item.buyPrice && (
                  <span className="item-price">{item.buyPrice} points</span>
                )}
                <button
                  className="remove-button"
                  onClick={() => handleRemove(id)}
                >
                  X
                </button>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
}

export default ShoppingCart;
