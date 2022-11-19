import {UserModel} from "../../shared/api/types/user";

export interface ProfileHeaderProps {
    user: UserModel,
    setUser: React.Dispatch<React.SetStateAction<UserModel | undefined>>,
    children?: React.ReactNode
}