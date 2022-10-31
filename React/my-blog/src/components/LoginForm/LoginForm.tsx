import {AssignmentInd,} from '@mui/icons-material';
import {Button, Checkbox, FormControl, FormControlLabel, FormHelperText, Input, InputLabel, Paper} from '@mui/material';
import {useFormik} from 'formik';
import React, {useState} from 'react';
import {authApi} from '../../shared/api/http/api';
import {AuthenticateRequest, ErrorResponse} from '../../shared/api/types';
import {palette} from '../../shared/assets';
import {PasswordValidationConstraints, UserValidationConstraints} from '../../shared/assets/validationConstraints';
import {FormHeader} from '../FormHeader/FormHeader';
import {AuthenticationForm} from './authenticationForm';
import * as Yup from 'yup';
import {CustomSnackbar} from '../CustomSnackbar';
import {AxiosError} from 'axios';
import {WholePageLoader} from '../WholePageLoader';
import {useDispatch} from 'react-redux';
import {ReduxActionTypes} from '../../redux';
import {Link} from 'react-router-dom';

const textFieldStyle: React.CSSProperties = {
    maxWidth: "400px",
    width: "20vw",
    minWidth: "300px",
    margin: "0 auto",
    padding: "0 0 20px 0"
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

const checkboxStyle: React.CSSProperties = {
    margin: "0 auto"
}

const errorTextStyle: React.CSSProperties = {
    color: "red",
    fontStyle: "italic"
}

const LoginForm = () => {

    const dispatch = useDispatch();
    const changeAuthorizedStateOfApplication = (state: boolean) => {
        dispatch({type: ReduxActionTypes.AuthorizationState, payload: state})
    }

    const [isSnackbarOpened, setIsSnackbarOpened] = useState(false);
    const [authorizationSuccessful, setAuthorizationSuccess] = useState(false);
    const [authorizationFailureMessage, setAuthorizationFailureMessage] = useState("Unknown error");
    const [loading, setLoading] = useState(false);

    const closeSnackbar = () => setIsSnackbarOpened(false);
    const openSnackbar = () => setIsSnackbarOpened(true);

    const formik = useFormik<AuthenticationForm>({
        initialValues: {
            username: "",
            password: "",
            rememberMe: false
        },
        onSubmit: async (values, formikHelpers) => {
            setLoading(true);
            const request = await authApi.TryAuthenticateAndPayloadInHeaders(values as AuthenticateRequest, values.rememberMe);
            console.log(request);

            if (request.status !== 200) {
                const errorMessage = (JSON.parse(((request as AxiosError).request as XMLHttpRequest).responseText) as ErrorResponse).Message;
                setAuthorizationFailureMessage(errorMessage);
                setAuthorizationSuccess(false);
            } else {
                setAuthorizationSuccess(true);
                changeAuthorizedStateOfApplication(true);
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
        })

    });


    return (
        <>
            {loading ?
                <WholePageLoader/>
                :
                <form style={{display: "inline-block"}} onSubmit={formik.handleSubmit}>

                    <Paper style={paperStyle} elevation={12}>

                        <FormHeader iconColor={palette.SOFT_ORANGE} caption="Login" icon={<AssignmentInd/>}/>

                        <FormControl style={textFieldStyle}>
                            <InputLabel htmlFor="username">Username</InputLabel>
                            <Input onChange={formik.handleChange} value={formik.values.username} name="username"/>
                            <FormHelperText>
                                {formik.touched.username && formik.errors.username && (
                                    <span style={errorTextStyle}>{formik.errors.username}</span>)}
                            </FormHelperText>
                        </FormControl>

                        <FormControl style={textFieldStyle}>
                            <InputLabel htmlFor="password">Password</InputLabel>
                            <Input type="password" onChange={formik.handleChange} value={formik.values.password}
                                   name="password"/>
                            <FormHelperText>
                                {formik.touched.password && formik.errors.password && (
                                    <span style={errorTextStyle}>{formik.errors.password}</span>)}
                            </FormHelperText>
                        </FormControl>

                        <FormControlLabel name="rememberMe" style={checkboxStyle} onChange={formik.handleChange}
                                          label="Remember me?" control={<Checkbox/>}/>
                        <Button variant="outlined" style={buttonStyle} type="submit">Log in</Button>

                        <Link style={{
                            textAlign: "center",
                            fontStyle: "italic",
                            textDecoration: "bold",
                            color: palette.JET
                        }} to="/register">Do not have an account? Click here</Link>
                    </Paper>
                    {
                        authorizationSuccessful
                            ?
                            <CustomSnackbar closeHandler={closeSnackbar} isOpen={isSnackbarOpened}
                                            alertMessage="Authorization successful" alertType="success"/>
                            :
                            <CustomSnackbar closeHandler={closeSnackbar} isOpen={isSnackbarOpened}
                                            alertMessage={authorizationFailureMessage} alertType="error"/>
                    }
                </form>
            }
        </>
    );
};

export {LoginForm};