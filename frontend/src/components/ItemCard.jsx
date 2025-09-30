function ItemCard({ item }) {
  return (
    <div className="itemCard">
      <h3>{item.title}</h3>
      <div className="item-image">
        <img src={`http://localhost:4000/${item.images}`} alt="No image" />
      </div>
      <div className="item-info">
        <p>Description: {item.description}</p>
        <p>Price: {item.buyPrice}</p>
      </div>
    </div>
  );
}

export default ItemCard;
