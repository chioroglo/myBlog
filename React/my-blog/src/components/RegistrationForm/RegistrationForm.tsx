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
    UsernameValidationConstraints
} from '../../shared/assets';
import {FormHeader} from '../FormHeader';
import AssignmentIcon from '@mui/icons-material/Assignment';
import {AxiosError} from 'axios';
import {CenteredLoader} from '../CenteredLoader';
import {useNotifier} from '../../hooks';


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

    const [loading, setLoading] = useState(false);

    const notifyUser = useNotifier();

    const formik = useFormik<RegistrationDto>({
        initialValues: {
            username: "",
            firstName: "",
            lastName: "",
            password: "",
            confirmPassword: ""
        },
        onSubmit: async (values) => {

            setLoading(true);
            const request = await authApi.TryRegister(values as RegistrationDto);

            if (request.status !== 200) {
                const errorMessage = (JSON.parse(((request as AxiosError).request as XMLHttpRequest).responseText) as ErrorResponse).Message;
                notifyUser(errorMessage, "error");
            } else {
                notifyUser("Registered successfull!", "success");
            }

            setLoading(false);
        },
        validationSchema: Yup.object({
            username:
                Yup.string()
                    .required()
                    .min(UsernameValidationConstraints.MinLength)
                    .max(UsernameValidationConstraints.MaxLength)
                    .matches(UsernameValidationConstraints.Regexp, "Username should not begin or end with _")
            ,
            password:
                Yup.string()
                    .required()
                    .min(PasswordValidationConstraints.MinLength)
                    .max(PasswordValidationConstraints.MaxLength)
            ,
            confirmPassword:
                Yup.string()
                    .required("Please confirm your password")
                    .oneOf([Yup.ref("password")], "Passwords are not the same")
            ,
            firstName:
                Yup.string()
                    .matches(FirstnameLastnameConstraints.Regexp, "First name shall begin with capital letter and not contain numbers")
                    .min(FirstnameLastnameConstraints.MinLength)
                    .max(FirstnameLastnameConstraints.MaxLength)
                    .nullable()
            ,
            lastName:
                Yup.string()
                    .matches(FirstnameLastnameConstraints.Regexp, "Last name shall begin with capital letter and not contain numbers")
                    .min(FirstnameLastnameConstraints.MinLength)
                    .max(FirstnameLastnameConstraints.MaxLength)
                    .nullable()
        })
    })

    return (
        <>
            {
                loading ?
                    <CenteredLoader/>
                    :
                    <form style={{display: "inline-block"}} onSubmit={formik.handleSubmit}>
                        <Paper style={paperStyle} elevation={12}>
                            <FormHeader iconColor={palette.LIGHT_PINK} caption="Register" icon={<AssignmentIcon/>}/>

                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="username">Username</InputLabel>
                                <Input onChange={formik.handleChange} value={formik.values.username} name="username"/>
                                <FormHelperText>
                                    Contains
                                    from {UsernameValidationConstraints.MinLength} to {UsernameValidationConstraints.MaxLength} characters<br/>
                                    {formik.touched.username && formik.errors.username && (
                                        <span style={errorTextStyle}>{formik.errors.username}</span>)}
                                </FormHelperText>
                            </FormControl>


                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="firstName">First name</InputLabel>
                                <Input onChange={formik.handleChange} value={formik.values.firstName} name="firstName"/>
                                <FormHelperText>
                                    {formik.touched.firstName && formik.errors.firstName && (
                                        <span style={errorTextStyle}>{formik.errors.firstName}</span>) || "Optional"}
                                </FormHelperText>
                            </FormControl>

                            <FormControl style={textFieldStyle}>
                                <InputLabel htmlFor="lastName">Last name</InputLabel>
                                <Input onChange={formik.handleChange} value={formik.values.lastName} name="lastName"/>
                                <FormHelperText>

                                    {formik.touched.lastName && formik.errors.lastName && (
                                        <span style={errorTextStyle}>{formik.errors.lastName}</span>) || "Optional"}
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
                    </form>
            }
        </>
    );
};

export {RegistrationForm};