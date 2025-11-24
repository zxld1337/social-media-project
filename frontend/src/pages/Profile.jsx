import React, { useState } from 'react';
import { useAuth } from '../hooks/useAuth';
import { useNavigate } from 'react-router-dom';
import '../styles/Profile.css';

const Profile = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [isEditing, setIsEditing] = useState(false);
  const [profileData, setProfileData] = useState({
    username: user?.username || '',
    email: user?.email || '',
    bio: 'passionate about ... and ... Love ...!',
    website: 'https://ekke.hu',
    location: 'Eger, HU'
  });

  const handleSave = () => {
    // waiting for backend
    setIsEditing(false);
  };

  const handleChange = (e) => {
    setProfileData({
      ...profileData,
      [e.target.name]: e.target.value
    });
  };

  const handleLogout = () => {
    logout();
    navigate('/auth');
  };

  // Mock user posts
  const userPosts = [
    { id: 1, content: 'Just launched my new golf club', likes: 24, comments: 8, time: '2 days ago' },
    { id: 2, content: 'Working on a new open-source project.', likes: 15, comments: 3, time: '1 week ago' },
    { id: 3, content: 'Great day at the Vaci borton', likes: 42, comments: 12, time: '2 weeks ago' }
  ];

  return (
    <div className="profile-container">
      {/* Header/Navigation */}
      <nav className="profile-nav">
        <button className="back-button" onClick={() => navigate('/')}>
          ← Back to Home
        </button>
        <div className="nav-actions">
          <button 
            className="edit-profile-btn"
            onClick={() => setIsEditing(!isEditing)}
          >
            {isEditing ? 'Cancel' : 'Edit Profile'}
          </button>
          <button className="logout-btn" onClick={handleLogout}>
            Logout
          </button>
        </div>
      </nav>

      <div className="profile-content">
        {/* Profile Header */}
        <div className="profile-header">
          <div className="profile-cover">
            <div className="cover-photo"></div>
            <div className="profile-info">
              <div className="avatar-section">
                <img 
                  src={`https://ui-avatars.com/api/?name=${user?.username}&background=007bff&color=fff&size=150`} 
                  alt="Profile" 
                  className="profile-avatar-large"
                />
                {isEditing && (
                  <button className="change-photo-btn">Change Photo</button>
                )}
              </div>
              <div className="profile-details">
                <div className="profile-main">
                  {isEditing ? (
                    <input
                      type="text"
                      name="username"
                      value={profileData.username}
                      onChange={handleChange}
                      className="edit-input"
                    />
                  ) : (
                    <h1>{profileData.username}</h1>
                  )}
                  <div className="profile-stats">
                    <div className="stat">
                      <strong>{userPosts.length}</strong>
                      <span>Posts</span>
                    </div>
                    <div className="stat">
                      <strong>1,245</strong>
                      <span>Followers</span>
                    </div>
                    <div className="stat">
                      <strong>567</strong>
                      <span>Following</span>
                    </div>
                  </div>
                </div>
                
                <div className="profile-bio">
                  {isEditing ? (
                    <textarea
                      name="bio"
                      value={profileData.bio}
                      onChange={handleChange}
                      className="edit-textarea"
                      rows="3"
                    />
                  ) : (
                    <p>{profileData.bio}</p>
                  )}
                </div>

                <div className="profile-meta">
                  {isEditing ? (
                    <>
                      <input
                        type="url"
                        name="website"
                        value={profileData.website}
                        onChange={handleChange}
                        className="edit-input"
                        placeholder="Website"
                      />
                      <input
                        type="text"
                        name="location"
                        value={profileData.location}
                        onChange={handleChange}
                        className="edit-input"
                        placeholder="Location"
                      />
                    </>
                  ) : (
                    <>
                      <span className="meta-item">🌐 {profileData.website}</span>
                      <span className="meta-item">📍 {profileData.location}</span>
                      <span className="meta-item">📅 Joined March 2024</span>
                    </>
                  )}
                </div>

                {isEditing && (
                  <button className="save-profile-btn" onClick={handleSave}>
                    Save Changes
                  </button>
                )}
              </div>
            </div>
          </div>
        </div>

        {/* Profile Tabs */}
        <div className="profile-tabs">
          <button className="profile-tab active">Posts</button>
          <button className="profile-tab">Media</button>
          <button className="profile-tab">Likes</button>
          <button className="profile-tab">Saved</button>
        </div>

        {/* Posts Grid */}
        <div className="posts-grid">
          {userPosts.map(post => (
            <div key={post.id} className="post-card">
              <div className="post-content">
                {post.content}
              </div>
              <div className="post-stats">
                <span className="stat">❤️ {post.likes}</span>
                <span className="stat">💬 {post.comments}</span>
                <span className="post-time">{post.time}</span>
              </div>
              <div className="post-actions">
                <button className="post-action-btn">Like</button>
                <button className="post-action-btn">Comment</button>
                <button className="post-action-btn">Share</button>
              </div>
            </div>
          ))}
        </div>

        {/* Empty State */}
        {userPosts.length === 0 && (
          <div className="empty-state">
            <div className="empty-icon">📝</div>
            <h3>No posts yet</h3>
            <p>When you create posts, they'll appear here.</p>
            <button className="create-first-post">Create your first post</button>
          </div>
        )}
      </div>
    </div>
  );
};

export default Profile;