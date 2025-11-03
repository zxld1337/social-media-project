import React, { useState } from 'react';

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
    <div>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleChange}
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleChange}
          required
        />
        <button type="submit">Log In</button>
      </form>
      <p>
        Don't have an account? 
        <button onClick={onToggle}>Sign Up</button>
      </p>
    </div>
  );
};

export default LoginForm;