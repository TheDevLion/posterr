import { useState } from "react"
import { HelperText, IconContainer, InputLabel, PostInputContainer, PostInputTextArea } from "./styles"
import SendIcon from '@mui/icons-material/Send';
import { getLoggedUser } from "../../helpers/helper";
import { createPost } from "../../services/PostService";

interface PostInputProps {
    clearData: () => void;
    fetchData: () => void;
    filterTextRef: React.RefObject<string>;
}

export const PostInput = ({clearData, fetchData, filterTextRef} : PostInputProps) => {
    /*====================== STATES ======================*/
    const [postText, setPostText] = useState<string>("");

    /*====================== FUNCTIONS ======================*/
    const confirmPost = () => {
        createPost(getLoggedUser(), postText)
            .then(() => {
                setPostText("");
                filterTextRef.current = "";
                clearData();
                fetchData();
            })
    }
    
    /*====================== RENDER ======================*/
    return <PostInputContainer>
        <InputLabel>New post</InputLabel>
        <HelperText>Max 777 characters</HelperText>
        <PostInputTextArea
            placeholder="Share something..."
            onChange={(e: React.ChangeEvent<HTMLTextAreaElement>) => {
                if (e.target.value.length <= 777)
                    setPostText(e.target.value)
            }} 
            value={postText}
        />

        <IconContainer>
            <SendIcon onClick={() => confirmPost()}/>
        </IconContainer>
    </PostInputContainer>
}
