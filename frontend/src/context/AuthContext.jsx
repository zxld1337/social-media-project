import React, { createContext, useState, useEffect } from 'react';
import { fetchUserById } from '../services/user';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const storedUser = JSON.parse(localStorage.getItem('user') || 'null');
    setUser(storedUser);
    setLoading(false);
  }, []);

  const login = async (userData) => {
    const fullUserData = await fetchUserById(userData.id);

    userData = {
      id: fullUserData.id || "",
      email: fullUserData.email || "",
      username: fullUserData.username || "",
      fullName: fullUserData.fullName || "",
      phoneNumber: fullUserData.phoneNumber || "",
      dateOfBirth: fullUserData.dateOfBirth || "",
      dateOfCreate: fullUserData.dateOfCreate || "",
      profilePicture: fullUserData.profilePicture || "",
      followerCount: fullUserData.followerCount || 0,
      followingCount: fullUserData.followingCount || 0,
    }

    setUser(userData);
    localStorage.setItem('user', JSON.stringify(userData));
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  const value = {
    user,
    login,
    logout,
    isAuthenticated: !!user,
    loading
  };

  return (
    <AuthContext.Provider value={value}>
      {!loading && children}
    </AuthContext.Provider>
  );
};