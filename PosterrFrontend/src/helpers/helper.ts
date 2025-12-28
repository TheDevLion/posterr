import { SESSION_STORAGE_USER_KEY } from "../constants";

export const formatDate = (dateStr: string) => {
    const date = new Date(dateStr);

    const formattedDate = date.toLocaleDateString("en-GB", {
        day: "2-digit",
        month: "short",
        year: "numeric",
    });

    const formattedTime = date.toLocaleTimeString("en-GB", {
        hour: "2-digit", 
        minute: "2-digit",
        second: "2-digit",
    });

    return `${formattedDate} - ${formattedTime}`

}

export const getLoggedUser = () => {
    const user = sessionStorage.getItem(SESSION_STORAGE_USER_KEY);
    return atob(user ?? "");
}