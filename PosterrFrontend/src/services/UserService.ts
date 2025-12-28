import { User } from "../models/User";

const API_URL = import.meta.env.VITE_API_URL;

export async function fetchUsers() {
  try {
    const response = await fetch(`${API_URL}/user`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        mode: "cors",
      });
    if (!response.ok) {
      throw new Error("Fail to return proper data");
    }
    
    const data: User[] = await response.json();
    return data;

  } catch (error) {
    console.error("Error: ", error);
    return [];
  }
}
