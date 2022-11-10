import {Outlet} from 'react-router-dom';
import {Header} from '../../components/Header';
import {CustomSnackbar} from "../../components/CustomSnackbar";
import {ApplicationState, CustomNotificationPayload, ReduxActionTypes} from "../../redux";
import {useDispatch, useSelector} from "react-redux";

const Layout = () => {

    const notificationProps: CustomNotificationPayload = useSelector<ApplicationState, CustomNotificationPayload>(s => new CustomNotificationPayload(s.notificationText, s.notificationSeverity));

    const notificationRaised: boolean = useSelector<ApplicationState, boolean>(s => s.isCurrentlyNotifying);

    const dispatch = useDispatch();

    const closeNotificationHandler = () => {
        dispatch({type: ReduxActionTypes.DisplayNotification, payload: false})
    }

    return (
        <>
            <div style={{height: "5vh", minHeight: "70px"}}></div>
            <Header/>
            <main>
                <CustomSnackbar isOpen={notificationRaised} alertMessage={notificationProps.message}
                                closeHandler={closeNotificationHandler} alertType={notificationProps.severity}/>
                <Outlet/>
            </main>
        </>
    );
};

export {Layout};