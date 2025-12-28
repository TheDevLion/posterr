import { PosterrRecord } from "../models/Record";

const API_URL = import.meta.env.VITE_API_URL;

export async function createPost(creator: string, text: string) {  
    const payload = {creator: creator, text: text};

    const response = await fetch(`${API_URL}/post`, {
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
        if (s === 403 || s === 400 ) alert(`Error: ${data.message}`)

        throw new Error("Fail to return proper data");
    }

    const data: PosterrRecord[] = await response.json();
    return data;  
}

export async function getPostsByKeywords(keywords: string, page: number = 0) {  
    const response = await fetch(`${API_URL}/post?keywords=${keywords}&page=${page}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
        mode: "cors",
    });
        
    if (!response.ok) {
        const data = await response.json();
        const s = response.status;
        if (s === 400 ) alert(`Error: ${data.message}`)

        throw new Error("Fail to return proper data");
    }

    const data: PosterrRecord[] = await response.json();
    data.every(d => d.isPost = true);
    return data;  
}
