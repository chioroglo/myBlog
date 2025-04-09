import { Avatar, Box, Button, Paper, Tooltip, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { ApplicationState } from '../../redux';
import { ProfileHeaderProps } from './ProfileHeaderProps';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import ScheduleIcon from '@mui/icons-material/Schedule';
import {
    transformUtcStringToDateMonthHoursMinutesString,
    transformUtcToLocalDate
} from '../../shared/assets';
import { EditProfileCustomModal } from "../CustomModal";
import { WarningProfileBox } from './WarningsProfileBox/WarningProfileBox';
import styles from './ProfileHeader.module.scss';
import { UserApi } from '../../shared/api/http/user-api';
import { ProfileSecurityModal } from '../CustomModal/ProfileSecurityModal/profile-security-modal';
import TuneIcon from '@mui/icons-material/Tune';

const ProfileHeader = ({user, setUser}: ProfileHeaderProps) => {
    const isUserOnHisProfilePage = useSelector<ApplicationState, boolean>(state => state.user !== undefined && state.user?.id === user.id);
    const [editProfileModalOpen, setEditProfileModalOpen] = useState<boolean>(false);
    const [editSecurityModalOpen, setEditSecurityModalOpen] = useState<boolean>(false);
    const [avatarLink, setAvatarLink] = useState<string>("");
    const fetchAvatarUrl = (userId: number) => UserApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    useEffect(() => {
        if (user) {
            fetchAvatarUrl(user.id);
        }
    }, []);

    return (
        <>
            <EditProfileCustomModal modalOpen={editProfileModalOpen} setModalOpen={setEditProfileModalOpen}
                                    user={user}
                                    setUser={setUser}
                                    title='Edit profile'/>
            
            <ProfileSecurityModal modalOpen={editSecurityModalOpen} setModalOpen={setEditSecurityModalOpen}/>

            <Paper className={styles.wrapper}>
                <Paper elevation={0} className={styles['upper-background']}/>

                <Box className={styles['upper-section']}>
                    <div>
                        <Avatar className={styles.avatar} src={avatarLink}>{user.initials}</Avatar>
                    </div>

                    <div className={styles['settings']}>
                        {isUserOnHisProfilePage &&
                            <>
                                <Tooltip title="Profile data settings">
                                    <TuneIcon/>
                                </Tooltip>
                                <Button variant="outlined" onClick={() => setEditProfileModalOpen(true)}>Personal</Button>
                                <Button variant="outlined" onClick={() => setEditSecurityModalOpen(true)}>Security</Button>
                            </>}
                    </div>
                </Box>

                <Box className={styles['lower-section']}>
                    <Typography variant="h2">{user.username}</Typography>
                    {
                        user.fullName.length !== 0 &&
                        <Typography variant="h6">Name: {user.fullName}
                        </Typography>
                    }
                    <Box className={styles['lower-section__activity']}>
                        <Box>
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