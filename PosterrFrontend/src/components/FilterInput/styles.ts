import styled from "styled-components";

export const FilterInputContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 0.4rem;
`;

export const InputLabel = styled.span`
    font-size: 0.85rem;
    letter-spacing: 0.03em;
    color: #b7f3ff;
    text-transform: uppercase;
`;

export const FilterInputText = styled.input`
    width: 100%;
    height: 1.5rem;
    padding: 0.2rem;
    margin: 1rem 0;
    border: 1px solid #2b5b66;
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
