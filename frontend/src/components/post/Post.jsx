// Post.jsx
import React, { useState, useEffect } from "react";
import "../../styles/post.css";

const Post = ({
  post,
  onLike,
  onComment,
  onFollow,
  isFollowing,
  currentUserId,
}) => {
  const isOwnPost = post.userId === currentUserId;
  const [followStatus, setFollowStatus] = useState(isFollowing);

  useEffect(() => {
    setFollowStatus(isFollowing);
  }, [isFollowing]);

  const handleFollowClick = () => {
    setFollowStatus(!followStatus);
    onFollow(post.userId);
  };

  return (
    <div className="post-card">
      <div className="post-header">
        <img
          src={`https://ui-avatars.com/api/?name=${post.username}&background=666&color=fff`}
          alt={post.username}
          className="post-avatar"
        />
        <div className="post-user">
          <strong>{post.username}</strong>
        </div>

        {!isOwnPost && (
          <button
            className={`follow-btn ${followStatus ? "following" : ""}`}
            onClick={handleFollowClick}
          >
            {followStatus ? "✓ Following" : "+ Follow"}
          </button>
        )}
      </div>
      <div className="post-content">{post.text}</div>

      <div className="post-stats">
        <span className="post-stat">❤️ {post.likeCount || 0} likes</span>
        <span className="post-stat">💬 {post.commentCount || 0} comments</span>
      </div>

      <div className="post-actions">
        <button className="action-btn" onClick={() => onLike(post.id)}>
          ❤️ Like
        </button>
        <button className="action-btn" onClick={() => onComment(post)}>
          💬 Comment
        </button>
        <span className="post-time">
          {new Date(post.dateOfPost).toLocaleDateString("en-US", {
            month: "long",
            day: "numeric",
            year: "numeric",
          }) || "xy hours ago"}
        </span>
      </div>
    </div>
  );
};

export default Post;
