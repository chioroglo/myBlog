import {AlertColor} from "@mui/material"
import {UserInfoCache} from "./shared/types";


export interface ApplicationState {
    isCurrentlyNotifying: boolean,
    notificationText: string,
    notificationSeverity: AlertColor,
    user: UserInfoCache | null
}

export const ReduxActionTypes = {
    ChangeUser: "SET_USER",
    ChangeNotification: "SET_NOTIFICATION_PAYLOAD",
    DisplayNotification: "SET_VISIBILITY_OF_NOTIFICATION"
}

export class CustomNotificationPayload {
    message: string;
    severity: AlertColor = "info";


    constructor(message: string, severity: AlertColor) {
        this.message = message;
        this.severity = severity;
    }
}