import { Box, Button, DialogActions, DialogContent } from "@mui/material";
import { CustomModal } from "../CustomModal";
import { CustomModalProps } from "../CustomModalProps";
import styles from "./profile-security-modal.module.scss";
import { FormHeader } from "../../FormHeader";
import { PasskeyList } from "../../PasskeyList/PasskeyList";
import { RegisterPasskeyButton } from "../../RegisterPasskeyButton";
import { useState } from "react";
import { palette } from "../../../shared/assets";
import VpnKeyIcon from '@mui/icons-material/VpnKey';
import { ChangePasswordForm } from "../../ChangePasswordForm/change-password-form";

export const ProfileSecurityModal = ({ modalOpen, setModalOpen }: CustomModalProps) => {

    const [passkeyListUpdateTrigger, setPasskeyListUpdateTrigger] = useState<number>(0);
    const goBack = () => {
        setModalOpen(false);
    };

    return (<CustomModal
        title={"Profile Security"}
        modalOpen={modalOpen}
        setModalOpen={setModalOpen}
        minWidthPx={600}>
        <DialogContent>
            <Box className={styles["security"]}>
                <FormHeader iconColor={palette.SUNRISE} caption="Security" icon={<VpnKeyIcon />} />
                <PasskeyList key={passkeyListUpdateTrigger} />
                <RegisterPasskeyButton caption="ADD PASSKEY" onSuccess={() => setPasskeyListUpdateTrigger(passkeyListUpdateTrigger + 1)} />
            </Box>
            <Box className={styles["change-password"]}>
                <ChangePasswordForm onPasswordChanged={goBack}/>
            </Box>
        </DialogContent>
        <DialogActions>
            <Button onClick={goBack}>Go back</Button>
        </DialogActions>
    </CustomModal>)
}