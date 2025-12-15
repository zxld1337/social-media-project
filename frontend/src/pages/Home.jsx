import React, { useEffect, useState, useMemo } from "react";
import { useAuth } from "../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import "../styles/Home.css";
import { createPost, fetchPosts } from "../services/post";
import { fetchUsers } from "../services/user";
import CommentsModal from "../components/post/CommentsModal";
import Post from "../components/post/Post";
import { HandleLikeLogicPost } from "../services/like";
import {
  fetchFollowers,
  fetchFollowing,
  handleFollowUser,
} from "../services/follow";

const Home = () => {
  const navigate = useNavigate();
  const { user, logout } = useAuth();
  const [users, setUsers] = useState([]);

  const [followers, setFollowers] = useState([]);
  const [following, setFollowing] = useState([]);

  const [feedPosts, setFeedPosts] = useState([]);
  const [explorePosts, setExplorePosts] = useState([]);

  const [activeTab, setActiveTab] = useState("explore");

  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  // states for comments modal
  const [selectedPost, setSelectedPost] = useState(null);
  const [showComments, setShowComments] = useState(false);

  const followingIds = useMemo(
    () => new Set(following.map((f) => f.userId)),
    [following]
  );
  // Fetch followers and following
  useEffect(() => {
    const loadFollowData = async () => {
      try {
        const followersData = await fetchFollowers(user.id);
        setFollowers(followersData);
        const followingData = await fetchFollowing(user.id);
        setFollowing(followingData);
        user.followerCount = followersData.length;
        user.followingCount = followingData.length;
      } catch (err) {
        console.error("Failed to load follow data:", err);
      }
    };
    if (user) {
      loadFollowData();
    }
  }, [user, refreshTrigger]);

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
        setExplorePosts(postsData);
      } catch (err) {
        setError("Failed to load posts");
      }
      setLoading(false);
    };
    loadFeedPosts();
  }, [refreshTrigger]);

  useEffect(() => {
    const filteredFeedPosts = explorePosts.filter((post) =>
      followingIds.has(post.userId)
    );
    setFeedPosts(filteredFeedPosts);
  }, [explorePosts, followingIds]);

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

  const handleFollow = async (userId) => {
    const followResult = await handleFollowUser(userId);
    setRefreshTrigger(refreshTrigger + 1);
  };

  const handleLike = async (postId) => {
    const likeResult = await HandleLikeLogicPost(postId);

    if (likeResult.success) {
      const updatedPosts = explorePosts.map((post) => {
        if (post.id === postId) {
          const countChange = likeResult.action === "liked" ? 1 : -1;
          return {
            ...post,
            likeCount: Math.max(0, (post.likeCount || 0) + countChange),
            isLiked: likeResult.action === "liked",
          };
        }
        return post;
      });
      setExplorePosts(updatedPosts);
    }
  };

  return (
    <>
      <div className="home-container">
        {/* Navbar */}
        <nav className="navbar">
          <div className="nav-brand">SocialApp</div>
          <div className="nav-tabs">
            <button
              className={`tab-button ${
                activeTab === "explore" ? "active" : ""
              }`}
              onClick={() => setActiveTab("explore")}
            >
              Explore
            </button>
            <button
              className={`tab-button ${activeTab === "feed" ? "active" : ""}`}
              onClick={() => setActiveTab("feed")}
            >
              Feed
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
                  onFollow={handleFollow}
                  isFollowing={followingIds.has(post.userId)}
                  currentUserId={user.id}
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
                  <strong>
                    {
                      explorePosts.filter(
                        (post) => post.username === user.username
                      ).length
                    }
                  </strong>
                  <span>Posts</span>
                </div>
                <div className="stat">
                  <strong>{followers.length}</strong>
                  <span>Followers</span>
                </div>
                <div className="stat">
                  <strong>{following.length}</strong>
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
              {[
                ...users
                  .filter((x) => x.id !== user.id) // Exclude yourself
                  .filter((x) => !followingIds.has(x.id)), // Exclude users you're already following
              ]
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
                      onClick={() => handleFollow(suggestedUser.id)}
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
        <CommentsModal
          post={selectedPost}
          onClose={handleCloseComments}
          trigger={setRefreshTrigger}
        />
      )}
    </>
  );
};

export default Home;
