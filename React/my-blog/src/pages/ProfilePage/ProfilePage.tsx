import {Box, Tab, Tabs} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {useParams} from 'react-router-dom';
import {BlogReel} from '../../components/BlogReel';
import {CustomTabPanel} from '../../components/CustomTabPanel';
import {ProfileHeader} from '../../components/ProfileHeader';
import {WholePageLoader} from '../../components/WholePageLoader';
import {ApplicationState} from '../../redux';
import {userApi} from '../../shared/api/http/api';
import {FilterLogicalOperator} from '../../shared/api/types/paging';
import {CursorPagedRequest} from '../../shared/api/types/paging/cursorPaging';
import {UserModel} from '../../shared/api/types/user';
import {DefaultPageSize} from '../../shared/config';

const ProfilePage = () => {

    const isAuthorized: boolean = useSelector<ApplicationState, boolean>(state => state.isAuthorized);

    const [isLoading, setLoading] = useState<boolean>(false);
    const {userId} = useParams();
    const [user, setUser] = useState<UserModel>({
        id: 0,
        registrationDate: new Date(),
        lastActivity: new Date(),
        username: "",
        fullName: ""
    });

    const [visibleTabIndex, setVisibleTabIndex] = useState<number>(0);

    let [pageRequest, setPageRequest] = useState<CursorPagedRequest>();

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

            setPageRequest({
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
    }, []);

    return (
        <>
            {
                isLoading
                    ?
                    <WholePageLoader/>
                    :
                    <>
                        <ProfileHeader user={user}/>

                        <Box style={{width: "70vw", margin: "0 auto"}}>
                            <Box>
                                <Tabs value={visibleTabIndex} onChange={handleTabChange}>
                                    <Tab label="Posts" {...tabProps(0)}/>
                                    <Tab label="Comments" {...tabProps(1)}/>
                                </Tabs>
                            </Box>

                            <CustomTabPanel index={0} value={visibleTabIndex}>
                                {pageRequest && <BlogReel reelWidth="100%" pageSize={DefaultPageSize}
                                                          pagingRequestDefault={pageRequest}
                                />}
                            </CustomTabPanel>

                            <CustomTabPanel index={1} value={visibleTabIndex}>
                                Comments
                            </CustomTabPanel>
                        </Box>

                    </>
            }
        </>
    );
};

export {ProfilePage};