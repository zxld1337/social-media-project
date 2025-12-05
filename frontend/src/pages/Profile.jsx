import React, { use, useEffect, useState } from 'react';
import { useAuth } from '../hooks/useAuth';
import { useNavigate } from 'react-router-dom';
import { fetchPosts } from '../services/post';
import '../styles/Profile.css';

const Profile = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [isEditing, setIsEditing] = useState(false);
  const [userPosts, setUserPosts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

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

  useEffect(() => {
      const loadPosts = async () => {
        setLoading(true);
        try {
          const postsData = await fetchPosts(); 
          const userSpecificPosts = postsData.filter(post => post.username === user.username);         
          setUserPosts(userSpecificPosts);
        } catch (err) {
          setError("Failed to load posts");
        }
        setLoading(false);
      };
      loadPosts();
    }, []);



  return (
    <div className="profile-container">
      {/* Navigation */}
      <nav className="profile-nav">
        <button className="back-button" onClick={() => navigate('/')}>
          ← Back
        </button>
        <h2 className="nav-title">Profile</h2>
        <div className="nav-actions">
          <button 
            className={`edit-profile-btn ${isEditing ? 'editing' : ''}`}
            onClick={() => setIsEditing(!isEditing)}
          >

            {isEditing ? 'Cancel' : 'Edit'}
          </button>
        </div>
      </nav>

      <div className="profile-content">
        {/* Profile Header */}
        <div className="profile-header">
          <div className="cover-container">
            <div className="cover-photo">
              <div className="cover-overlay"></div>
            </div>
            <div className="profile-avatar-section">
              <img 
                src={`https://ui-avatars.com/api/?name=Kerepesi&background=007bff&color=fff&size=120&bold=true`} 
                alt="Profile" 
                className="profile-avatar"
              />
              {isEditing && (
                <button className="change-photo-btn">📷</button>
              )}
            </div>
          </div>

          <div className="profile-info">
            <div className="profile-main">
              <div className="profile-text">
                {isEditing ? (
                  <input
                    type="text"
                    name="username"
                    value={profileData.username}
                    onChange={handleChange}
                    className="edit-input username-input"
                    placeholder="Username"
                  />
                ) : (
                  <h1 className="profile-name">{profileData.username}</h1>
                )}
                
                <div className="profile-bio">
                  {isEditing ? (
                    <textarea
                      name="bio"
                      value={profileData.bio}
                      onChange={handleChange}
                      className="edit-textarea"
                      placeholder="Tell us about yourself..."
                      rows="2"
                    />
                  ) : (
                    <p>{profileData.bio}</p>
                  )}
                </div>

                <div className="profile-meta">
                  {isEditing ? (
                    <div className="edit-meta">
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
                    </div>
                  ) : (
                    <>
                      <div className="meta-item">
                        <span className="meta-icon">🌐</span>
                        <a href={profileData.website} className="meta-link">{profileData.website}</a>
                      </div>
                      <div className="meta-item">
                        <span className="meta-icon">📍</span>
                        <span>{profileData.location}</span>
                      </div>
                      <div className="meta-item">
                        <span className="meta-icon">📅</span>
                        <span>Joined March 2024</span>
                      </div>
                    </>
                  )}
                </div>
              </div>

              <div className="profile-stats">
                <div className="stat-item">
                  <div className="stat-number">{userPosts.length}</div>
                  <div className="stat-label">Posts</div>
                </div>
                <div className="stat-item">
                  <div className="stat-number">1,245</div>
                  <div className="stat-label">Followers</div>
                </div>
                <div className="stat-item">
                  <div className="stat-number">567</div>
                  <div className="stat-label">Following</div>
                </div>
              </div>
            </div>

            {isEditing && (
              <button className="save-profile-btn" onClick={handleSave}>
                Save Changes
              </button>
            )}
          </div>
        </div>

        {/* Posts Section */}
        <div className="posts-section">
          <h3 className="posts-title">Recent Posts</h3>
          <div className="posts-grid">
            {userPosts.map(post => (
              <div key={post.id} className="post-card">
                <div className="post-content">
                  <p>{post.text}</p>
                </div>
                <div className="post-footer">
                  <div className="post-stats">
                    <span className="post-stat">❤️ {post.likes}</span>
                    <span className="post-stat">💬 {post.comments}</span>
                  </div>
                  <span className="post-time">{post.time || "idk h ago"}</span>
                </div>
              </div>
            ))}
          </div>

          {userPosts.length === 0 && (
            <div className="empty-posts">
              <div className="empty-icon">📝</div>
              <h4>No posts yet</h4>
              <p>Share your thoughts with the community</p>
              <button className="create-post-btn">Create first post</button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Profile;