import React, {useEffect} from 'react';
import {CustomModal} from '../CustomModal';
import {EditProfileCustomModalProps} from "./EditProfileCustomModalProps";
import {Button, DialogActions, DialogContent, FormControl, FormHelperText, Input, InputLabel} from "@mui/material";
import * as Yup from "yup";
import {useFormik} from "formik";
import {UserInfoDto} from "../../../shared/api/types/user";
import {FirstnameLastnameConstraints, palette, UsernameValidationConstraints} from "../../../shared/assets";
import {FormHeader} from '../../FormHeader';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import {userApi} from '../../../shared/api/http/api';
import {useSelector} from 'react-redux';
import {ApplicationState} from "../../../redux";
import {AxiosError} from "axios";
import {useNotifier} from '../../../hooks';
import {ErrorResponse} from "../../../shared/api/types";

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

const paperStyle: React.CSSProperties = {
    width: "450px",
    padding: "20px",
    display: "flex",
    flexDirection: "column"
}


const EditProfileCustomModal = ({modalOpen, setModalOpen, user, setUser}: EditProfileCustomModalProps) => {


    /* TODO ADD HANDLING OF AVATARS*/
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



    return (
        <CustomModal modalOpen={modalOpen} setModalOpen={setModalOpen}>
            <DialogContent>
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