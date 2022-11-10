import React from 'react';
import ReactDOM from 'react-dom/client';
import {Provider} from 'react-redux';
import {BrowserRouter} from 'react-router-dom';
import {createStore} from 'redux';
import App from './App';
import {ApplicationState, CustomNotificationPayload, ReduxActionTypes} from './redux';
import {JwtTokenKeyName} from './shared/config';

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);


const defaultState: ApplicationState = {
    isAuthorized: localStorage.getItem(JwtTokenKeyName) !== null || sessionStorage.getItem(JwtTokenKeyName) !== null,
    isCurrentlyNotifying: false,
    notificationText: '',
    notificationSeverity: 'info'
}

const reducer = (state = defaultState, action: { type: string, payload: boolean | CustomNotificationPayload }) => {
    switch (action.type) {

        case ReduxActionTypes.AuthorizationState: {
            if (typeof action.payload === "boolean") {
                return {...state, isAuthorized: action.payload};
            }
            return state
        }
        case ReduxActionTypes.ChangeNotification: {
            if (action.payload instanceof CustomNotificationPayload) {
                return {
                    ...state,
                    notificationText: action.payload.message,
                    notificationSeverity: action.payload.severity
                };
            }
            return state;
        }

        case ReduxActionTypes.DisplayNotification: {
            if (typeof action.payload === "boolean") {
                return {...state, isCurrentlyNotifying: action.payload}
            }
            return state;
        }
        default:
            return state
    }
}

const store = createStore(reducer);


root.render(
    <Provider store={store}>
        <BrowserRouter>
            <App/>
        </BrowserRouter>
    </Provider>
);