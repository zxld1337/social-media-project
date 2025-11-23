import React, { useState } from 'react';
import { useAuth } from '../hooks/useAuth';
import { useNavigate } from 'react-router-dom';
import '../styles/Home.css';

const Home = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState('feed');

  const handleLogout = () => {
    logout();
    navigate('/auth');
  };

  // Mock data for posts
  const feedPosts = [
    { id: 1, username: 'john_doe', content: 'Beautiful day for eating sand🙏🙏', time: '2h ago' },
    { id: 2, username: 'jane_smith', content: 'GTA VI JUST DROPPED', time: '4h ago' },
    { id: 3, username: 'dev_guru', content: 'The stockmarket is fucked📛📛📛', time: '6h ago' }
  ];

  const explorePosts = [
    { id: 1, username: 'tech_news', content: 'New JavaScript features coming soon!', time: '1h ago' },
    { id: 2, username: 'web_dev', content: 'CSS Grid vs Flexbox - when to use which?', time: '3h ago' }
  ];

  return (
    <div className="home-container">
      {/* Navbar */}
      <nav className="navbar">
        <div className="nav-brand">SocialApp</div>
        <div className="nav-tabs">
          <button 
            className={`tab-button ${activeTab === 'feed' ? 'active' : ''}`}
            onClick={() => setActiveTab('feed')}
          >
            Feed
          </button>
          <button 
            className={`tab-button ${activeTab === 'explore' ? 'active' : ''}`}
            onClick={() => setActiveTab('explore')}
          >
            Explore
          </button>
        </div>
        <div className="nav-actions">
          <button className="profile-btn" onClick={() => navigate('/profile')}>
            Profile
          </button>
          <button className="logout-btn" onClick={handleLogout}>
            Logout
          </button>
        </div>
      </nav>

      <div className="home-content">
        {/* Main Feed */}
        <div className="feed-section">
          <div className="welcome-header">
            <h1>Welcome back, {user?.username}! 👋</h1>
            <p>Here's what's happening in your network</p>
          </div>

          {/* Create Post */}
          <div className="create-post">
            <div className="post-input-container">
              <img 
                src={`https://ui-avatars.com/api/?name=${user?.username}&background=007bff&color=fff`} 
                alt="Profile" 
                className="user-avatar"
              />
              <input 
                type="text" 
                placeholder="What's on your mind?" 
                className="post-input"
              />
            </div>
            <button className="post-button">Post</button>
          </div>

          {/* Posts Feed */}
          <div className="posts-container">
            {(activeTab === 'feed' ? feedPosts : explorePosts).map(post => (
              <div key={post.id} className="post-card">
                <div className="post-header">
                  <img 
                    src={`https://ui-avatars.com/api/?name=${post.username}&background=666&color=fff`} 
                    alt={post.username} 
                    className="post-avatar"
                  />
                  <div className="post-user">
                    <strong>{post.username}</strong>
                    <span className="post-time">{post.time}</span>
                  </div>
                </div>
                <div className="post-content">
                  {post.content}
                </div>
                <div className="post-actions">
                  <button className="action-btn">❤️ Like</button>
                  <button className="action-btn">💬 Comment</button>
                  <button className="action-btn">🔄 Share</button>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Right Profile Sidebar */}
        <div className="profile-sidebar">
          <div className="profile-card">
            <img 
              src={`https://ui-avatars.com/api/?name=${user?.username}&background=007bff&color=fff&size=80`} 
              alt="Profile" 
              className="profile-avatar"
            />
            <h3>{user?.username}</h3>
            <p className="profile-email">{user?.email}</p>
            <div className="profile-stats">
              <div className="stat">
                <strong>245</strong>
                <span>Posts</span>
              </div>
              <div className="stat">
                <strong>1.2K</strong>
                <span>Followers</span>
              </div>
              <div className="stat">
                <strong>456</strong>
                <span>Following</span>
              </div>
            </div>
            <button 
              className="edit-profile-btn"
              onClick={() => navigate('/profile')}
            >
              Edit Profile
            </button>
          </div>

          {/* Suggestions */}
          <div className="suggestions-card">
            <h4>Suggestions for You</h4>
            <div className="suggestion-item">
              <img 
                src="https://ui-avatars.com/api/?name=Alex+Johnson&background=28a745&color=fff" 
                alt="Alex Johnson" 
                className="suggestion-avatar"
              />
              <div className="suggestion-info">
                <strong>alex_j</strong>
                <span>Suggested for you</span>
              </div>
              <button className="follow-btn">Follow</button>
            </div>
            <div className="suggestion-item">
              <img 
                src="https://ui-avatars.com/api/?name=Sarah+Miller&background=dc3545&color=fff" 
                alt="Sarah Miller" 
                className="suggestion-avatar"
              />
              <div className="suggestion-info">
                <strong>sarah_m</strong>
                <span>Follows you</span>
              </div>
              <button className="follow-btn">Follow</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;