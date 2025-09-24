import { useContext, useState } from "react";
import { AuthContext } from "../components/AuthContext";

import "../styles/Login.css";

function Login() {
  const [formData, setFormData] = useState({
    userName: "",
    password: "",
  });

  const { login } = useContext(AuthContext);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handelSubmit = async (e) => {
    e.preventDefault();

    const body = {
      userName: formData.userName,
      password: formData.password,
    };

    try {
      const res = await fetch("http://localhost:4000/login_users", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(body),
      });

      const data = await res.json();
      console.log("Server response: ", data);

      if (res.ok) {
        localStorage.setItem("token", data.token);
        login(data.token);
        alert("Login successfully logged in!");
      } else {
        alert("Error: ", data.error);
      }
    } catch (err) {
      console.log("Ferch error: ", err);
      alert("Something went wrong!");
    }
  };

  return (
    <div id="login-container">
      <form name="login-form" id="login-form" onSubmit={handelSubmit}>
        <label name="userName">User name</label>
        <input
          name="userName"
          placeholder="User name"
          value={formData.userName}
          onChange={handleChange}
        />
        <label name="password"></label>
        <input
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleChange}
        />
        <button type="submit">Login</button>
      </form>
    </div>
  );
}

export default Login;
