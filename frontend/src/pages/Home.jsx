import ItemCard from "../components/ItemCard";

function Home() {
  const items = [
    { id: 1, itemName: "Banana", price: 123 },
    { id: 2, itemName: "Apples", price: 123 },
    { id: 3, itemName: "Orange", price: 123 },
    { id: 4, itemName: "Bread", price: 123 },
  ];

  return (
    <div className="home">
      <div className="items-grid">
        {items.map((item) => (
          <ItemCard item={item} key={item.id} />
        ))}
      </div>
    </div>
  );
}

export default Home;
