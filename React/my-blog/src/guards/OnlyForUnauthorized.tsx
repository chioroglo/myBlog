import React from 'react';
import {useSelector} from 'react-redux';
import {Navigate, useLocation} from 'react-router-dom';
import {ApplicationState} from '../redux';
import {UserInfoCache} from "../shared/types";

const OnlyForUnauthorized = ({children}: { children: JSX.Element }) => {

    const location = useLocation();

    const user = useSelector<ApplicationState, (UserInfoCache | null)>(state => state.user);

    const fromPage = location.state?.from?.pathname || "/";

    if (!user) {
        return children
    } else {
        return <Navigate to={fromPage} state {...{from: location}} replace/>
    }
};

export default OnlyForUnauthorized;