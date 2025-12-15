import React, { useState } from "react";
import "../../styles/AuthForms.css";

const RegisterForm = ({ onSuccess, onToggle }) => {
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
  });

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch(
        `http://localhost:5000/api/auth/register`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            username: formData.username,
            email: formData.email,
            password: formData.password,
          }),
        }
      );

      if (response.status === 201) onSuccess();
      else if (response.status === 409) { // Username conflict
        
        const errorData = await response.json();
        alert(errorData.message || "Username is already taken!");

      } else if (response.status === 400) { // Bad request/validation error
        
        const errorData = await response.json();
        console.error("Validation errors:", errorData);
        alert("Please check your input and try again.");

      } else if (response.status === 500) { // Server error        
        
        alert("Server error. Please try again later.");
      } else { // Other errors
        
        alert("An unexpected error occurred.");
      }
    } catch (error) { // Network error or request failed

      console.error("Registration error:", error);
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
      <h2 className="login-title">Sign Up</h2>
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
          type="email"
          name="email"
          placeholder="Email"
          value={formData.email}
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
          Sign Up
        </button>
      </form>
      <p className="login-toggle-text">
        Have an account?
        <button onClick={onToggle} className="toggle-button">
          Log In
        </button>
      </p>
    </div>
  );
};

export default RegisterForm;
