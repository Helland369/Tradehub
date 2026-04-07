import { Link } from "react-router-dom";

function ItemCard({ item }) {
  const imgPath = Array.isArray(item.images) ? item.images[0] : item.images;

  const baseUrl = "http://localhost:3000";

  const formatedPath = imgPath?.startsWith("uploads/listings/")
    ? imgPath
    : imgPath?.replace("uploads/", "uploads/listings/");

  const src = imgPath ? `${baseUrl}${formatedPath}` : undefined;

  return (
    <div className="itemCard">
      <Link to={`/item/${item.id}`}>
        <h3>{item.title}</h3>
        <div className="item-image">
          <img src={src} alt={item.title} />
        </div>
      </Link>
      <div className="item-info">
        <p>Description: {item.description}</p>
        <p>Price: {item.buyPrice}</p>
      </div>
    </div>
  );
}

export default ItemCard;
