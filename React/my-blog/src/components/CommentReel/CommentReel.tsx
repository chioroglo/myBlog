import React, {useEffect, useState} from 'react';
import {CommentReelProps} from "./CommentReelProps";
import {CommentModel} from "../../shared/api/types/comment";
import {CursorPagedRequest, CursorPagedResult} from "../../shared/api/types/paging/cursorPaging";
import {commentApi} from "../../shared/api/http/api";
import {AxiosResponse} from "axios";
import {Box, CircularProgress, IconButton, Typography} from '@mui/material';
import {CommentCard} from "../CommentCard/CommentCard";
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';

const CommentReel = ({reelWidth = "100%", pagingRequestDefault}: CommentReelProps) => {

    const [comments, setComments] = useState<CommentModel[]>([]);
    const [isLoading, setLoading] = useState<boolean>(true);
    const [pagingRequest, setPagingRequest] = useState<CursorPagedRequest>(pagingRequestDefault);
    const [noMoreComments, setNoMoreComments] = useState<boolean>(false);


    const fetchComments = (pagingRequest: CursorPagedRequest) =>
        commentApi.getCursorPagedComments(pagingRequest).then((result: AxiosResponse<CursorPagedResult<CommentModel>>) => result.data);


    const loadMorePosts = () => {
        if (noMoreComments) {
            return;
        }
        setLoading(true);
        fetchComments(pagingRequest).then((result) => {

            setComments([...comments, ...result.items]);
            setPagingRequest({...pagingRequest, pivotElementId: result.tailElementId});

            if (result.items.length < pagingRequest.pageSize) {
                setNoMoreComments(true);
            }

            setLoading(false);
        });
    }

    useEffect(() => {
        loadMorePosts();
    }, []);


    return (
        <>
            {isLoading && comments.length === 0
                ?
                <Box style={{margin: "0 auto", width: "fit-content"}}>
                    <CircularProgress/>
                </Box>
                :
                <div>
                    {comments.length === 0
                        ?
                        <div>there is no comments</div>
                        :
                        <>
                            {
                                comments.map((comment) => <CommentCard key={comment.id} comment={comment}
                                                                       width={"100%"}/>)
                            }
                            {
                                isLoading &&
                                <Box style={{margin: "0 auto", width: reelWidth}}>
                                    <CircularProgress/>
                                </Box>
                            }
                            {
                                !noMoreComments &&
                                <div style={{display: "flex", alignItems: "flex-start"}}>
                                    <IconButton onClick={() => loadMorePosts()}>
                                        <ArrowDownwardIcon fontSize={"large"}/>
                                    </IconButton>
                                    <Typography style={{margin: "auto 0"}}>Please,push the arrow to load more
                                        comments</Typography>
                                </div>
                            }
                        </>
                    }
                </div>}
        </>
    )
};

export {CommentReel};