import React, { useState } from 'react';
import '../../styles/AuthForms.css';

const LoginForm = ({ onSuccess, onToggle }) => {
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });

  const handleSubmit = (e) => {
    e.preventDefault();
        
    const userData = {
      id: 1,
      email: formData.email,
      username: formData.email.split('@')[0]
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
      <h2 className="login-title">Login</h2>
      <form className="login-form" onSubmit={handleSubmit}>
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
        <button type="submit" className="login-button">Log In</button>
      </form>
      <p className="login-toggle-text">
        Don't have an account? 
        <button onClick={onToggle} className="toggle-button">Sign Up</button>
      </p>
    </div>
  );
};

export default LoginForm;