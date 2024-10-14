import {Avatar, Box, Button, Paper, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {ApplicationState} from '../../redux';
import {userApi} from '../../shared/api/http/api';
import {ProfileHeaderProps} from './ProfileHeaderProps';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import ScheduleIcon from '@mui/icons-material/Schedule';
import {
    getFirstCharOfStringUpperCase,
    transformUtcStringToDateMonthHoursMinutesString,
    transformUtcToLocalDate
} from '../../shared/assets';
import {EditProfileCustomModal} from "../CustomModal";
import {UserInfoCache} from "../../shared/types";
import "../ProfileHeader/ProfileHeader.scss";
import { WarningProfileBox } from './WarningsProfileBox/WarningProfileBox';

const avatarStyle: React.CSSProperties = {
    minWidth: "115px",
    minHeight: "115px",
    maxWidth: "320px",
    maxHeight: "320px",
    marginTop: "-50%",
    width: "20vw",
    height: "20vw"
}

const internalMarginOfDialog: string = "0 32px";

const ProfileHeader = ({user, setUser}: ProfileHeaderProps) => {


    const authorizedUser = useSelector<ApplicationState, (UserInfoCache | null)>(state => state.user);

    const isUserOnHisProfilePage = (): boolean => {
        if (user) {
            return user && (authorizedUser?.id === user.id);
        } else {
            return false;
        }

    };


    const [editProfileModalOpen, setEditProfileModalOpen] = useState<boolean>(false);

    const [avatarLink, setAvatarLink] = useState<string>("");

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    useEffect(() => {
        if (user) {
            fetchAvatarUrl(user.id);
        }
    }, []);

    return (
        <>
            <EditProfileCustomModal modalOpen={editProfileModalOpen} setModalOpen={setEditProfileModalOpen}
                                    user={user}
                                    setUser={setUser}/>

            <Paper className="profile-header-wrapper">
                <Paper elevation={0} className="profile-header-upper-background"/>

                <Box style={{
                    margin: internalMarginOfDialog,
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center"
                }}>
                    <div>
                        <Avatar style={avatarStyle} src={avatarLink}
                                sx={{fontSize: "128px"}}>{getFirstCharOfStringUpperCase(user.username)}</Avatar>
                    </div>

                    <div>
                        {isUserOnHisProfilePage() &&
                            <Button variant="outlined" onClick={() => setEditProfileModalOpen(true)}>Edit
                                profile</Button>}
                    </div>
                </Box>

                <Box style={{
                    margin: internalMarginOfDialog,
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "self-start",
                    flexDirection: "column"
                }}>

                    <Typography variant="h2">{user.username}</Typography>

                    {user.fullName.length !== 0 && <Typography variant="h6">Name: {user.fullName}</Typography>}

                    <Box style={{margin: "32px 0 0 0", display: "flex"}}>

                        <Box style={{margin: "0 32px 0 0"}}>
                            <CalendarMonthIcon/>
                            <Typography>
                                Joined on {transformUtcToLocalDate(user.registrationDate)}
                            </Typography>
                        </Box>

                        <Box>
                            <ScheduleIcon/>
                            <Typography>
                                Last activity {transformUtcStringToDateMonthHoursMinutesString(user.lastActivity)}
                            </Typography>
                        </Box>

                    </Box>
                    <WarningProfileBox isBanned={user.isBanned} warnings={user.activeWarnings}/>
                </Box>
            </Paper>
        </>
    );
};

export {ProfileHeader};