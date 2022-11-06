import {CustomModalProps} from "../CustomModalProps";

export interface ConfirmActionCustomModalProps extends CustomModalProps{
    actionCallback: () => void,
    caption:string
}