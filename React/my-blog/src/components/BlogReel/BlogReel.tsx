import { Box, Button, CircularProgress, Paper, Typography } from '@mui/material';
import React, { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { ApplicationState } from '../../redux';
import { postApi } from '../../shared/api/http/api';
import { CursorPagedRequest } from '../../shared/api/types/paging/cursorPaging';
import { PostModel } from '../../shared/api/types/post';
import { DefaultPageSize } from '../../shared/config';
import { PostCard } from '../PostCard';
import { BlogReelProps } from './BlogReelProps';
import EditIcon from '@mui/icons-material/Edit';
import { Waypoint } from 'react-waypoint';

const BlogReel = ({reelWidth,pagingConditions: pagingRequest}:BlogReelProps) => {
    
    const isAuthorized: boolean = useSelector<ApplicationState,boolean>(state => state.isAuthorized);

    const [isLoading,setIsLoading] = useState<boolean>(true);
    const [posts,setPosts] = useState<PostModel[]>([]);
    const [lastPostId,setLastPostId] = useState<number>(0);


    const fetchPosts = (pagingRequest: CursorPagedRequest) => postApi.getCursorPagedPosts(pagingRequest).then((result) => result.data);

    useEffect(() => {
        setIsLoading(true);

        fetchPosts(pagingRequest).then((response) => {
            console.log(response);
            setPosts(response.items);
            setLastPostId(response.tailElementId);
            setIsLoading(false);
        })
    },[]);

    const requireNewPostsAndUpdatePosts = () => {
        
        setIsLoading(true);
        pagingRequest.pivotElementId = lastPostId;
        
        
        fetchPosts(pagingRequest).then((response) => {
            console.log(response.items);
            setPosts(posts.concat(response.items));
            setLastPostId(response.tailElementId);
            setIsLoading(false);
        });

    }

    return isLoading && posts.length === 0 ?
        <Box style={{margin:"50px auto",width:"fit-content"}}>
            <CircularProgress/>
        </Box>
        :
        <>
        {
            posts.length === 0 ? 

            <Box width={reelWidth} style={{margin:"0 auto"}}>
                <Typography textAlign={"center"} variant="h5">
                    <EditIcon sx={{width:"100px",height:"100px",display:"block",margin:"0 auto"}}/>
                    Unfortunately,this reel is empty
                </Typography>
            </Box>
            :
            <>
                {posts.map((post) => <PostCard width={reelWidth} key={post.id} post={post}/>)}
                <Waypoint bottomOffset="-20px" onEnter={requireNewPostsAndUpdatePosts}></Waypoint>
                {isLoading && 
                    <Box style={{margin:"50px auto",width:"fit-content"}}>
                        <CircularProgress/>
                    </Box>
                }
            </>
            
        }
        </>
};

export {BlogReel};