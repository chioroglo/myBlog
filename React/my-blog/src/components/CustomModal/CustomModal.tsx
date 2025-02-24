import { Dialog, DialogTitle } from '@mui/material';
import React from 'react';
import { CustomModalProps } from "./CustomModalProps";


const CustomModal = ({

                         modalOpen,
                         setModalOpen,
                         title,
                         children,
                         minWidthPx = 400
                     }: CustomModalProps) => {
    return (
        <Dialog PaperProps={{elevation: 4, sx: {minWidth: `${minWidthPx}px`, width: "fit-content", height: "fit-content"}}}
                open={modalOpen} onClose={() => setModalOpen(false)}>

            <DialogTitle id="alert-dialog-title">
                {title}
            </DialogTitle>

            {children}

        </Dialog>
    );
};

export {CustomModal};