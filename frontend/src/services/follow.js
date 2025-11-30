const BASE_URL = "http://localhost:5000/api/follow";

export const fetchFollowers = async (userId) => {
    const response = await fetch(`${BASE_URL}/${userId}/followers`);
    const data = await response.json();
    return data;
};

export const fetchFollowing = async (userId) => {
    const response = await fetch(`${BASE_URL}/${userId}/following`);
    const data = await response.json();
    return data;
}

export const followUser = async (userId) => {
    const response = await fetch(`${BASE_URL}/${userId}/follow`, {
        method: 'POST',
        credentials: 'include',
    });

    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.Message || 'Failed to follow user');
    }
    const data = await response.json();
    return data;
}

export const unfollowUser = async (userId) => {
    const response = await fetch(`${BASE_URL}/${userId}/unfollow`, {
        method: 'POST',
        credentials: 'include',
    }); 
    
    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.Message || 'Failed to unfollow user');
    }

    const data = await response.json();
    return data;
}