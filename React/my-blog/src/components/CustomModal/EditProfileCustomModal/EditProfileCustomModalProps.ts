import {UserModel} from "../../../shared/api/types/user";
import {CustomModalProps} from "../CustomModalProps";

export interface EditProfileCustomModalProps extends CustomModalProps {
    user: UserModel,
    setUser: React.Dispatch<React.SetStateAction<UserModel | undefined>>,
}