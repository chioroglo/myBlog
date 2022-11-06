import {CustomModalProps} from "../CustomModalProps";

export interface LogoutCustomModalProps extends CustomModalProps {
    logoutHandler: () => void
}