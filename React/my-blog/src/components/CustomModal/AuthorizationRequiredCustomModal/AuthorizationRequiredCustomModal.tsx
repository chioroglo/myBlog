import React from 'react';
import {Box, Button, DialogActions, DialogContentText, Typography} from "@mui/material";
import {CustomModal} from "../CustomModal";
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import {useNavigate} from "react-router-dom";
import {AuthorizationRequiredCustomModalProps} from "./AuthorizationRequiredCustomModalProps";

const AuthorizationRequiredCustomModal = ({
                                              modalOpen,
                                              setModalOpen,
                                              caption
                                          }: AuthorizationRequiredCustomModalProps) => {

    const navigate = useNavigate();

    return (
        <CustomModal modalOpen={modalOpen} setModalOpen={setModalOpen} title={"Information!"}>

            <Box style={{margin: "0 auto"}}>
                <FavoriteBorderIcon fontSize={"large"} color={"info"}/>
            </Box>
            <DialogContentText id="alert-dialog-description">
                <Typography align='center'>{caption}</Typography>
            </DialogContentText>

            <DialogActions style={{display: "flex", flexDirection: "column", padding: "20px"}}>
                <Button style={{margin: "5px"}} fullWidth variant={"outlined"}
                        onClick={() => navigate("/login", {replace: true})}>Login</Button>
                <Button style={{margin: "5px"}} fullWidth variant={"outlined"}
                        onClick={() => navigate("register", {replace: true})}>Sign up</Button>
            </DialogActions>
        </CustomModal>
    );
};

export default AuthorizationRequiredCustomModal;