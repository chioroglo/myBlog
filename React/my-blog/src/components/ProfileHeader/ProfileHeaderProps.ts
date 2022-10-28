import { UserModel } from "../../shared/api/types/user";

export interface ProfileHeaderProps{
    user: UserModel,
    children?: React.ReactNode
}