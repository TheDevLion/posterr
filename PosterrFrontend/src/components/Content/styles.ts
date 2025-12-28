import styled from "styled-components";

export const ContentContainer = styled.div`
    display: flex;
    flex-direction: column;
`

export const SortingContainer = styled.div`
    display: flex;
    justify-content: start;
    gap: 0.5em;
    margin-left: 1rem;

    svg {
        cursor: pointer;
        color: cyan;
    }

    .selected {
        color: yellow;
    }
`
