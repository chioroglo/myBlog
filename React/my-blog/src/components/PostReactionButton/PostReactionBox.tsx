import React, {useEffect, useRef, useState} from 'react';
import {Avatar, AvatarGroup, Box, Button, IconButton, Menu} from "@mui/material";
import {useSelector} from "react-redux";
import {ApplicationState} from "../../redux";
import {useAuthorizedUserInfo} from "../../hooks";
import {PostReactionModel} from "../../shared/api/types/postReaction/PostReactionModel";
import {PostReactionBoxProps} from "./PostReactionBoxProps";
import {postReactionApi, userApi} from "../../shared/api/http/api";
import {AxiosResponse} from "axios";
import {ReactionType} from "../../shared/api/types";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import ThumbUpOutlinedIcon from "@mui/icons-material/ThumbUpOutlined";
import ThumbDownOutlinedIcon from "@mui/icons-material/ThumbDownOutlined";
import FavoriteIcon from "@mui/icons-material/Favorite";
import ThumbUpIcon from "@mui/icons-material/ThumbUp";
import ThumbDownIcon from "@mui/icons-material/ThumbDown";
import AuthorizationRequiredCustomModal from "../CustomModal/AuthorizationRequiredCustomModal";
import {DefaultAvatarGroupMaxLength} from "../../shared/config";

const popover: React.CSSProperties = {
    pointerEvents: "none"
}

const popoverContent: React.CSSProperties = {
    pointerEvents: "auto"
}

const PostReactionBox = ({postId, ...props}: PostReactionBoxProps) => {

    const isAuthorized = useSelector<ApplicationState>(state => state.isAuthorized);
    const user = useAuthorizedUserInfo();
    const [reactions, setReactions] = useState<PostReactionModel[]>([]);
    const [userReaction, setUserReaction] = useState<{ exists: boolean, type?: ReactionType }>({exists: false});
    const [modalOpen, setModalOpen] = useState<boolean>(false);
    const [avatarUrls,setAvatarUrls] = useState<string[]>([]);
    const [userAvatar,setUserAvatar] = useState<string>();
    const [isAvatarsLoading,setAvatarsLoading] = useState<boolean>(true);
    const [isReactionsLoading,setIsReactionsLoading] = useState<boolean>(true);

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

        return result.data
    });


    const handleNewReaction = (type: ReactionType) => {
        if (!isAuthorized) {
            setModalOpen(true);
            return;
        }

        if (userReaction.exists) {
            postReactionApi.removeReactionFromPost(postId).then(() => {
                postReactionApi.reactToPost({postId: postId, reactionType: type}).then(() => {
                    setUserReaction({exists: true, type: type});
                });
            })
        } else {
            postReactionApi.reactToPost({postId: postId, reactionType: type}).then(() => {
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
            postReactionApi.removeReactionFromPost(postId).then(() => {
                setUserReaction({exists: false})
            });
        }
    }

    // this system is based on order of reaction types like in enum.
    const reactionsAndComponentsUnactive = [
        <IconButton onClick={() => handleNewReaction(ReactionType.Like)} children={<ThumbUpOutlinedIcon/>}/>,
        <IconButton onClick={() => handleNewReaction(ReactionType.Dislike)} children={<ThumbDownOutlinedIcon/>}/>,
        <IconButton onClick={() => handleNewReaction(ReactionType.Love)} children={<FavoriteBorderIcon/>}/>
    ]

    const reactionsAndComponentsActive = [
        <IconButton onClick={() => handleRemoveReaction()} children={<ThumbUpIcon color={"info"}/>}/>,
        <IconButton onClick={() => handleRemoveReaction()} children={<ThumbDownIcon color={"warning"}/>}/>,
        <IconButton onClick={() => handleRemoveReaction()} children={<FavoriteIcon color={"error"}/>}/>
    ]

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then((result:AxiosResponse<string>) => result.data);

    useEffect(() => {
        setIsReactionsLoading(true);
        fetchPostReactions(postId).then((result) => {
            setIsReactionsLoading(false);
            const avatars: string[] = [];
            const groupLength = DefaultAvatarGroupMaxLength > result.length ? result.length : DefaultAvatarGroupMaxLength;


            for (let i = 0 ; i < groupLength ; i++)
            {
                fetchAvatarUrl(result[i].userId).then((link) => (result[i].userId !== (user?.id || 0)) && avatars.push(link));
            }

            setAvatarUrls(avatars);
        });

        if (isAuthorized && user) {
            fetchAvatarUrl(user?.id).then(result => setUserAvatar(result));
        }
        setAvatarsLoading(false);
    }, []);

    return (
        <>
            <AuthorizationRequiredCustomModal modalOpen={modalOpen} setModalOpen={setModalOpen}
                                              caption={"Please sign up to share your thoughts"}/>
            {
                !isReactionsLoading
                    &&
                <Box style={{display: "flex", flexDirection: "row"}}>

                    <Box>
                        {
                            userReaction.exists && userReaction.type
                                ?
                                <IconButton>
                                    {reactionsAndComponentsActive[userReaction.type - 1]}
                                </IconButton>
                                :
                                <IconButton>
                                    {reactionsAndComponentsUnactive[ReactionType.Love - 1]}
                                </IconButton>
                        }
                    </Box>

                    <Box style={{margin: "auto 0"}}>
                        {reactionsAndComponentsUnactive[ReactionType.Like - 1]}
                        {reactionsAndComponentsUnactive[ReactionType.Dislike - 1]}
                        {reactionsAndComponentsUnactive[ReactionType.Love - 1]}
                    </Box>

                    <AvatarGroup total={reactions.length}>
                        {
                            userReaction.exists && <Avatar src={userAvatar}/>
                        }
                        {
                            isAvatarsLoading ? new Array(DefaultAvatarGroupMaxLength).map(() =>
                                <Avatar/>) : avatarUrls.map(link => <Avatar src={link}/>)
                        }
                    </AvatarGroup>
                </Box>
            }
        </>
    )

};

export {PostReactionBox};
