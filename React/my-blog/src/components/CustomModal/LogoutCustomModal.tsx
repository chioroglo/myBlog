import {Button, DialogActions, DialogContentText, Typography} from '@mui/material';
import React from 'react';
import {CustomModal} from './CustomModal';

const LogoutCustomModal = ({
                               logoutHandler,
                               modalOpen,
                               setModalOpen
                           }: { logoutHandler: () => void, modalOpen: boolean, setModalOpen: React.Dispatch<React.SetStateAction<boolean>> }) => {
    return (

        <CustomModal modalOpen={modalOpen} setModalOpen={setModalOpen} title={"Logout!"}>
            <DialogContentText id="alert-dialog-description">
                <Typography align='center'>Are you sure you want to quit?</Typography>
            </DialogContentText>

            <DialogActions>
                <Button onClick={() => setModalOpen(false)}>No</Button>
                <Button onClick={() => logoutHandler()}>Yes</Button>
            </DialogActions>
        </CustomModal>
    );
};

export {LogoutCustomModal};