import {Avatar, Card, CardActions, CardContent, CardHeader, Chip, IconButton, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {PostCardProps} from './PostCardProps';
import MoreVertIcon from "@mui/icons-material/MoreVert";
import * as assets from '../../shared/assets';
import CommentIcon from '@mui/icons-material/Comment';
import {postApi, userApi} from '../../shared/api/http/api';
import {useSelector} from 'react-redux';
import {ApplicationState} from '../../redux';
import {useAuthorizedUserInfo} from "../../hooks";
import {PostReactionModel} from "../../shared/api/types/postReaction/PostReactionModel";
import AuthorizationRequiredCustomModal from "../CustomModal/AuthorizationRequiredCustomModal";
import {ReactionType} from "../../shared/api/types";

const PostCard = ({post, width = "100%"}: PostCardProps) => {

    const isAuthorized = useSelector<ApplicationState>(state => state.isAuthorized);

    const user = useAuthorizedUserInfo();
    const [avatarLink, setAvatarLink] = useState("");
    const [reactions, setReactions] = useState<PostReactionModel[]>([]);

    const [modalOpen, setModalOpen] = useState<boolean>(false);

    useEffect(() => {
        fetchAvatarUrl(post.authorId);
        fetchPostReactions(post.id);
    }, []);

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    const fetchPostReactions = (postId: number) => postApi.getReactionsByPost(postId).then(response => setReactions(response.data));

    const userHasReaction = (): boolean => isAuthorized ? reactions.filter((value) => value.userId === user?.id).length !== 0 : false;

    const handleReactionClick = (type: ReactionType) => {
        if (!isAuthorized) {
            setModalOpen(true);
            return;
        }
    }


    return (
        <>
            <AuthorizationRequiredCustomModal modalOpen={modalOpen} setModalOpen={setModalOpen}
                                              caption={"Please sign up to share your thoughts"}></AuthorizationRequiredCustomModal>

            <Card elevation={10} style={{width: width, margin: "20px auto"}}>

                <CardHeader
                    avatar={<Avatar src={avatarLink}>{assets.getFirstCharOfString(post.authorUsername)}</Avatar>}
                    action={<IconButton><MoreVertIcon/></IconButton>}
                    title={post.authorUsername}
                    subheader={assets.transformToDateMonthHourseMinutesString(new Date(post.registrationDate))}/>


                <CardContent>
                    <Typography variant="h5">{post.title}</Typography>
                    {/* todo add redirect to posts of this topic*/}
                    {post.topic && <Chip onClick={() => {
                    }} style={{display: "block", width: "fit-content", padding: "5px 5px"}} variant="outlined"
                                         label={"#" + post.topic}/>}
                    {post.content}
                </CardContent>


                <CardActions>
                    <IconButton onClick={() => handleReactionClick(ReactionType.Love)} aria-label="reaction">
                        {
                            userHasReaction() ? "has" : "has not"
                        }
                    </IconButton>

                    <IconButton aria-label="comments">
                        <CommentIcon/>
                    </IconButton>
                    {post.amountOfComments}
                </CardActions>

            </Card>
        </>
    )
};

export {PostCard};