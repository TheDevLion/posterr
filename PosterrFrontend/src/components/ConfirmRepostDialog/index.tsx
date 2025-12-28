import { Dialog, DialogTitle, DialogContent, DialogActions } from "@mui/material"

interface ConfirmRepostDialogProps {
    isDialogOpen: boolean;
    setIsDialogOpen: React.Dispatch<React.SetStateAction<boolean>>;
    confirmRepost: () => void;
}

export const ConfirmRepostDialog = ({isDialogOpen, setIsDialogOpen, confirmRepost}: ConfirmRepostDialogProps) => {
    /*====================== RENDER ======================*/
    return <Dialog open={isDialogOpen} onClose={() => setIsDialogOpen(false)}>
        <DialogTitle>Repost confirmation</DialogTitle>

        <DialogContent>
            <p>Are you sure you want to respost?</p>
        </DialogContent>

        <DialogActions>
            <button onClick={() => setIsDialogOpen(false)}>Cancel</button>
            <button onClick={() => confirmRepost()}>Confirm</button>
        </DialogActions>
    </Dialog>
}