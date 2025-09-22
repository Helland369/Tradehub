import { useState } from "react";

import "../styles/CreateListing.css";

function CreateListing() {
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    category: "",
    condition: "",
    images: null,
    startingPrice: "",
    buyPrice: "",
    isAuction: false,
    endTime: "",
    location: "",
  });

  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

  const handleChange = (e) => {
    const { name, type, value, checked, files } = e.target;
    let newValue = value;

    if (type === "checkbox") {
      newValue = checked;
    } else if (type === "file") {
      newValue = files;
    }

    if (type === "textarea" || e.target.tagName.toLowerCase() === "textarea") {
      e.target.style.height = "auto";
      e.target.style.height = e.target.scrollHeight + "px";
    }

    setFormData((prev) => ({
      ...prev,
      [name]: newValue,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const form = new FormData();
    form.append("title", formData.title);
    form.append("description", formData.description);
    form.append("category", formData.category);
    form.append("condition", formData.condition);
    form.append("startingPrice", formData.startingPrice);
    if (formData.buyPrice) form.append("buyPrice", formData.buyPrice);
    form.append("isAuction", formData.isAuction);
    form.append("endTime", formData.endTime);
    form.append("location", formData.location);

    if (formData.images) {
      for (let i = 0; i < formData.images.length; i++) {
        form.append("images", formData.images[i]);
      }
    }

    try {
      const res = await fetch("http://localhost:4000/api/create_listing", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: form,
      });

      const data = await res.json();
      console.log("Server response: ", data);

      if (res.ok) {
        setMessage("Listing created succsessfully!");
      } else {
        setMessage("Error: ", data.error);
      }
    } catch (err) {
      console.log("Fetch error: ", err);
      setMessage("Something went wrong!");
    }
  };

  return (
    <div id="create-listing-container">
      <form
        name="create-listing-form"
        id="create-listings-form"
        onSubmit={handleSubmit}
      >
        <label name="title">Title</label>
        <input
          name="title"
          placeholder="Title"
          value={formData.title}
          onChange={handleChange}
          required
        />

        <label name="description">Description</label>
        <textarea
          name="description"
          placeholder="Description"
          value={formData.description}
          onChange={handleChange}
          required
        />

        <label name="category">Category</label>
        <select
          name="category"
          value={formData.category}
          onChange={handleChange}
          required
        >
          <option value="">Please select an option</option>
          <option value="Technology">Technology</option>
          <option value="Clothes">Clothes</option>
          {/*TODO Add more categorys...*/}
        </select>

        <label name="condition">Condition</label>
        <select
          name="condition"
          value={formData.condition}
          onChange={handleChange}
          required
        >
          <option value="">Select an option</option>
          <option value="New">New</option>
          <option value="Used">Used</option>
          <option value="Lightly used">Lightly used</option>
          <option value="Heavily used">Heavily used</option>
        </select>

        <label name="startingPrice">Starting price</label>
        <input
          name="startingPrice"
          placeholder="Starting price"
          type="number"
          value={formData.startingPrice}
          onChange={handleChange}
          required
        />

        <label name="buyPrice">Buy price (optional)</label>
        <input
          name="buyPrice"
          placeholder="Buy price"
          type="number"
          value={formData.buyPrice}
          onChange={handleChange}
        />

        <label name="isAuction">This is an auction</label>
        <input
          name="isAuction"
          type="checkbox"
          checked={formData.isAuction}
          onChange={handleChange}
        />

        <label name="endTime">Set the end time</label>
        <input
          name="endTime"
          type="datetime-local"
          value={formData.endTime}
          onChange={handleChange}
        />

        <label name="location">Lcoation</label>
        <input
          name="location"
          placeholder="Where is the item?"
          value={formData.location}
          onChange={handleChange}
          required
        />

        {/*Work in progress...*/}
        <label name="images">Upload images</label>
        <input
          name="images"
          type="file"
          accept="image/*"
          multiple
          onChange={handleChange}
        />

        <button type="submit">Create listing</button>

        {message && <p>{message}</p>}
      </form>
    </div>
  );
}

export default CreateListing;
