const BASE_URL = "http://localhost:5000/api/posts";


export const HandleLikeLogicPost = async (postId) => {
    const likeStatus = await likePost(postId);
    
    if (likeStatus === 409) {
        // Post was already liked, so unlike it
        const unlikeSuccess = await unlikePost(postId);
        return { success: unlikeSuccess, action: 'unliked' };
    } else if (likeStatus === 200 || likeStatus === 201) {
        // Successfully liked
        return { success: true, action: 'liked' };
    }
    
    // Failed
    return { success: false, action: 'none' };
};

export const likePost = async (postId) => {
    const response = await fetch(`${BASE_URL}/${postId}/like`, {
        method: "POST",
        credentials: "include",
    });

    return response.status;
};


export const unlikePost = async (postId) => {
    const response = await fetch(`${BASE_URL}/${postId}/like`, {
        method: "DELETE",
        credentials: "include",
    });
    return response.ok ? true : false;
};


