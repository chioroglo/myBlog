import React, {useEffect, useState} from 'react';
import {CommentCardProps} from "./CommentCardProps";
import {getFirstCharOfStringUpperCase, transformToDateMonthHoursMinutesString} from "../../shared/assets";
import {Avatar, Box, Card, CardContent, CardHeader, IconButton, Typography} from '@mui/material';
import {userApi} from "../../shared/api/http/api";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import {PostMentioningInline} from "../PostMentioningInline";
import {UserMentioningInline} from "../UserMentioningInline";

const CommentCard = ({width = "100%", comment}: CommentCardProps) => {

    const [avatarLink, setAvatarLink] = useState("");
    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    useEffect(() => {
        fetchAvatarUrl(comment.authorId);
    }, []);


    return (
        <Card elevation={10} style={{width: width, margin: "20px auto", minHeight: "fit-content"}}>
            <CardHeader
                avatar={<Avatar src={avatarLink}>{getFirstCharOfStringUpperCase(comment.authorUsername)}</Avatar>}
                title={<span>
                    <Typography style={{display: "inline"}}>
                        <UserMentioningInline userId={comment.authorId} username={comment.authorUsername}/>
                        <span style={{fontStyle: "italic"}}>{` replied to `}</span>
                        <PostMentioningInline postId={comment.postId} postTitle={comment.postTitle}/>
                    </Typography>
                </span>}
                action={<IconButton><MoreVertIcon/></IconButton>}
                subheader={transformToDateMonthHoursMinutesString(new Date(comment.registrationDate))}/>

            <CardContent style={{display: "block"}}>
                <Box style={{wordWrap: "break-word"}}>
                    {comment.content}
                </Box>
            </CardContent>
        </Card>
    );

};

export {CommentCard};