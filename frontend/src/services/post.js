const BASE_URL = "http://localhost:5000/api/posts";



export const fetchPosts = async () => {
  const response = await fetch(BASE_URL);
  const data = await response.json();
  return data;
};


export const createPost = async (text, imageFile) => {
  const formData = new FormData();
  
  formData.append('Text', text);
  formData.append('ImageFile', imageFile);  

  try {
    const response = await fetch(BASE_URL, {
      method: 'POST',
      body: formData,
      credentials: 'include', // cookies for auth
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.Message || 'Failed to create post');
    }

    const data = await response.json();
    return data;
  } catch (err) {
    throw err;
  }
};
