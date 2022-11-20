import React from 'react';
import {BlogReel} from '../../components/BlogReel';
import {fetchFiltersFromUrlSearchParams, PostFilterNames} from "../../shared/assets";
import {DefaultPageSize} from "../../shared/config";
import {Navigate, useParams, useSearchParams} from 'react-router-dom';
import {Typography} from '@mui/material';

const TopicPage = () => {

    const {topicName} = useParams();
    const [searchParams, setSearchParams] = useSearchParams();
    const availableFilterNames: PostFilterNames[] = [PostFilterNames.Content, PostFilterNames.Title];

    const filters = fetchFiltersFromUrlSearchParams(searchParams, availableFilterNames);
    filters.filters.push({path: PostFilterNames.Topic, value: topicName || ""});


    const topicPagePagingConditions = {
        pageSize: DefaultPageSize,
        getNewer: false,
        requestFilters: filters
    };

    return (
        <>
            <Typography style={{textAlign: "center", fontSize: "36px"}}>
                Welcome to topic <span style={{fontStyle: "italic"}}>{topicName}</span>
            </Typography>
            {
                topicName ?
                    <BlogReel pagingRequestDefault={topicPagePagingConditions} reelWidth={"50%"}
                              pageSize={DefaultPageSize}
                              showAddPostForm={false} availableFilterNames={availableFilterNames} showFilteringMenu/>
                    :
                    <Navigate to={"/"}/>
            }
        </>

    );
};

export {TopicPage};