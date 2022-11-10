import {AlertColor} from "@mui/material"

export interface ApplicationState {
    isAuthorized: boolean,
    isCurrentlyNotifying: boolean,
    notificationText: string,
    notificationSeverity: AlertColor
}

export const ReduxActionTypes = {
    AuthorizationState: "SET_IS_AUTHORIZED",
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