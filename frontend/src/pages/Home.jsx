import { useEffect, useState } from "react";
import ItemCard from "../components/ItemCard";

function Home() {
  const [items, setItems] = useState([]);

  useEffect(() => {
    async function fetchItems() {
      try {
        const res = await fetch("http://localhost:3000/api/listings");
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
        {items.map((item) => {
          return <ItemCard item={item} key={item.id} />;
        })}
      </div>
    </div>
  );
}

export default Home;
