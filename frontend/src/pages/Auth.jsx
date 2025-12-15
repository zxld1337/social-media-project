import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import LoginForm from '../components/auth/LoginForm';
import RegisterForm from '../components/auth/RegisterForm';

const Auth = () => {
  const [isLogin, setIsLogin] = useState(true);
  const navigate = useNavigate();
  const { login, isAuthenticated } = useAuth();

  // Redirect if already authenticated
  React.useEffect(() => {
    if (isAuthenticated) {
      navigate('/');
    }
  }, [isAuthenticated, navigate]);

  const handleLogin = (userData) => {
    login(userData);
    navigate('/');
  };

  const handleRegisterSuccess = () => {
    // After successful registration, switch to login view
    setIsLogin(true);
  };

  return (
    <div className="auth-page">
      {isLogin ? (
        <LoginForm 
          onSuccess={handleLogin} 
          onToggle={() => setIsLogin(false)} 
        />
      ) : (
        <RegisterForm 
          onSuccess={handleRegisterSuccess} 
          onToggle={() => setIsLogin(true)} 
        />
      )}
    </div>
  );
};

export default Auth;