import { useState } from "react";

function Registration() {
  const [formData, setFormData] = useState({
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
    role: "user",
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const body = {
      fname: formData.fname,
      lname: formData.lname,
      email: formData.email,
      userName: formData.userName,
      address: {
        street: formData.street,
        city: formData.street,
        zip: parseInt(formData.zip, 10),
        country: formData.country,
      },
      phone: formData.phone,
      password: formData.password,
      role: formData.role,
    };

    try {
      const res = await fetch("http://localhost:3000/create_user", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(body),
      });

      const data = await res.json();
      console.log("Server response: ", data);

      if (res.ok) {
        alert("User created successfully!");
      } else {
        alert("Error: ", data.error);
      }
    } catch (err) {
      console.log("Fetch error: ", err);
      alert("Something went wrong!");
    }
  };

  return (
    <form name="registration-form" onSubmit={handleSubmit}>
      <label name="fname">First Name</label>
      <input
        name="fname"
        placeholder="First name"
        value={formData.fname}
        onChange={handleChange}
      />

      <label name="lname">Last Name</label>
      <input
        name="lname"
        placeholder="Last name"
        value={formData.lname}
        onChange={handleChange}
      />

      <label name="email">Email</label>
      <input
        name="email"
        placeholder="Email"
        value={formData.email}
        onChange={handleChange}
      />

      <label name="userName"> User name</label>
      <input
        name="userName"
        placeholder="User name"
        value={formData.userName}
        onChange={handleChange}
      />

      <label name="street">Street</label>
      <input
        name="street"
        placeholder="Street"
        value={formData.street}
        onChange={handleChange}
      />

      <label name="city">City</label>
      <input
        name="city"
        placeholder="City"
        value={formData.city}
        onChange={handleChange}
      />

      <label name="zip">Zip code</label>
      <input
        name="zip"
        placeholder="Zip code"
        value={formData.zip}
        onChange={handleChange}
      />

      <label name="country">Country</label>
      <input
        name="country"
        placeholder="Country"
        value={formData.country}
        onChange={handleChange}
      />

      <label name="phone">Phone number</label>
      <input
        name="phone"
        placeholder="Phone number"
        value={formData.phone}
        onChange={handleChange}
      />

      <label name="password">Password</label>
      <input
        name="password"
        placeholder="Password"
        value={formData.password}
        onChange={handleChange}
      />

      <button type="submit">Create user</button>
    </form>
  );
}

export default Registration;
