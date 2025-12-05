import React, { useState, useEffect } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { fetchCommentsByPostId, createComment, deleteComment } from '../../services/comment';
import '../../styles/CommentsModal.css';

const CommentsModal = ({ post, onClose }) => {
  const { user } = useAuth();
  const [newComment, setNewComment] = useState('');
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  // Fetch comments when modal opens
  useEffect(() => {
    const loadComments = async () => {
      if (!post?.id) return;
      
      setLoading(true);
      setError(null);
      try {
        const commentsData = await fetchCommentsByPostId(post.id);
        setComments(commentsData);
      } catch (err) {
        setError('Failed to load comments');
        console.error('Error loading comments:', err);
      } finally {
        setLoading(false);
      }
    };

    loadComments();
  }, [post?.id]);

  const handleSubmitComment = async (e) => {
    e.preventDefault();
    if (!newComment.trim()) return;
    
    setSubmitting(true);
    setError(null);
    
    try {
      const result = await createComment(post.id, newComment);
      
      // Create new comment object for display
      const newCommentObj = {
        id: result.CommentId,
        username: user.username,
        userId: user.id,
        comment: newComment,
        time: 'Just now',
        likes: 0
      };
      
      // Add new comment to the beginning of the list
      setComments([newCommentObj, ...comments]);
      setNewComment('');
    } catch (err) {
      setError('Failed to post comment');
      console.error('Error posting comment:', err);
    } finally {
      setSubmitting(false);
    }
  };

  const handleDeleteComment = async (commentId) => {
    if (!window.confirm('Are you sure you want to delete this comment?')) return;
    console.log('Deleting comment with ID:', commentId);
    
    try {
      await deleteComment(commentId);
      // Remove deleted comment from state
      setComments(comments.filter(comment => comment.id !== commentId));
    } catch (err) {
      setError('Failed to delete comment');
      console.error('Error deleting comment:', err);
    }
  };

  const handleLikeComment = (commentId) => {
    // Note: This is a placeholder - you'll need to implement the like endpoint
    setComments(comments.map(comment => 
      comment.id === commentId 
        ? { ...comment, likes: comment.likes + 1 }
        : comment
    ));
  };

  const canDeleteComment = (commentUserId) => {
    return user.id === commentUserId;
  };

  return (
    <div className="comments-modal-overlay" onClick={onClose}>
      <div className="comments-modal" onClick={(e) => e.stopPropagation()}>
        {/* Modal Header */}
        <div className="comments-header">
          <div className="comments-header-left">
            <button className="close-button" onClick={onClose}>
              ✕
            </button>
            <h2 className="comments-title">Comments</h2>
          </div>
          <div className="post-info">
            <div className="post-user-info">
              <img 
                src={`https://ui-avatars.com/api/?name=${post.username}&background=007bff&color=fff`} 
                alt={post.username}
                className="post-user-avatar"
              />
              <span className="post-username">{post.username}</span>
            </div>
          </div>
        </div>

        {/* Error Message */}
        {error && (
          <div className="error-message">
            {error}
          </div>
        )}

        {/* Original Post */}
        <div className="original-post">
          <div className="post-content">
            <p>{post.content}</p>
            <div className="post-stats">
              <span className="post-stat">❤️ {post.likes} likes</span>
              <span className="post-stat">💬 {comments.length} comments</span>
              <span className="post-time">{post.time}</span>
            </div>
          </div>
        </div>

        {/* Comments Section */}
        <div className="comments-section">
          <h3 className="comments-count">
            {comments.length} {comments.length === 1 ? 'Comment' : 'Comments'}
          </h3>
          
          {loading ? (
            <div className="loading-comments">Loading comments...</div>
          ) : (
            <div className="comments-list">
              {comments.map((comment) => (
                <div key={comment.id} className="comment-item">
                  <img 
                    src={`https://ui-avatars.com/api/?name=${comment.username}&background=${comment.userId === user.id ? '28a745' : '666'}&color=fff`} 
                    alt={comment.username}
                    className="comment-avatar"
                  />
                  <div className="comment-content">
                    <div className="comment-header">
                      <span className="comment-username">{comment.username}</span>
                      <span className="comment-time">{comment.time}</span>
                      {canDeleteComment(comment.userId) && (
                        <button 
                          className="comment-delete-btn"
                          onClick={() => handleDeleteComment(comment.id)}
                        >
                          Delete
                        </button>
                      )}
                    </div>
                    <p className="comment-text">{comment.comment}</p>
                    <div className="comment-actions">
                      <button 
                        className="comment-like-btn"
                        onClick={() => handleLikeComment(comment.id)}
                      >
                        ❤️ {comment.likes || 0}
                      </button>
                      <button className="comment-reply-btn">Reply</button>
                    </div>
                  </div>
                </div>
              ))}
              
              {comments.length === 0 && !loading && (
                <div className="no-comments">
                  No comments yet. Be the first to comment!
                </div>
              )}
            </div>
          )}
        </div>

        {/* Comment Input */}
        <form className="comment-input-container" onSubmit={handleSubmitComment}>
          <input
            type="text"
            value={newComment}
            onChange={(e) => setNewComment(e.target.value)}
            placeholder="Add a comment..."
            className="comment-input"
            disabled={submitting}
            autoFocus
          />
          <button 
            type="submit" 
            className="comment-submit-btn"
            disabled={!newComment.trim() || submitting}
          >
            {submitting ? 'Posting...' : 'Post'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default CommentsModal;