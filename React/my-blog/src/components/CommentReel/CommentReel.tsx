import React, {useEffect, useState} from 'react';
import {CommentReelProps} from "./CommentReelProps";
import {DefaultPageSize} from "../../shared/config";
import {CommentModel} from "../../shared/api/types/comment";
import {CursorPagedRequest, CursorPagedResult} from "../../shared/api/types/paging/cursorPaging";
import {commentApi} from "../../shared/api/http/api";
import {AxiosResponse} from "axios";
import {Box, CircularProgress} from '@mui/material';
import {CommentCard} from "../CommentCard/CommentCard";
import {FilterLogicalOperator} from "../../shared/api/types/paging";

const CommentReel = ({reelWidth = "100%",pagingRequestDefault}: CommentReelProps) => {

    const [comments, setComments] = useState<CommentModel[]>([]);
    const [isLoading,setLoading] = useState<boolean>(true);
    const [pagingRequest,setPagingRequest] = useState<CursorPagedRequest>(pagingRequestDefault);

    const fetchComments = (pagingRequest: CursorPagedRequest) => commentApi.getCursorPagedComments(pagingRequest).then((result: AxiosResponse<CursorPagedResult<CommentModel>>) => result.data.items);

    useEffect(() => {
       fetchComments(pagingRequest).then((result) =>
       {
           setComments(result);
           console.log(result);
           setLoading(false);
       })
    },[])

    return (
        <>
            {isLoading ?
            <Box style={{margin:"0 auto"}}>
                <CircularProgress/>
            </Box>
                :
            <div>
                {comments.length === 0
                    ?
                    <div>there is no comments</div>
                    : comments.map((comment) => <CommentCard key={comment.id} comment={comment} width={"100%"}/>)
                }
            </div>}
        </>
    )
};

export {CommentReel};