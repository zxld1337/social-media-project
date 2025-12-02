BASE_URL = "http://localhost:5000/api/posts";


export const likePost = async (postId) => {
    const response = await fetch(`${BASE_URL}/${postId}/like`, {
        method: "POST" });

    return response.ok ? true : false;
};


export const unlikePost = async (postId) => {
    const response = await fetch(`${BASE_URL}/${postId}/unlike`, {
        method: "POST" });

    return response.ok ? true : false;
};
