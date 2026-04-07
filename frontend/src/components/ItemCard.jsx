import { Link } from "react-router-dom";

function ItemCard({ item }) {
  const imgPath = Array.isArray(item.images) ? item.images[0] : item.images;

  let src = undefined;

  if (imgPath && typeof imgPath === "string") {
    const path =
      imgPath.startsWith("uploads/") && !imgPath.includes("uploads/listings/")
        ? imgPath.replace("uploads/", "uploads/listings/")
        : imgPath;

    src = `http://localhost:3000/${path}`;
  }

  return (
    <div className="itemCard">
      <Link to={`/item/${item.id}`}>
        <h3>{item.title}</h3>
        <div className="item-image">
          {src ? (
            <img src={src} alt={item.title} />
          ) : (
            <div className="placeholder">No Image</div>
          )}
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
