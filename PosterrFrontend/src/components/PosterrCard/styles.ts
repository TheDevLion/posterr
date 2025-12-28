import styled from "styled-components"

export const PosterrCardContainer = styled.div`
    border: 1px solid black;
    background: #111111;
    padding: 0.5rem;
    margin: 0.5rem;
    border-radius: 10px;
    overflow: hidden;
    
    svg {
        cursor: pointer;
        color: cyan;
    }
`
export const PosterrCardHeader = styled.div`
    display: flex;
    gap: 0.5rem;
    font-size: 9pt;
    justify-content: space-between;
    color: cyan;
`
export const PosterrText = styled.p`
`
export const RepostIconContainer = styled.div`
    display: flex;
    justify-content: end;
`