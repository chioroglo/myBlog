import {Box, Button, CircularProgress, IconButton, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {ApplicationState} from '../../redux';
import {postApi} from '../../shared/api/http/api';
import {
    CursorPagedRequest,
    CursorPagedResult,
    PagingRequestConfiguration
} from '../../shared/api/types/paging/cursorPaging';
import {PostDto, PostModel} from '../../shared/api/types/post';
import {PostCard} from '../PostCard';
import {BlogReelProps} from './BlogReelProps';
import {Waypoint} from 'react-waypoint';
import {AxiosResponse} from 'axios';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import {FilterLogicalOperator, RequestFilters} from '../../shared/api/types/paging';
import {FilterMenu} from '../FilterMenu';
import {DefaultPageSize} from '../../shared/config';
import {PostForm} from "../PostForm";
import {EmptyReelPlate} from "../EmptyReelPlate";
import {UserInfoCache} from "../../shared/types";

const BlogReel = ({
                      pageSize = DefaultPageSize,
                      reelWidth,
                      pagingRequestDefault = {
                          requestFilters: {
                              logicalOperator: FilterLogicalOperator.Or,
                              filters: []
                          },
                          pageSize: pageSize,
                          getNewer: false
                      },
                      showFilteringMenu,
                      availableFilterNames = [],
                      showAddPostForm = false
                  }: BlogReelProps) => {

    const user = useSelector<ApplicationState, (UserInfoCache | null)>(state => state.user);

    const [formVisible, setFormVisible] = useState<boolean>(showAddPostForm);
    const [isLoading, setLoading] = useState<boolean>(true);
    const [noMorePosts, setNoMorePosts] = useState<boolean>(false);
    const [posts, setPosts] = useState<PostModel[]>([]);

    const [filters, setFilters] = useState<RequestFilters>(pagingRequestDefault.requestFilters);
    const [pagingRequestConfiguration, setPagingRequestConfiguration] = useState<PagingRequestConfiguration>({
        pageSize: pageSize,
        getNewer: pagingRequestDefault.getNewer,
        pivotElementId: pagingRequestDefault.pivotElementId
    });

    const fetchPosts = (pagingRequest: CursorPagedRequest) => postApi.getCursorPagedPosts(pagingRequest).then((result: AxiosResponse<CursorPagedResult<PostModel>>) => result.data);

    const loadMorePosts = (request: CursorPagedRequest, cleanPosts: boolean): void => {

        if (noMorePosts && cleanPosts !== true) {
            return;
        }

        setLoading(true);
        fetchPosts(request).then((result) => {

            if (result.items.length === 0) {
                setNoMorePosts(true);
            }

            if (cleanPosts) {
                setPosts(result.items);
            } else {
                setPosts(posts.concat(result.items));
            }

            setLoading(false);
            setPagingRequestConfiguration({...request, pivotElementId: result.tailElementId});
        });

    }

    useEffect(() => {
        setNoMorePosts(false);

        let request = {
            ...pagingRequestConfiguration,
            pivotElementId: undefined,
            requestFilters: filters
        };

        loadMorePosts(request, true);
    }, [filters]);


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

    const handleDeletePost = (postId: number) => setPosts(posts.filter((value) => value.id !== postId));

    return (
        <>
            {showFilteringMenu && <FilterMenu width={reelWidth} requestFilters={filters} setFilters={setFilters}
                                              availableFilters={availableFilterNames}/>}

            {user && showAddPostForm &&
                (formVisible ?
                    <PostForm formCloseHandler={() => setFormVisible(false)} caption={"New post"}
                              formActionCallback={handleNewPost} width="50%"/>
                    :
                    <Box style={{margin: "20px 0", display: "flex", justifyContent: "space-around"}}>
                        <Button variant={"contained"} onClick={() => setFormVisible(true)}>Add new post</Button>
                    </Box>)
            }

            {isLoading && posts.length === 0 ?
                <Box style={{margin: "50px auto", width: "fit-content"}}>
                    <CircularProgress/>
                </Box>
                :
                posts.length === 0 ?
                    <EmptyReelPlate width={reelWidth}/>
                    :
                    <>
                        {posts.map((post) => <PostCard width={reelWidth} key={post.id} initialPost={post}
                                                       commentPortionSize={pageSize}
                                                       disappearPostCallback={() => handleDeletePost(post.id)}/>)}
                        <Waypoint bottomOffset="-700px"
                                  onEnter={() => !noMorePosts && loadMorePosts({
                                      ...pagingRequestConfiguration,
                                      requestFilters: filters
                                  }, false)}></Waypoint>

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