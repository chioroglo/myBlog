import React, {ChangeEvent, useEffect, useRef, useState} from 'react';
import {CustomModal} from '../CustomModal';
import {EditProfileCustomModalProps} from "./EditProfileCustomModalProps";
import {
    Avatar,
    Box,
    Button,
    DialogActions,
    DialogContent,
    FormControl,
    FormHelperText,
    Input,
    InputLabel
} from "@mui/material";
import * as Yup from "yup";
import {useFormik} from "formik";
import {UserInfoDto} from "../../../shared/api/types/user";
import {FirstnameLastnameConstraints, palette, UsernameValidationConstraints} from "../../../shared/assets";
import {FormHeader} from '../../FormHeader';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import {avatarApi, userApi} from '../../../shared/api/http/api';
import {useSelector} from 'react-redux';
import {ApplicationState} from "../../../redux";
import {AxiosError, AxiosResponse} from "axios";
import {useNotifier} from '../../../hooks';
import {ErrorResponse} from "../../../shared/api/types";
import UploadFileRoundedIcon from '@mui/icons-material/UploadFileRounded';
import CancelRoundedIcon from '@mui/icons-material/CancelRounded';


const textFieldStyle: React.CSSProperties = {
    maxWidth: "400px",
    width: "20vw",
    minWidth: "300px",
    margin: "0 auto",
    padding: "0 0 20px 0"
};

const errorTextStyle: React.CSSProperties = {
    color: "red",
    fontStyle: "italic"
}


const EditProfileCustomModal = ({modalOpen, setModalOpen, user, setUser}: EditProfileCustomModalProps) => {


    /* TODO FIX UPDATING GLOBAL STATE WHILE CHANGING USER  */
    const isAuthorized = useSelector<ApplicationState>(s => s.isAuthorized);

    const notifyUser = useNotifier();

    const formik = useFormik<UserInfoDto>({
        initialValues: {
            username: user.username,
            firstName: user.fullName.split(' ')[0],
            lastName: user.fullName.split(' ')[1]
        },
        onSubmit: (values, formikHelpers) => {

            if (values.username === user.username) {
                values.username = undefined;
            }

            if ((!values.firstName && !values.lastName) || (values.firstName?.length === 0 && values.lastName?.length === 0)) {
                values.firstName = undefined;
                values.lastName = undefined;
            }


            userApi.editProfileOfAuthorizedUser(values).then((response) => {
                setUser({
                    ...user,
                    lastActivity: new Date(),
                    fullName: `${values.firstName} ${values.lastName}`,
                    username: values.username || user.username
                });
                notifyUser("User information was successfully updated", "success")
                setModalOpen(false);
                formikHelpers.resetForm();

            }).catch((result: AxiosError<ErrorResponse>) => {
                notifyUser(result.response?.data.Message || "Unknown error", "error");
            });
        },
        validationSchema: Yup.object({
            username: Yup.string()
                .min(UsernameValidationConstraints.MinLength)
                .max(UsernameValidationConstraints.MaxLength)
                .matches(UsernameValidationConstraints.Regexp),
            firstName: Yup.string()
                .matches(FirstnameLastnameConstraints.Regexp, "First name shall be a single word and begin with capital letter!")
                .min(FirstnameLastnameConstraints.MinLength)
                .max(FirstnameLastnameConstraints.MaxLength)
                .nullable(),
            lastName: Yup.string()
                .matches(FirstnameLastnameConstraints.Regexp, "Last name shall be a single word and begin with capital letter!")
                .min(FirstnameLastnameConstraints.MinLength)
                .max(FirstnameLastnameConstraints.MaxLength)
                .nullable()
        })
    });

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then((result: AxiosResponse<string>) => result.data);


    const [avatarPreview, setAvatarPreview] = useState<String | ArrayBuffer | null>("");
    const [avatarFile, setAvatarFile] = useState<File | null>(null);

    const inputRef = useRef<HTMLInputElement | null>(null);

    const handleFile = (e: ChangeEvent<HTMLInputElement>) => {

        if (e.target.files) {
            let file = e.target.files[0];

            setAvatarFile(file);
        }
    }

    const handleUpload = async () => {
        console.log(avatarFile);

        if (avatarFile) {
            await avatarApi.RemoveAvatarForAuthorizedUser();

            const response = await avatarApi.UploadNewAvatarForAuthorizedUser(avatarFile);


            /*
            avatarApi.RemoveAvatarForAuthorizedUser()
                .then(() => avatarApi.UploadNewAvatarForAuthorizedUser(avatarFile).then((response) => {
                        notifyUser("Avatar was successfully changed","success");
            })).catch((result: AxiosError<ErrorResponse>) => {
               notifyUser(result.response?.data.Message || "Unknown error","error");
            });
            */
        } else {
            notifyUser("Please select image.", "info");
        }
        ;
    }

    const handleDeleteAvatar = (e: any) => {
        avatarApi.RemoveAvatarForAuthorizedUser().then((response) => {
            notifyUser("Avatar was successfully removed", "success");
        }).catch((result: AxiosError<ErrorResponse>) => {
            notifyUser(result.response?.data.Message || "Unknown error", "error");
        });
    }

    useEffect(() => {
        fetchAvatarUrl(user?.id || 0).then((result) => setAvatarPreview((result)));
    }, []);


    return (
        <CustomModal modalOpen={modalOpen} setModalOpen={setModalOpen}>
            <DialogContent>

                <Box style={{minHeight: "100px"}} display={"flex"} textAlign={"center"} justifyContent={"space-around"}
                     flexDirection={"column"}>

                    <Avatar sx={{minHeight: "128px", minWidth: "128px", width: "2vw", height: "2vw"}}
                            style={{margin: "0 auto"}} src={avatarPreview?.toString()}></Avatar>

                    <Button onClick={handleUpload} color={"primary"} variant={"contained"}
                            startIcon={<UploadFileRoundedIcon/>}>Upload new avatar</Button>

                    <Button onClick={handleDeleteAvatar} color={"error"} variant={"contained"}
                            startIcon={<CancelRoundedIcon/>}>Remove Avatar</Button>


                    <input ref={inputRef} name={"avatar"} accept={"image/*"} id={"contained-button-file"}
                           type={"file"} onChange={handleFile}/>
                </Box>

                <form style={{display: "flex", flexDirection: "column", justifyContent: "space-between"}}
                      onSubmit={formik.handleSubmit}>
                    <FormHeader iconColor={palette.BAYERN_BLUE} caption={"Edit profile information"}
                                icon={<AccountBoxIcon/>}/>

                    <FormControl style={textFieldStyle}>
                        <InputLabel htmlFor="username">Username</InputLabel>
                        <Input onChange={formik.handleChange} value={formik.values.username} name="username"/>
                        <FormHelperText>
                            {formik.touched.username && formik.errors.username && (
                                <span style={errorTextStyle}>{formik.errors.username}</span>)}
                        </FormHelperText>
                    </FormControl>

                    <FormControl style={textFieldStyle}>
                        <InputLabel htmlFor="firstName">First name</InputLabel>
                        <Input onChange={formik.handleChange} value={formik.values.firstName} name="firstName"/>
                        <FormHelperText>
                            {formik.touched.firstName && formik.errors.firstName && (
                                <span style={errorTextStyle}>{formik.errors.firstName}</span>)}
                        </FormHelperText>
                    </FormControl>

                    <FormControl style={textFieldStyle}>
                        <InputLabel htmlFor="lastName">Last name</InputLabel>
                        <Input onChange={formik.handleChange} value={formik.values.lastName} name="lastName"/>
                        <FormHelperText>
                            {formik.touched.lastName && formik.errors.lastName && (
                                <span style={errorTextStyle}>{formik.errors.lastName}</span>)}
                        </FormHelperText>
                    </FormControl>
                </form>
            </DialogContent>

            <DialogActions>
                <Button onClick={() => formik.handleSubmit()}>Update</Button>
                <Button onClick={() => setModalOpen(false)}>Go back</Button>
            </DialogActions>
        </CustomModal>
    );
};

export {EditProfileCustomModal};