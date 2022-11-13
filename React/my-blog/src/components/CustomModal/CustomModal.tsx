import {Dialog, DialogTitle} from '@mui/material';
import React from 'react';
import {CustomModalProps} from "./CustomModalProps";


const CustomModal = ({
                         modalOpen,
                         setModalOpen,
                         title,
                         children
                     }: CustomModalProps) => {
    return (
        <Dialog PaperProps={{elevation: 4, sx: {minWidth: "400px", width: "fit-content", height: "fit-content"}}}
                open={modalOpen} onClose={() => setModalOpen(false)}
                aria-labelledby="alert-dialog-title" aria-describedby='alert-dialog-description'>

            <DialogTitle id="alert-dialog-title">
                {title}
            </DialogTitle>

            {children}

        </Dialog>
    );
};

export {CustomModal};