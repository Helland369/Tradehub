import { useState } from "react";

function EditUser() {
  const [formData, setFormData] = useState({
    id: localStorage.getItem("id") || "",
    fname: "",
    lname: "",
    email: "",
    userName: "",
    street: "",
    city: "",
    zip: "",
    country: "",
    phone: "",
    password: "",
  });

  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

  function handleChaneg(e) {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  }

  async function handleSubmit(e) {
    e.preventDefault();

    try {
      const res = await fetch("http://localhost:4000/api/edit_user", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(formData),
      });

      const data = await res.json();

      if (res.ok) {
        setMessage(data.message || "User updated successfully!");
      } else {
        setMessage(data.error || "Something went wrong!");
      }
    } catch (err) {
      setMessage("Network error: " + err.message);
    }
  }

  return (
    <form name="editUser-form" onSubmit={handleSubmit}>
      <label name="fname">First name</label>
      <input
        name="fname"
        placeholder="First name"
        value={formData.fname}
        onChange={handleChaneg}
      />

      <label name="lname">Last name</label>
      <input
        name="lname"
        placeholder="Last name"
        value={formData.lname}
        onChange={handleChaneg}
      />

      <label name="email">Email</label>
      <input
        name="email"
        placeholder="Email"
        value={formData.email}
        onChange={handleChaneg}
      />

      <label name="userName">User name</label>
      <input
        name="userName"
        placeholder="User name"
        value={formData.userName}
        onChange={handleChaneg}
      />

      <label name="street">Street</label>
      <input
        name="street"
        placeholder="Street"
        value={formData.street}
        onChange={handleChaneg}
      />

      <label name="city">City</label>
      <input
        name="city"
        placeholder="City"
        value={formData.city}
        onChange={handleChaneg}
      />

      <label name="zip">Zip code</label>
      <input
        name="zip"
        placeholder="Zip code"
        value={formData.zip}
        onChange={handleChaneg}
      />

      <label name="country">Country</label>
      <input
        name="country"
        placeholder="Country"
        value={formData.country}
        onChange={handleChaneg}
      />

      <label name="phone">Phone number</label>
      <input
        name="phone"
        placeholder="Phone number"
        value={formData.phone}
        onChange={handleChaneg}
      />

      <label name="password">Password</label>
      <input
        name="password"
        placeholder="Password"
        value={formData.password}
        onChange={handleChaneg}
      />

      <button type="submit">Save & update</button>

      {message && <p>{message}</p>}
    </form>
  );
}

export default EditUser;
