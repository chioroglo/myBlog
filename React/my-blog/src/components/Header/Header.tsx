import {Avatar, Box, Button, IconButton, Toolbar, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {ApplicationState} from '../../redux';
import {CustomNavbar} from '../CustomNavbar/CustomNavbar';
import {CustomNavLink} from '../CustomNavLink';
import MenuIcon from '@mui/icons-material/Menu';
import {authApi, userApi} from '../../shared/api/http/api'
import {Link} from 'react-router-dom';
import {UserModel} from '../../shared/api/types/user';
import {AccountMenuDropdown} from '../AccountMenuDropdown';
import {getFirstCharOfStringUpperCase} from '../../shared/assets';


const Header = () => {


    /* REWRITE IT USING useAuthorizedUserInfo HOOK */
    const isAuthorized: boolean = useSelector<ApplicationState, boolean>(state => state.isAuthorized);
    const [user, setUser] = useState<UserModel>();
    const [avatarLink, setAvatarLink] = useState<string>("");


    const fetchUser = () => authApi.getCurrent().then(response => {
        setUser(response.data);
        return response.data
    });
    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    useEffect(() => {
        if (isAuthorized) {
            fetchUser().then((userInfo) => fetchAvatarUrl(userInfo.id));
        }
    }, [isAuthorized]);

    return (
        <CustomNavbar>
            <Toolbar>
                <IconButton size="large" edge="start" aria-label="menu">
                    <MenuIcon/>
                </IconButton>

                <Box sx={{flexGrow: 1}} style={{display: "flex", justifyContent: "flex-start"}}>

                    <CustomNavLink to={"/"}>
                        Home
                    </CustomNavLink>

                </Box>

                {
                    isAuthorized ?
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