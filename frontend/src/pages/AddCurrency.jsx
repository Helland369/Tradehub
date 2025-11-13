import { useState } from "react";

function AddCurrency() {
  const [formData, setFormData] = useState({
    amount: "",
  });
  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

  const handleChange = (e) => {
    setFormData({
      ...formData,
      amount: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const amountInt = parseInt(formData.amount, 10);

    if (Number.isNaN(amountInt)) {
      setMessage("Please enter a valid number");
      return;
    }

    try {
      const res = await fetch("http://localhost:3000/api/addcurrency", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ amount: amountInt }),
      });

      const data = await res.json();

      if (res.ok) {
        setMessage("Currency added succsessfully!");
      } else {
        setMessage("Error: ", data.error || "Unknown error");
      }
    } catch (err) {
      console.log("Fetch error: ", err);
      setMessage("Something went wrong!");
    }
  };

  return (
    <div id="add-currency-container">
      <form
        name="add-currency-form"
        id="add-currency-form"
        onSubmit={handleSubmit}
      >
        <label name="add-currency">Add currency</label>
        <input
          name="add-currency"
          placeholder="Amount of currency to add"
          type="number"
          value={formData.amount || ""}
          onChange={handleChange}
          required
        />
        <button type="submit">Add currency</button>

        {message && <p>{message}</p>}
      </form>
    </div>
  );
}

export default AddCurrency;
