import { useEffect, useState } from "react";
import ItemCard from "../components/ItemCard";

function Home() {
  const [items, setItems] = useState([]);

  useEffect(() => {
    async function fetchItems() {
      try {
        const res = await fetch("http://localhost:4000/listings");
        const data = await res.json();
        setItems(data);
      } catch (err) {
        console.log("Failed to fetch items", err);
      }
    }

    fetchItems();
  }, []);

  return (
    <div className="home">
      <div className="items-grid">
        {items.map((item, index) => (
          <ItemCard item={item} key={item.id || index} />
        ))}
      </div>
    </div>
  );
}

export default Home;
