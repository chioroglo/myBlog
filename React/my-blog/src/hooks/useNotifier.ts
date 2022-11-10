import {AlertColor} from "@mui/material";
import {useDispatch} from "react-redux";
import {CustomNotificationPayload, ReduxActionTypes} from "../redux";

export const useNotifier = () => {

    const dispatch = useDispatch();

    const displayNotification = (message: string, severity: AlertColor) => {
        dispatch({
            type: ReduxActionTypes.ChangeNotification,
            payload: new CustomNotificationPayload(message, severity)
        });
        dispatch({type: ReduxActionTypes.DisplayNotification, payload: true});
    }

    return displayNotification;
}