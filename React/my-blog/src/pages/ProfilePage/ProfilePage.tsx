import {Box, Tab, Tabs} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {useParams} from 'react-router-dom';
import {BlogReel} from '../../components/BlogReel';
import {CommentReel} from '../../components/CommentReel';
import {CustomTabPanel} from '../../components/CustomTabPanel';
import {ProfileHeader} from '../../components/ProfileHeader';
import {WholePageLoader} from '../../components/WholePageLoader';
import {ApplicationState} from '../../redux';
import {userApi} from '../../shared/api/http/api';
import {FilterLogicalOperator} from '../../shared/api/types/paging';
import {CursorPagedRequest} from '../../shared/api/types/paging/cursorPaging';
import {UserModel} from '../../shared/api/types/user';
import {DefaultPageSize} from '../../shared/config';
import {UserInfoCache} from "../../shared/types";

const ProfilePage = () => {

    const isAuthorized = useSelector<ApplicationState, UserInfoCache | null>(state => state.user);

    const [isLoading, setLoading] = useState<boolean>(false);
    const {userId} = useParams();
    //TODO make user nullable and do not load page while user is undefined,it causes unnecessary requests
    const [user, setUser] = useState<UserModel>({
        id: 0,
        registrationDate: new Date().toUTCString(),
        lastActivity: new Date().toUTCString(),
        username: "",
        fullName: ""
    });

    const [visibleTabIndex, setVisibleTabIndex] = useState<number>(0);

    const [pageRequestPostReel, setPageRequestPostReel] = useState<CursorPagedRequest>();
    const [pageRequestCommentReel, setPageRequestCommentReel] = useState<CursorPagedRequest>();

    const fetchUser = () => userApi.getUserById(parseInt(userId || "0")).then(response => {
        setUser(response.data);
        return response.data
    });

    const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
        event.preventDefault();
        setVisibleTabIndex(newValue)
    };

    const tabProps = (index: number) => {
        return {id: `simple-tab-${index}`, 'aria-controls': `simple-tabpanel-${index}`}
    };

    useEffect(() => {
        setLoading(true);

        fetchUser().then((response) => {

            setPageRequestPostReel({
                requestFilters: {
                    logicalOperator: FilterLogicalOperator.And,
                    filters: [
                        {
                            path: "UserId",
                            value: response.id.toString()
                        }
                    ]
                },
                pageSize: DefaultPageSize,
                getNewer: false
            });

            setPageRequestCommentReel({
                requestFilters: {
                    logicalOperator: FilterLogicalOperator.And,
                    filters: [
                        {
                            path: "UserId",
                            value: response.id.toString()
                        }
                    ]
                },
                pageSize: DefaultPageSize,
                getNewer: false
            });

            setLoading(false);
        });
    }, [isAuthorized]);

    return (
        <>
            {
                isLoading
                    ?
                    <WholePageLoader/>
                    :
                    <>
                        <ProfileHeader setUser={setUser} user={user}/>

                        <Box style={{width: "70vw", margin: "0 auto"}}>
                            <Box>
                                <Tabs value={visibleTabIndex} onChange={handleTabChange}>
                                    <Tab label="Posts" {...tabProps(0)}/>
                                    <Tab label="Comments" {...tabProps(1)}/>
                                </Tabs>
                            </Box>

                            <CustomTabPanel index={0} value={visibleTabIndex}>
                                {pageRequestPostReel && <BlogReel reelWidth="100%" pageSize={DefaultPageSize}
                                                                  pagingRequestDefault={pageRequestPostReel}
                                />}
                            </CustomTabPanel>

                            <CustomTabPanel index={1} value={visibleTabIndex}>
                                {<CommentReel enableInfiniteScroll reelWidth="100%"
                                              pagingRequestDefault={pageRequestCommentReel}></CommentReel>}
                            </CustomTabPanel>
                        </Box>

                    </>
            }
        </>
    );
};

export {ProfilePage};