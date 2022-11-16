import {AssignmentInd,} from '@mui/icons-material';
import {Button, Checkbox, FormControl, FormControlLabel, FormHelperText, Input, InputLabel, Paper} from '@mui/material';
import {useFormik} from 'formik';
import React, {useState} from 'react';
import {authApi, userApi} from '../../shared/api/http/api';
import {ErrorResponse} from '../../shared/api/types';
import {palette, PasswordValidationConstraints, UsernameValidationConstraints} from '../../shared/assets';
import {FormHeader} from '../FormHeader';
import {AuthenticationForm} from './AuthenticationForm';
import * as Yup from 'yup';
import {AxiosError, AxiosResponse} from 'axios';
import {WholePageLoader} from '../WholePageLoader';
import {useDispatch} from 'react-redux';
import {ReduxActionTypes} from '../../redux';
import {Link} from 'react-router-dom';
import {useNotifier} from '../../hooks';
import {UserInfoCache} from "../../shared/types";

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

    const setUser = (user: UserInfoCache) => {
        dispatch({type: ReduxActionTypes.ChangeUser, payload: user});
    }

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then((result: AxiosResponse<string>) => result.data);

    const [loading, setLoading] = useState(false);
    const displayNotification = useNotifier();

    const formik = useFormik<AuthenticationForm>({
        initialValues: {
            username: "",
            password: "",
            rememberMe: false
        },
        onSubmit: async (values, formikHelpers) => {
            setLoading(true);
            const result = await authApi.TryAuthenticateAndPayloadInHeaders({...values}, values.rememberMe);

            if (result.status !== 200 && result instanceof AxiosError<ErrorResponse>) {
                const errorMessage = result.response?.data.Message;
                displayNotification(errorMessage, "error");
            } else {
                fetchAvatarUrl(result.data.id).then((resultOfAvatarFetching) => {
                    setUser(new UserInfoCache(result.data.id, result.data.username, resultOfAvatarFetching));

                    displayNotification("Authorization successfull", "success");
                })
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
                </form>
            }
        </>
    );
};

export {LoginForm};