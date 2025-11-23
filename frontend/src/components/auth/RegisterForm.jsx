import React, { useState } from 'react';
import '../../styles/AuthForms.css';

const RegisterForm = ({ onSuccess, onToggle }) => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: ''
  });

  const handleSubmit = (e) => {
    e.preventDefault(); 
    
    // Mock registration
    const userData = {
      id: Date.now(),
      email: formData.email,
      username: formData.username
    };
    
    onSuccess(userData);
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
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
        <button type="submit" className="login-button">Sign Up</button>
      </form>
      <p className="login-toggle-text">
        Have an account? 
        <button onClick={onToggle} className="toggle-button">Log In</button>
      </p>
    </div>
  );
};

export default RegisterForm;