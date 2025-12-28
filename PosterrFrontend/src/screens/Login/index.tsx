import { useEffect, useState } from "react";
import { fetchUsers } from "../../services/UserService";
import { User } from "../../models/User";
import { LoginContainer, UsersContainer } from "./styles";
import { SESSION_STORAGE_USER_KEY } from "../../constants";
import { useNavigate } from "react-router-dom";

export const Login = () => {
    /*====================== STATES ======================*/
    const [users, setUsers] = useState<User[]>([]);

    const navigate = useNavigate();

    /*====================== EFFECTS ======================*/    
    useEffect(() => {
        fetchData();
    }, [])

    /*====================== FETCHES ======================*/
    const fetchData = () => {
        fetchUsers()
            .then((response: User[]) => setUsers(response))
    }

    
    /*====================== FUNCTIONS ======================*/
    const handleClick = (u: User) => {
        sessionStorage.setItem(SESSION_STORAGE_USER_KEY, btoa(u.userName));
        navigate("/home");
    }

    /*====================== RENDER ======================*/
    return <LoginContainer>
        <h1>Select the user</h1>
        <UsersContainer>
            {users.map((u: User) => {
                return <button 
                        key={u.userName}
                        onClick={() => handleClick(u)}
                    >
                        {u.userName}
                    </button>
            })}
        </UsersContainer>
    </LoginContainer>
}