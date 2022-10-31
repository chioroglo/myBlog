import {Button, FormControl, FormHelperText, Input, InputLabel, Paper} from '@mui/material';
import {useFormik} from 'formik';
import React, {useState} from 'react';
import {authApi} from '../../shared/api/http/api';
import {ErrorResponse, RegistrationDto} from '../../shared/api/types';
import * as Yup from "yup";
import {
    FirstnameLastnameConstraints,
    palette,
    PasswordValidationConstraints,
    UserValidationConstraints
} from '../../shared/assets';
import {FormHeader} from '../FormHeader';
import AssignmentIcon from '@mui/icons-material/Assignment';
import {CustomSnackbar} from '../CustomSnackbar';
import {AxiosError} from 'axios';
import {WholePageLoader} from '../WholePageLoader';


const textFieldStyle: React.CSSProperties = {
    maxWidth: "400px",
    width: "20vw",
    minWidth: "300px",
    margin: "0 auto",
    padding: "10px"
};

const paperStyle: React.CSSProperties = {
    width: "450px",
    padding: "20px",
    display: "flex",
    flexDirection: "column"
}

const buttonStyle: React.CSSProperties = {
    margin: "0 auto"
}


const errorTextStyle: React.CSSProperties = {
    color: "red",
    fontStyle: "italic"
}

const RegistrationForm = () => {

    const [isSnackbarOpened, setIsSnackbarOpened] = useState(false);
    const [registrationSuccessful, setRegistrationSuccess] = useState(false);
    const [registrationFailureMessage, setRegistrationFailureMessage] = useState("Unknown error");
    const [loading, setLoading] = useState(false);

    const closeSnackbar = () => setIsSnackbarOpened(false);

    const openSnackbar = () => setIsSnackbarOpened(true);

    const formik = useFormik<RegistrationDto>({
        initialValues: {
            username: "",
            firstName: "",
            lastName: "",
            password: "",
            confirmPassword: ""
        },
        onSubmit: async (values, formikHelpers) => {

            setLoading(true);
            const request = await authApi.TryRegister(values as RegistrationDto);
            console.log(values);

            if (request.status !== 200) {
                const errorMessage = (JSON.parse(((request as AxiosError).request as XMLHttpRequest).responseText) as ErrorResponse).Message;
                setRegistrationFailureMessage(errorMessage);
                setRegistrationSuccess(false);
            } else {
                setRegistrationSuccess(true);
            }

            setLoading(false);
            openSnackbar();
        },
        validationSchema: Yup.object({
            username:
                Yup.string()
                    .required()
                    .min(UserValidationConstraints.MinLength)
                    .max(UserValidationConstraints.MaxLength)
                    .matches(UserValidationConstraints.Regexp, "Username should not begin or end with _")
            ,
            password:
                Yup.string()
                    .required()
                    .min(PasswordValidationConstraints.MinLength)
                    .max(PasswordValidationConstraints.MaxLength)
            ,
            confirmPassword:
                Yup.string()
                    .oneOf([Yup.ref("password")], "Passwords are not the same")
            ,
            firstName:
                Yup.string()
                    .matches(FirstnameLastnameConstraints.Regexp, "First name shall begin with capital letter")
                    .min(FirstnameLastnameConstraints.MinLength)
                    .max(FirstnameLastnameConstraints.MaxLength)
                    .nullable()
            ,
            lastName:
                Yup.string()
                    .matches(FirstnameLastnameConstraints.Regexp, "Last name shall begin with capital letter")
                    .min(FirstnameLastnameConstraints.MinLength)
                    .max(FirstnameLastnameConstraints.MaxLength)
                    .nullable()
        })
    })

    return (
        <>
            {
                loading ?
                    <WholePageLoader/>
                    :
                    <form style={{display: "inline-block"}} onSubmit={formik.handleSubmit}>
                        <Paper style={paperStyle} elevation={12}>
                            <FormHeader iconColor={palette.LIGHT_PINK} caption="Register" icon={<AssignmentIcon/>}/>

                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="username">Username</InputLabel>
                                <Input onChange={formik.handleChange} value={formik.values.username} name="username"/>
                                <FormHelperText>
                                    Contains
                                    from {UserValidationConstraints.MinLength} to {UserValidationConstraints.MaxLength} characters<br/>
                                    {formik.touched.username && formik.errors.username && (
                                        <span style={errorTextStyle}>{formik.errors.username}</span>)}
                                </FormHelperText>
                            </FormControl>


                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="firstName">First name</InputLabel>
                                <Input onChange={formik.handleChange} value={formik.values.firstName} name="firstName"/>
                                <FormHelperText>
                                    Optional
                                    {formik.touched.firstName && formik.errors.firstName && (
                                        <span style={errorTextStyle}>{formik.errors.firstName}</span>)}
                                </FormHelperText>
                            </FormControl>

                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="lastName">Last name</InputLabel>
                                <Input onChange={formik.handleChange} value={formik.values.lastName} name="lastName"/>
                                <FormHelperText>
                                    Optional
                                    {formik.touched.lastName && formik.errors.lastName && (
                                        <span style={errorTextStyle}>{formik.errors.lastName}</span>)}
                                </FormHelperText>
                            </FormControl>

                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="password">Password</InputLabel>
                                <Input type="password" onChange={formik.handleChange} value={formik.values.password}
                                       name="password"/>
                                <FormHelperText>
                                    Contains
                                    from {PasswordValidationConstraints.MinLength} to {PasswordValidationConstraints.MaxLength} characters<br/>
                                    {formik.touched.password && formik.errors.password && (
                                        <span style={errorTextStyle}>{formik.errors.password}</span>)}
                                </FormHelperText>
                            </FormControl>

                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="confirmPassword">Repeat password</InputLabel>
                                <Input type="password" onChange={formik.handleChange}
                                       value={formik.values.confirmPassword} name="confirmPassword"/>
                                <FormHelperText>
                                    {formik.touched.confirmPassword && formik.errors.confirmPassword && (
                                        <span style={errorTextStyle}>{formik.errors.confirmPassword}</span>)}
                                </FormHelperText>
                            </FormControl>


                            <Button style={buttonStyle} type="submit">Register</Button>

                        </Paper>
                        {
                            registrationSuccessful
                                ?
                                <CustomSnackbar closeHandler={closeSnackbar} isOpen={isSnackbarOpened}
                                                alertMessage="Registration successful" alertType="success"/>
                                :
                                <CustomSnackbar closeHandler={closeSnackbar} isOpen={isSnackbarOpened}
                                                alertMessage={registrationFailureMessage} alertType="error"/>
                        }
                    </form>
            }
        </>
    );
};

export {RegistrationForm};