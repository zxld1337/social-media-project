import React, { useState } from "react";
import "../../styles/AuthForms.css";

const LoginForm = ({ onSuccess, onToggle }) => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch(`http://localhost:5000/api/auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({
          username: formData.username,
          password: formData.password,
        }),
      });

      if (response.status === 200) { // Login successful        
        const data = await response.json();

        const userData = {
          id: null,
          email: formData.email || "",
          username: data.username,
        };

        onSuccess(userData);

      } else if (response.status === 401) {// Invalid credentials
        
        const errorData = await response.json();
        alert(errorData.message || "Invalid username or password!");

      } else if (response.status === 400) { // Validation error
        
        const errorData = await response.json();
        console.error("Validation errors:", errorData);
        alert("Please check your input and try again.");

      } else { // Other errors
        
        alert("An unexpected error occurred.");
      }
    } catch (error) { // Network error or request failed     
      console.error("Login error:", error);
      alert("Unable to connect to the server. Please check your connection.");
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  return (
    <div className="login-container">
      <h2 className="login-title">Login</h2>
      <form className="login-form" onSubmit={handleSubmit}>
        <input
          type="text"
          name="username"
          placeholder="Username"
          value={formData.username}
          onChange={handleChange}
          className="login-input"
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleChange}
          className="login-input"
          required
        />
        <button type="submit" className="login-button">
          Log In
        </button>
      </form>
      <p className="login-toggle-text">
        Don't have an account?
        <button onClick={onToggle} className="toggle-button">
          Sign Up
        </button>
      </p>
    </div>
  );
};

export default LoginForm;
