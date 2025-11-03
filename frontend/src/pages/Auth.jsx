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

  const handleAuth = (userData) => {
    login(userData);
    navigate('/');
  };

  return (
    <div className="auth-page">
      {isLogin ? (
        <LoginForm 
          onSuccess={handleAuth} 
          onToggle={() => setIsLogin(false)} 
        />
      ) : (
        <RegisterForm 
          onSuccess={handleAuth} 
          onToggle={() => setIsLogin(true)} 
        />
      )}
    </div>
  );
};

export default Auth;