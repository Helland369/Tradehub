function ItemCard({ item }) {
  return (
    <div className="itemCard">
      <div className="item-image">
        <img src="{item.url}" alt="{item.itemName}" />
      </div>
      <div className="item-info">
        <h3>{item.itemName}</h3>
        <p>Price: {item.price}</p>
      </div>
    </div>
  );
}

export default ItemCard;
