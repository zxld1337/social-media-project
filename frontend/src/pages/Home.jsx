import React, { useEffect, useState } from "react";
import { useAuth } from "../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import "../styles/Home.css";
import { createPost, fetchPosts } from "../services/post";
import { fetchUsers } from "../services/user";
import CommentsModal from "../components/post/CommentsModal";
import Post from "../components/post/Post";

const Home = () => {
  const navigate = useNavigate();
  const { user, logout } = useAuth();
  const [users, setUsers] = useState([]);

  const [feedPosts, setFeedPosts] = useState([]);
  const [explorePosts, setExplorePosts] = useState([]);

  const [activeTab, setActiveTab] = useState("feed");

  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  // states for comments modal
  const [selectedPost, setSelectedPost] = useState(null);
  const [showComments, setShowComments] = useState(false);

  // Fetch users for suggestions
  useEffect(() => {
    const users = async () => {
      try {
        const usersData = await fetchUsers();
        setUsers(usersData);
      } catch (err) {
        console.error("Failed to fetch users:", err);
      }
    };
    users();
  }, []);

  // Fetch posts for feed/explore
  useEffect(() => {
    const loadFeedPosts = async () => {
      setLoading(true);
      try {
        const postsData = await fetchPosts();
        setFeedPosts(postsData);
      } catch (err) {
        setError("Failed to load posts");
      }
      setLoading(false);
    };
    loadFeedPosts();
  }, [refreshTrigger]);

  // handle logout
  const handleLogout = () => {
    logout();
    navigate("/auth");
  };
  
  // handle create post
  const handleCreatePost = async () => {
    const text = document.querySelector(".post-input").value;

    try {
      const result = await createPost(text, null);
      setRefreshTrigger(result.PostId);
      document.querySelector(".post-input").value = "";
    } catch (error) {
      console.error("Error:", error.message);
    }
  };

  // Comment modal handlers
  const handleOpenComments = (post) => {
    const modalPost = {
      id: post.id,
      username: post.username,
      content: post.text,
      likes: post.likes || 0,
      comments: post.comments || 0,
      time: post.dateOfPost || "xy hours ago",
    };
    setSelectedPost(modalPost);
    setShowComments(true);
  };

  const handleCloseComments = () => {
    setShowComments(false);
    setSelectedPost(null);
  };

  const handleFollow = (userId) => (e) => {
    e.preventDefault();

    console.log(`Followed user with ID: ${userId}`);
  };

  const handleLike = (postId) => (e) => {
    e.preventDefault();

    console.log(`Liked post with ID: ${postId}`);
  };

  return (
    <>
      <div className="home-container">
        {/* Navbar */}
        <nav className="navbar">
          <div className="nav-brand">SocialApp</div>
          <div className="nav-tabs">
            <button
              className={`tab-button ${activeTab === "feed" ? "active" : ""}`}
              onClick={() => setActiveTab("feed")}
            >
              Feed
            </button>
            <button
              className={`tab-button ${
                activeTab === "explore" ? "active" : ""
              }`}
              onClick={() => setActiveTab("explore")}
            >
              Explore
            </button>
          </div>
          <div className="nav-actions">
            <button
              className="profile-btn"
              onClick={() => navigate("/profile")}
            >
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
              <button className="post-button" onClick={handleCreatePost}>
                Post
              </button>
            </div>

            {/* Posts Feed */}
            <div className="posts-container">
              {(activeTab === "feed" ? feedPosts : explorePosts).map((post) => (
                <Post
                  key={post.id}
                  post={post}
                  onLike={handleLike}
                  onComment={handleOpenComments}
                />
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
                onClick={() => navigate("/profile")}
              >
                Edit Profile
              </button>
            </div>

            {/* Suggestions */}
            <div className="suggestions-card">
              <h4>Suggestions for You</h4>
              {[...users.filter((x) => x.id !== user.id)]
                .sort(() => Math.random() - 0.5)
                .slice(0, 2)
                .map((suggestedUser, index) => (
                  <div key={index} className="suggestion-item">
                    <img
                      src={`https://ui-avatars.com/api/?name=${
                        suggestedUser.username
                      }&background=${Math.floor(
                        Math.random() * 16777215
                      ).toString(16)}&color=fff`}
                      alt={suggestedUser.username}
                      className="suggestion-avatar"
                    />
                    <div className="suggestion-info">
                      <strong>{suggestedUser.username}</strong>
                      <span>Suggested for you</span>
                    </div>
                    <button
                      className="follow-btn"
                      onClick={handleFollow(suggestedUser.id)}
                    >
                      Follow
                    </button>
                  </div>
                ))}
            </div>
          </div>
        </div>
      </div>
      {/* Comments Modal */}
      {showComments && selectedPost && (
        <CommentsModal post={selectedPost} onClose={handleCloseComments} />
      )}
    </>
  );
};

export default Home;
