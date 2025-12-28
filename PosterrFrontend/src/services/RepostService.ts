import { PosterrRecord } from "../models/Record";


const API_URL = import.meta.env.VITE_API_URL;

export async function createRepost(creator: string, idPost: number) {  
    const payload = {creator: creator, idPost: idPost};

    const response = await fetch(`${API_URL}/repost`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        mode: "cors",
        body: JSON.stringify(payload),
        });
        
    if (!response.ok) {
        const data = await response.json();
        const s = response.status;
        if (s === 403 || s === 400 || s === 422 || s === 404) alert(`Error: ${data.message}`)

        throw new Error("Fail to return proper data");
    }

    const data: PosterrRecord[] = await response.json();
    return data;  
}
