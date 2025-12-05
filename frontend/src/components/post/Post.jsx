// Post.jsx
import React from 'react';
import '../../styles/post.css';

const Post = ({ post, onLike, onComment }) => {
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
          <span className="post-time">{post.dateOfPost || "xy hours ago"}</span>
        </div>
      </div>
      <div className="post-content">{post.text}</div>
      
      <div className="post-stats">
        <span className="post-stat">❤️ {post.likes || 0} likes</span>
        <span className="post-stat">💬 {post.commentCount || 0} comments</span>
      </div>
      
      <div className="post-actions">
        <button className="action-btn" onClick={() => onLike(post.id)}>
          ❤️ Like
        </button>
        <button className="action-btn" onClick={() => onComment(post)}>
          💬 Comment
        </button>
      </div>
    </div>
  );
};

export default Post;