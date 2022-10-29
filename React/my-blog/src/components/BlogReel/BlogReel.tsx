import { Box,  CircularProgress, Paper, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { ApplicationState } from '../../redux';
import { postApi } from '../../shared/api/http/api';
import { CursorPagedRequest, CursorPagedResult } from '../../shared/api/types/paging/cursorPaging';
import { PostModel } from '../../shared/api/types/post';
import { PostCard } from '../PostCard';
import { BlogReelProps } from './BlogReelProps';
import EditIcon from '@mui/icons-material/Edit';
import { Waypoint } from 'react-waypoint';
import { AxiosResponse } from 'axios';

const BlogReel = ({reelWidth,pagingConditions: pagingRequestDefault}:BlogReelProps) => {
    
    const isAuthorized: boolean = useSelector<ApplicationState,boolean>(state => state.isAuthorized);

    const [isLoading,setLoading] = useState<boolean>(true);
    const [posts,setPosts] = useState<PostModel[]>([]);
    const [pagingRequest,setPagingRequest] = useState<CursorPagedRequest>(pagingRequestDefault);

    const fetchPosts = (pagingRequest: CursorPagedRequest) => postApi.getCursorPagedPosts(pagingRequest).then((result: AxiosResponse<CursorPagedResult<PostModel>>) => result.data);
    
    const loadMorePosts = (request: CursorPagedRequest) => {
        setLoading(true);
        console.log(request);
        fetchPosts(request).then((result) => {
            setPosts(posts.concat(result.items));
            setPagingRequest({...pagingRequest,pivotElementId: result.tailElementId});
            setLoading(false);
            console.log(result.items);
        });
        
    }

    
    useEffect(() => {
        fetchPosts(pagingRequestDefault).then((result) => {
            loadMorePosts(pagingRequest);
        });
    },[]);



    return (isLoading && posts.length === 0 ?
        <Box style={{margin:"50px auto",width:"fit-content"}}>
            <CircularProgress/>
        </Box>
        :
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
                <Waypoint bottomOffset="-20px" onEnter={() => loadMorePosts(pagingRequest)}></Waypoint>
                {isLoading && 
                    <Box style={{margin:"50px auto",width:"fit-content"}}>
                        <CircularProgress/>
                    </Box>
                }
            </>
    );
}
export {BlogReel};