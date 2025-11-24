import { useEffect, useState } from "react";

function ShoppingCart() {
  const [items, setItems] = useState([]);

  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

  async function buyItems() {
    if (!token) {
      setMessage("You must login to buy items!");
      return;
    }

    try {
      const res = await fetch("http://localhost:3000/api/buy_shopping_cart", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      const data = await res.json().catch(() => ({}));

      if (!res.ok) {
        setMessage(`Error buying cart: ${data.error || "Unknown error!"}`);
        return;
      }

      setMessage(data.message || "Purchuse complete!");

      await fetchItems();
    } catch (err) {
      console.log(err);
      setMessage("Something went wrong when buing the cart!");
    }
  }

  async function fetchItems() {
    if (!token) {
      setMessage("You must login to buy items!");
      return;
    }

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
        setMessage(
          `Error loading cart: ${data.error || data.message || "Unknown error!"}`,
        );
      }
    } catch (err) {
      console.log(err);
      setMessage("Something went wrong while loading the cart!");
    }
  }

  async function handleRemove(id) {
    if (!token) {
      setMessage("You must login to buy items!");
      return;
    }

    try {
      const res = await fetch("http://localhost:3000/api/remove_from_cart", {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          itemId: id,
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

      await fetchItems();
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
        <>
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
                  {item.quantity && (
                    <span className="item-quantity">
                      {item.quantity} quantity
                    </span>
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

          <div className="shopping-cart-footer">
            <button className="buy-button" onClick={buyItems}>
              Buy all
            </button>
          </div>
        </>
      )}
    </div>
  );
}

export default ShoppingCart;
