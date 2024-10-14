import { UserWarningModel } from "../warning/user-warning-model"

export interface UserModel {
    id: number
    username: string
    fullName: string
    registrationDate: string
    lastActivity: string
    activeWarnings: UserWarningModel[]
    isBanned: boolean;
}