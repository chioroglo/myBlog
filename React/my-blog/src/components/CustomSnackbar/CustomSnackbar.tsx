import {Alert, AlertColor, Snackbar} from '@mui/material';
import React from 'react';

interface SnackbarProps {
    isOpen: boolean,
    alertMessage: string,
    alertType: AlertColor,
    closeHandler: () => void
}


const CustomSnackbar = ({
                            isOpen = false,
                            alertMessage = "Snackbar",
                            alertType,
                            closeHandler
                        }: SnackbarProps) => {

    const onClose = () => {
        closeHandler();
    }

    return (

        <Snackbar transitionDuration={1000} onClose={onClose} open={isOpen} autoHideDuration={3000}
                  message={alertMessage}>
            <Alert sx={{width: "100%"}} severity={alertType}>{alertMessage}</Alert>
        </Snackbar>
    )
};

export {CustomSnackbar}