const BASE_URL = "http://localhost:5000/api/users";

export const fetchUsers = async () => {
  const response = await fetch(`${BASE_URL}`);
  const data = await response.json();
  return data;
};

export const fetchUserById = async (userId) => {
  const response = await fetch(`${BASE_URL}/${userId}`, {
    credentials: "include",
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  const data = await response.json();
  return data;
};
