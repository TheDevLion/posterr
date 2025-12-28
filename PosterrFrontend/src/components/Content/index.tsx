import { useState } from "react"
import { PosterrRecord } from "../../models/Record";
import { ContentContainer, SortingContainer } from "./styles";
import { getLoggedUser } from "../../helpers/helper";
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import TrendingUpIcon from '@mui/icons-material/TrendingUp';
import { Tooltip } from "@mui/material";
import { createRepost } from "../../services/RepostService";
import { ConfirmRepostDialog } from "../ConfirmRepostDialog";
import { PosterrCard } from "../PosterrCard";

interface ContentProps {
    records: PosterrRecord[];
    clearData: () => void;
    fetchData: () => void;
    sortByDateRef: React.RefObject<boolean>;
    changeSortData: () => void;
}

export const Content = ({records, clearData, fetchData, sortByDateRef, changeSortData}: ContentProps) => {
    /*====================== STATES ======================*/
    const [isDialogOpen, setIsDialogOpen] = useState<boolean>(false);
    const [selectedIdToRepost, setSelectedIdToRepost] = useState<number>(0);

    /*====================== FUNCTIONS ======================*/
    const confirmRepost = () => {
        createRepost(getLoggedUser(), selectedIdToRepost)
            .then(() => {
                setSelectedIdToRepost(0);
                setIsDialogOpen(false);
                clearData();
                fetchData();
            })
            .catch(() => {
                setIsDialogOpen(false);
                setSelectedIdToRepost(0);
            })
    }

    /*====================== RENDER ======================*/
    return <div>
        <SortingContainer>
            <Tooltip title="Sort by date">
                <CalendarMonthIcon onClick={() => {
                    sortByDateRef.current = true;
                    changeSortData();
                }} className={sortByDateRef.current ? "selected" : ""}/>
            </Tooltip>
            <Tooltip title="Sort by trend">
                <TrendingUpIcon onClick={() => {
                    sortByDateRef.current = false
                    changeSortData();
                }} className={!sortByDateRef.current ? "selected" : ""}/>
            </Tooltip>
        </SortingContainer>

        {records && <ContentContainer>
            {records.map((r: PosterrRecord) => {
                return <PosterrCard 
                        key={`${r.id}-${r.isPost}`}
                        record={r} 
                        setIsDialogOpen={setIsDialogOpen} 
                        setSelectedIdToRepost={setSelectedIdToRepost}
                    />
            })}
        </ContentContainer>
        }

        <ConfirmRepostDialog
            isDialogOpen={isDialogOpen} 
            setIsDialogOpen={setIsDialogOpen} 
            confirmRepost={confirmRepost}
        />
    </div>
}