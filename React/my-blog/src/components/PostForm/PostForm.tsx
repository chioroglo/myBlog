import React, {useState} from 'react';
import {useFormik} from "formik";
import {PostDto} from "../../shared/api/types/post";
import * as Yup from 'yup';
import {palette, PostValidationConstraints} from "../../shared/assets";
import {AlertColor, Box, Button, CircularProgress, FormControl, FormHelperText, Paper, TextField} from '@mui/material';
import {PostFormProps} from "./PostFormProps";
import {FormHeader} from '../FormHeader';
import AutoFixHighIcon from '@mui/icons-material/AutoFixHigh';
import {CustomSnackbar} from "../CustomSnackbar";
import {AxiosError} from "axios";

const textFieldStyle: React.CSSProperties = {
    padding: "0 0 20px 0"
}

const errorTextStyle: React.CSSProperties = {
    color: "red",
    fontStyle: "italic"
}

const PostForm = ({caption = "Form", callback, width = "100%"}: PostFormProps) => { // TODO update to actual type of axios API call

    const [loading, setLoading] = useState<boolean>(false);
    const [snackbarOpen, setSnackbarOpen] = useState<boolean>(false);
    const [snackbarCaption, setSnackbarCaption] = useState<string>("Unknown error");
    const [snackbarAlertType, setSnackbarAlertType] = useState<AlertColor>();

    const openSnackbar = () => setSnackbarOpen(true);
    const closeSnackbar = () => setSnackbarOpen(false);

    const formik = useFormik<PostDto>({
            initialValues: {
                title: "",
                content: "",
                topic: ""
            },
            onSubmit: async (values,formikHelpers) => {
                setLoading(true);
                callback(values).then((result) => {
                    if (result.status !== 200 && result instanceof AxiosError) {
                        setSnackbarCaption(result.response?.data.Message);
                        setSnackbarAlertType("error");
                    } else {
                        setSnackbarCaption("Post was successfully handled!");
                        setSnackbarAlertType("success");
                        formikHelpers.resetForm();
                    }
                }).then(() => {
                    openSnackbar();
                    setLoading(false);
                });
            },
            validationSchema: Yup.object({
                title: Yup.string()
                    .required()
                    .max(PostValidationConstraints.TitleMaxLength),
                content: Yup.string()
                    .required()
                    .max(PostValidationConstraints.ContentMaxLength),
                topic: Yup.string()
                    .optional()
                    .max(PostValidationConstraints.TopicMaxLength)
            })
        }
    );

    return (<>
            {
                loading
                    ?
                    <Box style={{margin: "50px auto", width: "fit-content"}}>
                        <CircularProgress/>
                    </Box>
                    :
                    <Paper elevation={12} style={{
                        width: width,
                        padding: "20px",
                        margin: "20px auto",
                        display: "flex",
                        justifyContent: "space-between",
                        flexDirection: "row"
                    }}>
                        <FormHeader iconColor={palette.JET} caption={caption} icon={<AutoFixHighIcon/>}></FormHeader>

                        <form style={{width: "70%", display: "flex", flexDirection: "column"}}
                              onSubmit={formik.handleSubmit}>
                            <FormControl>
                                <FormHelperText>
                                    {formik.touched.title && formik.errors.title &&
                                        <span style={errorTextStyle}>{formik.errors.title}</span>}
                                </FormHelperText>
                                <TextField style={textFieldStyle} name="title" label="Title" placeholder="Title"
                                           value={formik.values.title} onChange={formik.handleChange}/>
                            </FormControl>

                            <FormControl>
                                <FormHelperText>
                                    {formik.touched.topic && formik.errors.topic &&
                                        <span style={errorTextStyle}>{formik.errors.topic}</span>}
                                </FormHelperText>
                                <TextField style={textFieldStyle} name="topic" label="Topic (optional)"
                                           value={formik.values.topic}
                                           onChange={formik.handleChange}/>
                            </FormControl>

                            <FormControl>
                                <FormHelperText>
                                    {formik.touched.content && formik.errors.content &&
                                        <span style={errorTextStyle}>{formik.errors.content}</span>}
                                </FormHelperText>

                                <TextField style={textFieldStyle}
                                           name="content"
                                           label="Content"
                                           placeholder="Content"
                                           multiline
                                           rows={5}
                                           value={formik.values.content}
                                           onChange={formik.handleChange}
                                />
                            </FormControl>

                            <Button type={"submit"}>Submit</Button>
                            {
                                snackbarOpen && <CustomSnackbar alertType={snackbarAlertType} isOpen={snackbarOpen}
                                                                alertMessage={snackbarCaption}
                                                                closeHandler={closeSnackbar}/>
                            }
                        </form>
                    </Paper>
            }
        </>
    );
};

export {PostForm};
