import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import PrivateRoute from './components/auth/PrivateRoute';
import Auth from './pages/Auth';
import Home from './pages/Home';

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/auth" element={<Auth />} />
          
          <Route 
            path="/" 
            element={
              <PrivateRoute>
                <Home />
              </PrivateRoute>
            } 
          />
          
          
          <Route path="*" element={<Navigate to="/404" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;