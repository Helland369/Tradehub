import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";

function ItemPage() {
  const { id } = useParams();
  const [item, setItem] = useState(null);

  useEffect(() => {
    async function fetchItem() {
      const res = await fetch(`http://localhost:4000/listings/${id}`);
      const data = await res.json();
      setItem(data);
    }
    fetchItem();
  }, [id]);

  if (!item) return <p>Loading...</p>;

  return (
    <div>
      <h1>{item.title}</h1>
      <div>
        {item.images.map((img, i) => {
          <img key={i} src={`http://localhost:4000/${img}`} alt="item" />;
        })}
      </div>
      <p>{item.description}</p>
      <p>Price: {item.buyPrice}</p>
      <button>Buy</button>
    </div>
  );
}

export default ItemPage;
