import {Avatar, Card, CardActions, CardContent, CardHeader, Chip, IconButton, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {PostCardProps} from './PostCardProps';
import MoreVertIcon from "@mui/icons-material/MoreVert";
import * as assets from '../../shared/assets';
import CommentIcon from '@mui/icons-material/Comment';
import {postReactionApi, userApi} from '../../shared/api/http/api';
import {useSelector} from 'react-redux';
import {ApplicationState} from '../../redux';
import {useAuthorizedUserInfo} from "../../hooks";
import {PostReactionModel} from "../../shared/api/types/postReaction/PostReactionModel";
import AuthorizationRequiredCustomModal from "../CustomModal/AuthorizationRequiredCustomModal";
import {ReactionType} from "../../shared/api/types";
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import ThumbDownIcon from '@mui/icons-material/ThumbDown';
import {AxiosResponse} from "axios";
import ThumbDownOutlinedIcon from '@mui/icons-material/ThumbDownOutlined';
import ThumbUpOutlinedIcon from '@mui/icons-material/ThumbUpOutlined';

const PostCard = ({post, width = "100%"}: PostCardProps) => {

    const isAuthorized = useSelector<ApplicationState>(state => state.isAuthorized);

    const user = useAuthorizedUserInfo();
    const [avatarLink, setAvatarLink] = useState("");
    const [reactions, setReactions] = useState<PostReactionModel[]>([]);
    const [userReaction, setUserReaction] = useState<{ exists: boolean, type?: ReactionType }>({exists: false});

    const [modalOpen, setModalOpen] = useState<boolean>(false);

    const reactionsAndComponentsUnactive = {
        "Love": <FavoriteBorderIcon onClick={() => handleNewReaction(ReactionType.Love)}/>,
        "Like": <ThumbUpOutlinedIcon onClick={() => handleNewReaction(ReactionType.Like)}/>,
        "Dislike": <ThumbDownOutlinedIcon onClick={() => handleNewReaction(ReactionType.Dislike)}/>
    }

    const reactionsAndComponentsActive = {
        "Love": <FavoriteIcon color={"error"} onClick={() => handleRemoveReaction()}/>,
        "Like": <ThumbUpIcon color={"warning"} onClick={() => handleRemoveReaction()}/>,
        "Dislike": <ThumbDownIcon color={"warning"} onClick={() => handleRemoveReaction()}/>
    }

    useEffect(() => {
        fetchAvatarUrl(post.authorId);
    }, []);

    useEffect(() => {
        fetchPostReactions(post.id);
    }, [isAuthorized])

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    const fetchPostReactions = (postId: number) => postReactionApi.getReactionsByPost(postId).then((result: AxiosResponse<PostReactionModel[]>) => {
        setReactions(result.data);

        if (isAuthorized) {
            const reactionsFilteredByUserId = result.data.filter((val) => val.userId === user?.id)

            if (reactionsFilteredByUserId.length === 1) {
                setUserReaction({exists: true, type: reactionsFilteredByUserId[0].reactionType});
            } else {
                setUserReaction({exists: false});
            }
        } else {
            setUserReaction({exists: false});
        }
    });


    const handleNewReaction = (type: ReactionType) => {
        if (!isAuthorized) {
            setModalOpen(true);
            return;
        }

        if (userReaction.exists) {
            postReactionApi.removeReactionFromPost(post.id).then(() => {
                postReactionApi.reactToPost({postId: post.id, reactionType: type}).then(() => {
                    setUserReaction({exists: true, type: type});
                });
            })
        } else {
            postReactionApi.reactToPost({postId: post.id, reactionType: type}).then(() => {
                setUserReaction({exists: true, type: type});
            });
        }
    }

    const handleRemoveReaction = () => {
        if (!isAuthorized) {
            setModalOpen(true);
            return;
        }

        if (userReaction.exists) {
            postReactionApi.removeReactionFromPost(post.id).then(() => {
                setUserReaction({exists: false})
            });
        }
    }


    return (
        <>
            <AuthorizationRequiredCustomModal modalOpen={modalOpen} setModalOpen={setModalOpen}
                                              caption={"Please sign up to share your thoughts"}></AuthorizationRequiredCustomModal>

            <Card elevation={10} style={{width: width, margin: "20px auto"}}>

                <CardHeader
                    avatar={<Avatar
                        src={avatarLink}>{assets.getFirstCharOfStringUpperCase(post.authorUsername)}</Avatar>}
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
                    {
                        userReaction.exists ?
                            reactionsAndComponentsActive["Love"]
                            :
                            reactionsAndComponentsUnactive["Love"]
                    }

                    <IconButton style={{display: "flex",}} aria-label="comments">
                        <CommentIcon/>
                        {post.amountOfComments}
                    </IconButton>
                </CardActions>

            </Card>
        </>
    )
};

export {PostCard};