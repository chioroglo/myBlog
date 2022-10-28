import { Dialog,DialogTitle } from '@mui/material';
import React from 'react';



const CustomModal = ({modalOpen,setModalOpen,title,children} : {modalOpen: boolean, setModalOpen: React.Dispatch<React.SetStateAction<boolean>>,title:string,children:React.ReactNode}) => {

    return (
        <Dialog PaperProps={{elevation:4, sx:{width:"400px"}}} open={modalOpen} onClose={() => setModalOpen(false)} aria-labelledby="alert-dialog-title" aria-describedby='alert-dialog-description'>
           
            <DialogTitle id="alert-dialog-title">
                {title}
            </DialogTitle>
            
            {children}
        </Dialog>
    );
};

export {CustomModal};