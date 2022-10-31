import {Alert, AlertColor, Snackbar, SnackbarCloseReason} from '@mui/material';
import React, {SyntheticEvent} from 'react';

interface SnackbarProps {
    isOpen?: boolean,
    alertMessage: string,
    alertType: AlertColor,
    closeHandler: () => void
}


const CustomSnackbar = ({
                            isOpen = false,
                            alertMessage = "Snackbar",
                            alertType = "info",
                            closeHandler
                        }: SnackbarProps) => {

    const onClose = (event: Event | SyntheticEvent<any, Event>, reason: SnackbarCloseReason) => {
        closeHandler();
    }

    return (

        <Snackbar transitionDuration={1500} onClose={onClose} open={isOpen} autoHideDuration={1500}
                  message={alertMessage}>
            <Alert sx={{width: "100%"}} severity={alertType}>{alertMessage}</Alert>
        </Snackbar>
    )
};

export {CustomSnackbar}