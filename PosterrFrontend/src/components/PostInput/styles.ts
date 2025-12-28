import styled from "styled-components";

export const PostInputContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 0.4rem;
`;

export const InputLabel = styled.span`
    font-size: 0.85rem;
    letter-spacing: 0.03em;
    color: #ffe7a1;
    text-transform: uppercase;
`;

export const HelperText = styled.span`
    font-size: 0.75rem;
    color: #d0d0d0;
`;

export const PostInputTextArea = styled.textarea`
    width: 100%;
    margin-top: 2rem;
    height: 7rem;
    resize: none;
    padding: 0.2rem;
    border: 1px solid #6a5c2d;
    border-radius: 6px;
`

export const IconContainer = styled.div`
    display: flex;
    justify-content: end;

    svg {
        color: cyan;
        cursor: pointer;
    }
`
