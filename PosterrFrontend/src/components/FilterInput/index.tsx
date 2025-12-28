import SearchIcon from '@mui/icons-material/Search';
import { FilterInputContainer, FilterInputText, IconContainer, InputLabel } from './styles';
import { useState } from 'react';

interface FilterInputProps {
    filterPostsByKeywords: (keywords: string) => void;
    filterTextRef: React.RefObject<string>;
    clearData: () => void;
}

export const FilterInput = ({filterPostsByKeywords, filterTextRef, clearData}: FilterInputProps) => {
    /*====================== STATES ======================*/
    const [text, setText] = useState<string>("");

    /*====================== RENDER ======================*/
    return <FilterInputContainer>
        <InputLabel>Filter posts</InputLabel>
        <FilterInputText
            placeholder="Type keywords and click the search icon"
            value={text}
            onChange={(e) => {
                filterTextRef.current = e.target.value;
                setText(e.target.value);
            }}
        />
        <IconContainer>
            <SearchIcon onClick={() => {
                clearData();
                filterPostsByKeywords(filterTextRef.current);
            }}/>
        </IconContainer>
    </FilterInputContainer>
}
