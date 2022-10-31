import {
    Avatar,
    Card,
    CardActions,
    CardContent,
    CardHeader,
    Chip,
    Icon,
    IconButton,
    SvgIcon,
    Typography
} from '@mui/material';
import React, { forwardRef, useEffect, useState } from 'react';
import { PostCardProps } from './PostCardProps';
import MoreVertIcon from "@mui/icons-material/MoreVert";
import * as assets from '../../shared/assets';
import FavoriteIcon from "@mui/icons-material/Favorite";
import CommentIcon from '@mui/icons-material/Comment';
import {postApi, userApi} from '../../shared/api/http/api';
import { useSelector } from 'react-redux';
import { ApplicationState } from '../../redux';
import {palette} from "../../shared/assets";
import {imagePaths} from "../../shared/assets/reactionIcons";
import {Image} from "@mui/icons-material";

const PostCard = ({post,width= "100%"}: PostCardProps) => {

    const isAuthorized = useSelector<ApplicationState>(state => state.isAuthorized);
    
    const [avatarLink,setAvatarLink] = useState("");
    const [reactions,setReactions] = useState([]);

    useEffect(() => {
        fetchAvatarUrl(post.authorId);
        fetchPostReactions(post.id);
    },[]);

    const fetchAvatarUrl = (userId: number) => userApi.getAvatarUrlById(userId).then(response => setAvatarLink(response.data));

    const fetchPostReactions = (postId: number) => postApi.getReactionsByPost(postId).then(response => setReactions(response.data));

    return(
        <Card elevation={10} style={{width: width,margin:"20px auto"}}>
            <CardHeader 
            avatar={<Avatar src={avatarLink}>{assets.getFirstCharOfString(post.authorUsername)}</Avatar>}
            action={<IconButton><MoreVertIcon/></IconButton>}
            title={post.authorUsername}
            subheader={assets.transformToDateMonthHourseMinutesString(new Date(post.registrationDate))}/>
            
    
            <CardContent>
                <Typography variant="h5">{post.title}</Typography>
                {/* todo add redirect to posts of this topic*/}
                {post.topic && <Chip onClick={() => {}} style={{display:"block",width:"fit-content",padding:"5px 5px"}} variant="outlined" label={"#" + post.topic}/>}
                {post.content}
            </CardContent>


            <CardActions>
                <IconButton aria-label="reaction">
                    <Icon>
                        <svg></svg>
                    </Icon>
                </IconButton>

                <IconButton aria-label="comments">
                    <CommentIcon/>
                </IconButton>
                {post.amountOfComments}
            </CardActions>
        </Card>
    )
};

export {PostCard};