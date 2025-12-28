import RepeatIcon from '@mui/icons-material/Repeat';
import { PosterrCardContainer, PosterrCardHeader, PosterrText, RepostIconContainer } from './styles';
import { formatDate, getLoggedUser } from '../../helpers/helper';
import { PosterrRecord } from '../../models/Record';

interface PosterrCardProps {
    record: PosterrRecord;
    setIsDialogOpen: React.Dispatch<React.SetStateAction<boolean>>;
    setSelectedIdToRepost: React.Dispatch<React.SetStateAction<number>>;
}

export const PosterrCard = ({record, setIsDialogOpen, setSelectedIdToRepost}: PosterrCardProps) => {
    /*====================== RENDER ======================*/
    return <PosterrCardContainer>
        <PosterrCardHeader>
            <span>Author: {record.creator}</span>
            <span>{formatDate(record.creationDate)}</span>
        </PosterrCardHeader>
        <PosterrText>
            {record.text}
        </PosterrText>
        {record.isPost && record.creator !== getLoggedUser() && <RepostIconContainer>
                <RepeatIcon onClick={() => {
                    setIsDialogOpen(true);
                    setSelectedIdToRepost(record.id);
                }}/>
        </RepostIconContainer>}
    </PosterrCardContainer>
}