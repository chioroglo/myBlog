import React from 'react';
import {useSelector} from 'react-redux';
import {Navigate, useLocation} from 'react-router-dom';
import {ApplicationState} from '../redux';

const RequireAuth = ({children}: { children: JSX.Element }) => {

    const isAuthorized = useSelector<ApplicationState>(state => state.isAuthorized);
    const location = useLocation();

    if (isAuthorized) {
        return children
    } else {
        return <Navigate to="/login" state {...{from: location}} replace/>
    }
};

export {RequireAuth};