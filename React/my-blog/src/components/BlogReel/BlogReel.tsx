import {Box, CircularProgress, IconButton, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {ApplicationState} from '../../redux';
import {postApi} from '../../shared/api/http/api';
import {CursorPagedRequest, CursorPagedResult} from '../../shared/api/types/paging/cursorPaging';
import {PostDto, PostModel} from '../../shared/api/types/post';
import {PostCard} from '../PostCard';
import {BlogReelProps} from './BlogReelProps';
import EditIcon from '@mui/icons-material/Edit';
import {Waypoint} from 'react-waypoint';
import {AxiosResponse} from 'axios';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import {FilterLogicalOperator, RequestFilters} from '../../shared/api/types/paging';
import {FilterMenu} from '../FilterMenu';
import {DefaultPageSize} from '../../shared/config';
import {PostForm} from "../PostForm";
import {useAuthorizedUserInfo} from "../../hooks";

const BlogReel = ({
                      pageSize = DefaultPageSize,
                      reelWidth,
                      pagingRequestDefault,
                      showFilteringMenu,
                      availableFilterNames = [],
                      showAddPostForm = false
                  }: BlogReelProps) => {

    /* TODO ADD SYNCHRONIZATION BETWEEN QUERY STRING IN BROWSER AND FILTERS AND PARSING THEM WHILE LOADING PAGE */

    const isAuthorized: boolean = useSelector<ApplicationState, boolean>(state => state.isAuthorized);

    const user = useAuthorizedUserInfo();

    const [isLoading, setLoading] = useState<boolean>(true);
    const [noMorePosts, setNoMorePosts] = useState<boolean>(false);
    const [posts, setPosts] = useState<PostModel[]>([]);
    const [filters, setFilters] = useState<RequestFilters>(pagingRequestDefault.requestFilters || {
        filters: [],
        logicalOperator: FilterLogicalOperator.And
    });
    const [pagingRequestConfiguration, setPagingRequestConfiguration] = useState<{ pivotElementId?: number, pageSize: number, getNewer: boolean }>({...pagingRequestDefault});

    const fetchPosts = (pagingRequest: CursorPagedRequest) => postApi.getCursorPagedPosts({
        ...pagingRequest,
        requestFilters: filters
    }).then((result: AxiosResponse<CursorPagedResult<PostModel>>) => result.data);

    const loadMorePosts = (request: CursorPagedRequest): void => {


        if (noMorePosts) {
            return;
        }

        setLoading(true);
        fetchPosts(request).then((result) => {

            if (result.items.length === 0) {
                setNoMorePosts(true);
            }

            setPosts(posts.concat(result.items));
            setPagingRequestConfiguration({...pagingRequestConfiguration, pivotElementId: result.tailElementId});
            setLoading(false);
        });

    }

    const handleNewPost = async (post: PostDto): Promise<AxiosResponse<PostModel>> => {
        return postApi.addPost(post).then((result: AxiosResponse<PostModel>) => {
            if (result.status === 200 && user) {
                result.data.authorUsername = user?.username;
                result.data.authorId = user?.id;
                setPosts([result.data, ...posts]);
            }

            return result;
        }).catch((result) => result);
    }

    useEffect(() => {
        loadMorePosts({...pagingRequestConfiguration, requestFilters: filters});
    }, []);

    useEffect(() => {
        setNoMorePosts(false);
        setLoading(true);
        setPagingRequestConfiguration({
            pageSize: pageSize,
            getNewer: false,
        });


        fetchPosts({
            ...pagingRequestConfiguration,
            pivotElementId: undefined,
            requestFilters: filters
        }).then((result) => {

            if (result.items.length === 0) {
                setNoMorePosts(true);
            }

            setPosts(result.items);
            setPagingRequestConfiguration({...pagingRequestConfiguration, pivotElementId: result.tailElementId});
            setLoading(false);
        })


    }, [filters]);


    return (
        <>
            {showFilteringMenu && <FilterMenu width={reelWidth} requestFilters={filters} setFilters={setFilters}
                                              availableFilters={availableFilterNames}/>}

            {showAddPostForm && isAuthorized && <PostForm caption={"New post"} callback={handleNewPost} width="50%"/>}

            {isLoading && posts.length === 0 ?
                <Box style={{margin: "50px auto", width: "fit-content"}}>
                    <CircularProgress/>
                </Box>
                :
                posts.length === 0 ?
                    <Box width={reelWidth} style={{margin: "0 auto"}}>
                        <Typography textAlign={"center"} variant="h5">
                            <EditIcon sx={{width: "100px", height: "100px", display: "block", margin: "0 auto"}}/>
                            Unfortunately,this reel is empty
                        </Typography>
                    </Box>
                    :
                    <>
                        {posts.map((post) => <PostCard width={reelWidth} key={post.id} post={post}
                                                       commentPortionSize={pageSize}/>)}
                        <Waypoint bottomOffset="-700px"
                                  onEnter={() => !noMorePosts && loadMorePosts(pagingRequestConfiguration)}></Waypoint>

                        {isLoading &&
                            <Box style={{margin: "50px auto", width: "fit-content"}}>
                                <CircularProgress/>
                            </Box>
                        }

                        {
                            noMorePosts && <Box style={{margin: "50px auto", width: "fit-content"}}>

                                <IconButton children={<ArrowUpwardIcon fontSize={"large"}/>}
                                            style={{margin: "0 auto", display: "block"}}
                                            onClick={() => window.scrollTo({top: 0, behavior: "smooth"})}/>
                                <Typography textAlign={"center"}>
                                    Oops.. Looks like there is nothing for you to show<br/>
                                    Press the arrow button to scroll to the top
                                </Typography>
                            </Box>
                        }
                    </>}
        </>
    );
}
export {BlogReel};