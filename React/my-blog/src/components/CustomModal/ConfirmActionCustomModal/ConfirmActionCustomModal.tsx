import React from 'react';
import {CustomModal} from '../CustomModal';
import {ConfirmActionCustomModalProps} from "./ConfirmActionCustomModalProps";
import {Button, DialogActions, DialogContentText, Typography} from "@mui/material";

const ConfirmActionCustomModal = ({
                                      caption,
                                      modalOpen,
                                      setModalOpen,
                                      actionCallback,
                                      title
                                  }: ConfirmActionCustomModalProps) => {
    return (
        <CustomModal modalOpen={modalOpen} setModalOpen={setModalOpen} title={title}>
            <DialogContentText sx={{px: 3, py: 1}}>
                <Typography align='center'>{caption}</Typography>
            </DialogContentText>

            <DialogActions sx={{px: 3, pb: 3, gap: 1}}>
                <Button color="inherit" onClick={() => setModalOpen(false)}>Cancel</Button>
                <Button variant="contained" color="error" onClick={() => actionCallback()}>Confirm</Button>
            </DialogActions>
        </CustomModal>
    );
};

export {ConfirmActionCustomModal};
