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
    transformToDateMonthHoursMinutesString,
    transformToDayMonthYear
} from '../../shared/assets';
import {useAuthorizedUserInfo} from "../../hooks";
import {EditProfileCustomModal} from "../CustomModal";


const paperStyle: React.CSSProperties = {
    width: "70vw",
    minWidth: "300px",
    margin: "0 auto",
}

const paperBackgroundStyle: React.CSSProperties = {
    backgroundImage: "radial-gradient(circle, rgba(238,174,202,1) 0%, rgba(148,187,233,1) 100%)",
    height: "200px",
    borderBottomLeftRadius: "0px",
    borderBottomRightRadius: "0px"
}


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

    const isAuthorized: boolean = useSelector<ApplicationState, boolean>(state => state.isAuthorized);

    const authorizedUser = useAuthorizedUserInfo();

    const isUserOnHisProfilePage = (): boolean => {
        return isAuthorized && (authorizedUser?.id === user.id);
    };


    const [editProfileModalOpen, setEditProfileModalOpen] = useState<boolean>(false);

    const [avatarLink, setAvatarLink] = useState<string>("");

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    useEffect(() => {
        fetchAvatarUrl(user.id);
    }, []);

    return (
        <>

            <EditProfileCustomModal modalOpen={editProfileModalOpen} setModalOpen={setEditProfileModalOpen} user={user}
                                    setUser={setUser}/>

            <Paper style={paperStyle}>
                <Paper elevation={0} style={paperBackgroundStyle}/>

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

                    <Typography variant="h2">{`${user.username} | №${user.id}`}</Typography>

                    {user.fullName.length !== 0 && <Typography variant="h6">Name: {user.fullName}</Typography>}

                    <Box style={{margin: "32px 0 0 0", display: "flex"}}>

                        <Box style={{margin: "0 32px 0 0"}}>
                            <CalendarMonthIcon/>
                            <Typography>
                                Joined on {transformToDayMonthYear(new Date(user.registrationDate))}
                            </Typography>
                        </Box>

                        <Box>
                            <ScheduleIcon/>
                            <Typography>
                                Last activity {transformToDateMonthHoursMinutesString(new Date(user.lastActivity))}
                            </Typography>
                        </Box>

                    </Box>

                </Box>
            </Paper>
        </>
    );
};

export {ProfileHeader};