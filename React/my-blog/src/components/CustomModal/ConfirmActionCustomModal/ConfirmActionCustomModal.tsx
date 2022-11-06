import React from 'react';
import { CustomModal } from '../CustomModal';
import {ConfirmActionCustomModalProps} from "./ConfirmActionCustomModalProps";
import {Button, DialogActions, DialogContentText, Typography} from "@mui/material";

const ConfirmActionCustomModal = ({caption,modalOpen,setModalOpen,actionCallback,title}: ConfirmActionCustomModalProps) => {
    return (
        <CustomModal modalOpen={modalOpen} setModalOpen={setModalOpen} title={title}>
            <DialogContentText>
                <Typography align='center'>{caption}</Typography>
            </DialogContentText>

            <DialogActions>
                <Button onClick={() => actionCallback()}>Yes</Button>
                <Button onClick={() => setModalOpen(false)}>No</Button>
            </DialogActions>
        </CustomModal>
    );
};

export {ConfirmActionCustomModal};