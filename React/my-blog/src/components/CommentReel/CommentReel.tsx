import React, {useEffect, useState} from 'react';
import {CommentReelProps} from "./CommentReelProps";
import {CommentDto, CommentModel} from "../../shared/api/types/comment";
import {CursorPagedRequest, CursorPagedResult} from "../../shared/api/types/paging/cursorPaging";
import {commentApi} from "../../shared/api/http/api";
import {AxiosResponse} from "axios";
import {Box, Button, CircularProgress, IconButton, Typography} from '@mui/material';
import {CommentCard} from "../CommentCard/CommentCard";
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import {useSelector} from 'react-redux';
import {ApplicationState} from "../../redux";
import {useAuthorizedUserInfo} from "../../hooks";
import {CommentForm} from "../CommentForm";
import {EmptyReelPlate} from '../EmptyReelPlate';


const CommentReel = ({reelWidth = "100%", pagingRequestDefault, post}: CommentReelProps) => {

    const isAuthorized = useSelector<ApplicationState>(s => s.isAuthorized);

    const user = useAuthorizedUserInfo();

    const [comments, setComments] = useState<CommentModel[]>([]);
    const [isLoading, setLoading] = useState<boolean>(true);
    const [pagingRequest, setPagingRequest] = useState<CursorPagedRequest>(pagingRequestDefault);
    const [noMoreComments, setNoMoreComments] = useState<boolean>(false);
    const [newCommentFormEnabled, setNewCommentForm] = useState<boolean>(true);


    const handleOpenNewCommentForm = () => setNewCommentForm(true);

    const handleCloseNewCommentForm = () => setNewCommentForm(false);

    const fetchComments = (pagingRequest: CursorPagedRequest) =>
        commentApi.getCursorPagedComments(pagingRequest).then((result: AxiosResponse<CursorPagedResult<CommentModel>>) => result.data);

    const handleDeleteComment = (commentToDeleteId: number) => {
        setComments(comments.filter((comment) => comment.id !== commentToDeleteId))
    };


    const loadMoreComments = () => {
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
        loadMoreComments();
    }, []);

    const handleAddNewComment = async (comment: CommentDto) => {
        setLoading(true);
        return commentApi.addComment(comment).then((result: AxiosResponse<CommentModel>) => {
            if (result.status === 200 && user) {
                result.data.authorUsername = user?.username;
                result.data.authorId = user?.id;

                if (post) {
                    result.data.postTitle = post.title;
                }

                setComments([result.data, ...comments]);
            }
            setLoading(false);
            return result;
        }).catch(result => result);
    }


    return (
        <>
            {isAuthorized && post && (newCommentFormEnabled ?
                    <CommentForm caption="New Comment" formActionCallback={handleAddNewComment}
                                 formCloseHandler={handleCloseNewCommentForm} post={post}/>
                    :
                    <Box width={"100%"} style={{margin: "0 auto", width: "fit-content"}}>
                        <Button variant="outlined" onClick={handleOpenNewCommentForm}>Add new comment</Button>
                    </Box>
            )}
            {isLoading && comments.length === 0
                ?
                <Box style={{margin: "0 auto", width: "fit-content"}}>
                    <CircularProgress/>
                </Box>
                :
                <div>
                    {comments.length === 0
                        ?
                        <EmptyReelPlate width={reelWidth}/>
                        :
                        <>
                            {
                                comments.map((comment) => <CommentCard key={comment.id} initialComment={comment}
                                                                       width={"100%"}
                                                                       disappearCommentCallback={() => handleDeleteComment(comment.id)}/>)
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
                                    <IconButton onClick={() => loadMoreComments()}>
                                        <ArrowDownwardIcon fontSize={"large"}/>
                                    </IconButton>
                                    <Typography style={{margin: "auto 0"}}>Please,push the arrow to load more
                                        comments</Typography>
                                </div>
                            }
                        </>
                    }
                </div>
            }
        </>)
};

export {CommentReel};