import { PosterrRecord } from "../models/Record";


const API_URL = import.meta.env.VITE_API_URL;

export async function fetchRecords(page: number, sortByDate: boolean) {
  try {
    const response = await fetch(`${API_URL}/record?page=${page}&sortByDate=${sortByDate}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        mode: "cors",
    });
    
    if (!response.ok) {
      throw new Error("Fail to return proper data");
    }

    if (response.status === 204) return [];
    
    const data: PosterrRecord[] = await response.json();
    return data;

  } catch (error) {
    console.error("Error: ", error);
    return [];
  }
}
