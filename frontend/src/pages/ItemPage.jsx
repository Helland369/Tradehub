import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";

function ItemPage() {
  const { id } = useParams();
  const [item, setItem] = useState(null);
  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

  useEffect(() => {
    async function fetchItem() {
      const res = await fetch(`http://localhost:3000/api/listings/${id}`);
      if (!res.ok) {
        const msg = await res.text();
        throw new Error(msg || `HTTP ${res.status}`);
      }
      const data = await res.json();
      setItem(data);
    }
    fetchItem().catch((err) => {
      console.error("failed to load item:", err);
    });
  }, [id]);

  if (!item) return <p>Loading...</p>;

  const renderImage = () => {
    const imgs = Array.isArray(item.images)
      ? item.images
      : [item.images].filter(Boolean);
    return imgs.map((img, i) => {
      const src =
        typeof img === "string"
          ? `http://localhost:3000/${img.replace(/^uploads\//, "uploads/listings/")}`
          : undefined;
      return <img key={i} src={src} alt={item.title} />;
    });
  };

  const addToCart = async () => {
    if (!token) {
      setMessage("You must be logged inn to add to cart!");
    }

    try {
      const res = await fetch("http://localhost:3000/api/add_to_cart", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(item.id),
      });

      let data = null;

      try {
        data = await res.json();
      } catch (err) {
        setMessage("There was no json!");
      }

      if (res.ok) {
        setMessage("Successfully added item to cart!");
      } else {
        setMessage(`Error: ${data.error || "Unknown error"}`);
      }
    } catch (err) {
      console.log(err);
      setMessage("Something went wrong when adding to cart!");
    }
  };

  return (
    <div>
      <h1>{item.title}</h1>
      <div>{renderImage()}</div>
      <p>{item.description}</p>
      <p>Price: {item.buyPrice}</p>
      <button onClick={addToCart}>Add to cart</button>
      {message && <p>{message}</p>}
    </div>
  );
}

export default ItemPage;
