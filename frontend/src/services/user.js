const BASE_URL = "http://localhost:5000/api/users";

export const fetchUsers = async () => {
    const response = await fetch(`${BASE_URL}`);
    const data = await response.json();
    return data;
};





