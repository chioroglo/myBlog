import {
    Avatar,
    Card,
    CardActions,
    CardContent,
    CardHeader,
    Chip,
    Collapse,
    IconButton,
    Typography
} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {PostCardProps} from './PostCardProps';
import MoreVertIcon from "@mui/icons-material/MoreVert";
import * as assets from '../../shared/assets';
import CommentIcon from '@mui/icons-material/Comment';
import {userApi} from '../../shared/api/http/api';
import {CommentReel} from "../CommentReel";
import {DefaultPageSize} from "../../shared/config";
import {FilterLogicalOperator} from "../../shared/api/types/paging";
import {CursorPagedRequest} from "../../shared/api/types/paging/cursorPaging";
import {ExpandMoreCard} from './ExpandMoreCard';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import {PostReactionBox} from "../PostReactionButton";

const PostCard = ({post, width = "100%", commentPortionSize = DefaultPageSize}: PostCardProps) => {

    /* TODO MAKE POSSIBILITY TO ADD MORE REACTIONS */
    /* TODO MAKE POSSIBILITY TO ADD MORE REACTIONS */
    /* TODO MAKE POSSIBILITY TO ADD MORE REACTIONS */
    /* TODO MAKE POSSIBILITY TO ADD MORE REACTIONS */
    /* TODO MAKE POSSIBILITY TO ADD MORE REACTIONS */

    const commentsPagingRequestDefault: CursorPagedRequest = {
        pageSize: commentPortionSize,
        getNewer: false,
        requestFilters: {
            logicalOperator: FilterLogicalOperator.And,
            filters: [
                {
                    path: "PostId",
                    value: post.id.toString()
                }
            ]
        }
    }

    const [avatarLink, setAvatarLink] = useState("");
    const [commentsOpen, setCommentsOpen] = useState<boolean>(false);

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    const handleExpandCommentSection = () => {
        setCommentsOpen(!commentsOpen);
    }

    useEffect(() => {
        fetchAvatarUrl(post.authorId);
    }, []);

    return (
        <>

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
                    {post.topic &&
                        <Chip style={{display: "block", width: "fit-content", padding: "5px 5px"}} onClick={() => {
                        }} variant="outlined" color={"primary"} label={"#" + post.topic}/>}
                    {post.content}

                </CardContent>


                <CardActions>

                    <PostReactionBox postId={post.id}/>

                    <IconButton onClick={() => setCommentsOpen(true)} style={{display: "flex"}} aria-label="comments">
                        <CommentIcon/>
                        {post.amountOfComments}
                    </IconButton>

                    <ExpandMoreCard expanded={commentsOpen} onClick={() => handleExpandCommentSection()}>
                        <ExpandMoreIcon/>
                    </ExpandMoreCard>

                </CardActions>

                <Collapse in={commentsOpen} orientation={"vertical"} timeout={"auto"}>
                    <CardContent>
                        <CommentReel reelWidth={"100%"} pagingRequestDefault={commentsPagingRequestDefault}/>
                    </CardContent>
                </Collapse>
            </Card>
        </>
    )
};

export {PostCard};