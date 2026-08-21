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
        <Dialog fullWidth maxWidth="sm" PaperProps={{elevation: 4, sx: {
                    minWidth: {xs: 0, sm: `${minWidthPx}px`},
                    width: {xs: "calc(100% - 24px)", sm: "fit-content"},
                    maxWidth: "calc(100% - 24px)",
                    height: "fit-content",
                    m: {xs: 1.5, sm: 4},
                    overflowX: "hidden"
                }}}
                open={modalOpen} onClose={() => setModalOpen(false)}>

            <DialogTitle id="alert-dialog-title">
                {title}
            </DialogTitle>

            {children}

        </Dialog>
    );
};

export {CustomModal};
