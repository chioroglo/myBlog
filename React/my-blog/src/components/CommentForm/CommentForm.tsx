import React, {useState} from 'react';
import {CommentFormProps} from "./CommentFormProps";
import {
    AlertColor,
    Box,
    Button,
    CircularProgress,
    FormControl,
    FormHelperText,
    IconButton,
    Paper,
    TextField
} from "@mui/material";
import {useFormik} from "formik";
import {CommentDto} from '../../shared/api/types/comment';
import {AxiosError} from "axios";
import * as Yup from "yup";
import {CommentValidationConstraints, palette} from "../../shared/assets";
import {FormHeader} from '../FormHeader';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import {CustomSnackbar} from "../CustomSnackbar";
import CloseIcon from "@mui/icons-material/Close";


const textFieldStyle: React.CSSProperties = {
    padding: "0 0 20px 0"
}

const errorTextStyle: React.CSSProperties = {
    color: "red",
    fontStyle: "italic"
}


const CommentForm = ({
                         post,
                         initialComment = {postId: 0, content: ""},
                         formActionCallback,
                         formCloseHandler,
                         width = "100%",
                         caption = "Form"
                     }: CommentFormProps) => {

    const [loading, setLoading] = useState<boolean>();
    const [snackbarOpen, setSnackbarOpen] = useState<boolean>(false);
    const [snackbarCaption, setSnackbarCaption] = useState<string>("Unknown error");
    const [snackbarAlertType, setSnackbarAlertType] = useState<AlertColor>();

    const openSnackbar = () => setSnackbarOpen(true);
    const closeSnackbar = () => setSnackbarOpen(false);


    const formik = useFormik<CommentDto>({
        initialValues: {
            postId: post.id,
            content: initialComment.content
        },
        onSubmit: async (values, formikHelpers) => {
            setLoading(true);

            formActionCallback(values).then((result) => {
                if (result.status !== 200 && result instanceof AxiosError) {
                    setSnackbarCaption(result.response?.data.Message);
                    setSnackbarAlertType("error");
                } else {
                    console.log(result);
                    setSnackbarCaption("Comment was successfully handled!");
                    setSnackbarAlertType("success");
                    formikHelpers.resetForm();
                }
            }).then(() => {
                setLoading(false);
                openSnackbar();
            });
        },
        validationSchema: Yup.object({
            postId: Yup.number()
                .required(),
            content: Yup.string()
                .required("Please introduce comment text")
                .max(CommentValidationConstraints.ContentMaxLength)
        })
    });

    return (
        <>
            {
                loading
                    ?
                    <Box style={{margin: "50px auto", width: "fit-content"}}>
                        <CircularProgress/>
                    </Box>
                    :
                    <Paper elevation={1} sx={{
                        width: width,
                        padding: "20px 0",
                        margin: "0",
                        display: "flex",
                        justifyContent: "space-between",
                        flexDirection: "row"
                    }}>
                        <FormHeader iconColor={palette.JET} caption={caption} icon={<ModeEditIcon/>}/>

                        <form style={{
                            width: "100%",
                            display: "flex",
                            flexDirection: "column",
                            justifyContent: "space-around"
                        }}
                              onSubmit={formik.handleSubmit}>

                            <FormControl>
                                <FormHelperText>
                                    {formik.touched.content && formik.errors.content &&
                                        <span style={errorTextStyle}>{formik.errors.content}</span>}
                                </FormHelperText>

                                <TextField multiline
                                           rows={5} style={textFieldStyle} name="content" label="Comment"
                                           placeholder="Your comment here..."
                                           value={formik.values.content} onChange={formik.handleChange}/>
                            </FormControl>

                            <Button type={"submit"}>Submit</Button>

                            {
                                snackbarOpen && <CustomSnackbar alertType={snackbarAlertType} isOpen={snackbarOpen}
                                                                alertMessage={snackbarCaption}
                                                                closeHandler={closeSnackbar}/>
                            }
                        </form>

                        <Box>
                            <IconButton onClick={formCloseHandler}>
                                <CloseIcon/>
                            </IconButton>
                        </Box>
                    </Paper>
            }
        </>
    );
};

export {CommentForm};