import {Avatar, Box, Button, Toolbar, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {ApplicationState} from '../../redux';
import {CustomNavbar} from '../CustomNavbar/CustomNavbar';
import {CustomNavLink} from '../CustomNavLink';
import {userApi} from '../../shared/api/http/api'
import {Link} from 'react-router-dom';
import {AccountMenuDropdown} from '../AccountMenuDropdown';
import {getFirstCharOfStringUpperCase} from '../../shared/assets';
import {useAuthorizedUserInfo} from "../../hooks";


const Header = () => {


    const isAuthorized: boolean = useSelector<ApplicationState, boolean>(state => state.isAuthorized);
    const user = useAuthorizedUserInfo();
    const [avatarLink, setAvatarLink] = useState<string>("");

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    useEffect(() => {
        if (isAuthorized && user) {
            fetchAvatarUrl(user?.id);
        }
    }, [isAuthorized]);

    return (
        <CustomNavbar>
            <Toolbar>

                <Box sx={{flexGrow: 1}} style={{display: "flex", justifyContent: "flex-start"}}>

                    <CustomNavLink to={"/"}>
                        Home
                    </CustomNavLink>

                </Box>

                {
                    isAuthorized && user ?
                        <div>
                            <Typography display={"inline"}>Welcome, {user?.username}!</Typography>
                            <AccountMenuDropdown icon={<Avatar
                                src={avatarLink}>{getFirstCharOfStringUpperCase(user?.username)}</Avatar>}></AccountMenuDropdown>
                        </div>
                        :
                        <Button variant="contained" component={Link} to="/login">Login</Button>
                }
            </Toolbar>
        </CustomNavbar>
    );
};

export {Header};