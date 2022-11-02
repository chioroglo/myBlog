import React, {useEffect, useState} from 'react';
import {CommentCardProps} from "./CommentCardProps";
import {getFirstCharOfStringUpperCase, transformToDateMonthHourseMinutesString} from "../../shared/assets";
import {Avatar, Card, CardContent, CardHeader, IconButton, Typography} from '@mui/material';
import {userApi} from "../../shared/api/http/api";
import MoreVertIcon from "@mui/icons-material/MoreVert";

const CommentCard = ({width = "100%", comment}: CommentCardProps) => {

    const [avatarLink, setAvatarLink] = useState("");

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    useEffect(() => {
        fetchAvatarUrl(comment.authorId);
    }, []);


    return (
        <Card>
            <CardHeader
                avatar={<Avatar src={avatarLink}>{getFirstCharOfStringUpperCase(comment.authorUsername)}</Avatar>}
                title={comment.authorUsername}
                action={<IconButton><MoreVertIcon/></IconButton>}
                subheader={transformToDateMonthHourseMinutesString(new Date(comment.registrationDate))}/>

            <CardContent style={{height:"fit-content"}}>
                <Typography style={{fontSize:"16px",display:"block"}}>
                    {comment.content}
                </Typography>
            </CardContent>
        </Card>
    );

};

export {CommentCard};