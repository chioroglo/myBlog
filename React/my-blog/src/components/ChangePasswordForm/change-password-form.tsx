import { Box, Button, FormControl, FormHelperText, Input, InputLabel } from "@mui/material"
import { FormHeader } from "../FormHeader"
import styles from "./change-password-form.module.scss"
import ChangeCircleIcon from '@mui/icons-material/ChangeCircle';
import { palette, PasswordValidationConstraints } from "../../shared/assets";
import { useFormik } from "formik";
import { ChangePasswordDto } from "../../shared/api/types/user";
import * as Yup from "yup";
import { authApi } from "../../shared/api/http/api";
import { selectCurrentUser } from "../../redux";
import { useSelector } from "react-redux";
import { useNotifier } from "../../hooks";

export interface ChangePasswordFormProps {
    onPasswordChanged: () => void
}

export const ChangePasswordForm = ({ onPasswordChanged } : ChangePasswordFormProps) => {
    const user = useSelector(selectCurrentUser);
    const notifyUser = useNotifier();
    
    const formik = useFormik<ChangePasswordDto>({
        initialValues: {
            currentPassword: "",
            newPassword: "",
            confirmNewPassword: ""
        },
        onSubmit: (values, { setSubmitting }) => {

            authApi.changePassword(values).then(() => {
                notifyUser("Password was successfully changed", "success")
                onPasswordChanged();
            }).catch(err => {
                notifyUser(err?.response?.data?.Message ?? "Unknown error occurred", "error");
            }).finally(() => setSubmitting(false));
        },
        validateOnChange: true,
        validationSchema: Yup.object({
            currentPassword: Yup.string()
                .required()
                .min(PasswordValidationConstraints.MinLength)
                .max(PasswordValidationConstraints.MaxLength),
            newPassword: Yup.string()
                .required()
                .notOneOf([Yup.ref("currentPassword")], "Passwords should be different from actual one")
                .min(PasswordValidationConstraints.MinLength)
                .max(PasswordValidationConstraints.MaxLength),
            confirmNewPassword: Yup.string()
                .notOneOf([Yup.ref("currentPassword")], "Passwords should be different from actual one")
                .oneOf([Yup.ref("newPassword")], "Passwords should match")
                .min(PasswordValidationConstraints.MinLength)
                .max(PasswordValidationConstraints.MaxLength)
        })
    });

    if (!user) {
        return <div></div>
    }

    return (<form className={styles["form"]} onSubmit={formik.handleSubmit}>
        <FormHeader className={styles["form__header"]}
            iconColor={palette.JET} caption={"Change Password"}
            icon={<ChangeCircleIcon />} />

        <FormControl className={styles["form__field"]}>
            <InputLabel htmlFor="currentPassword">Current Password</InputLabel>
            <Input
                type="password"
                onChange={formik.handleChange} 
                value={formik.values.currentPassword}
                name="currentPassword"
                placeholder="Old password..."/>
            <FormHelperText>
                {formik.touched.currentPassword && formik.errors.currentPassword && (
                    <span className={styles["form__field--error"]}>{formik.errors.currentPassword}</span>)}
            </FormHelperText>
        </FormControl>

        <FormControl className={styles["form__field"]}>
            <InputLabel htmlFor="newPassword">New Password</InputLabel>
            <Input
                type="password"
                onPaste={(e) => e.preventDefault()}
                onChange={formik.handleChange}
                value={formik.values.newPassword}
                name="newPassword"
                placeholder="New password"/>
            <FormHelperText>
                {formik.touched.newPassword && formik.errors.newPassword && (
                    <span className={styles["form__field--error"]}>{formik.errors.newPassword}</span>)}
            </FormHelperText>
        </FormControl>

        <FormControl className={styles["form__field"]}>
            <InputLabel htmlFor="confirmNewPassword">Repeat New Password</InputLabel>
            <Input
                type="password"
                onPaste={(e) => e.preventDefault()}
                onChange={formik.handleChange}
                value={formik.values.confirmNewPassword}
                name="confirmNewPassword"
                placeholder="New password once more"/>
            <FormHelperText>
                {formik.touched.confirmNewPassword && formik.errors.confirmNewPassword && (
                    <span className={styles["form__field--error"]}>{formik.errors.confirmNewPassword}</span>)}
            </FormHelperText>
        </FormControl>

        <Button loading={formik.isSubmitting} variant="contained" type="submit" disabled={!formik.dirty}>Change Password</Button>
    </form>)
}