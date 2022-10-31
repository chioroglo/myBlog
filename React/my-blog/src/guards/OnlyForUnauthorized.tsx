import React from 'react';
import {useSelector} from 'react-redux';
import {Navigate, useLocation} from 'react-router-dom';
import {ApplicationState} from '../redux';

const OnlyForUnauthorized = ({children}: { children: JSX.Element }) => {

    const isAuthorized = useSelector<ApplicationState>(state => state.isAuthorized);
    const location = useLocation();

    const fromPage = location.state?.from?.pathname || "/";

    if (!isAuthorized) {
        return children
    } else {
        return <Navigate to={fromPage} state {...{from: location}} replace/>
    }
};

export default OnlyForUnauthorized;