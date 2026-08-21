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
        <div className="page-shell">
            <Typography variant="h3" sx={{textAlign: "center", fontSize: {xs: "1.8rem", sm: "2.5rem"}, mb: 3}}>
                Welcome to topic <span style={{fontStyle: "italic"}}>{topicName}</span>
            </Typography>
            {
                topicName ?
                    <BlogReel pagingRequestDefault={topicPagePagingConditions} reelWidth={"min(100%, 760px)"}
                              pageSize={DefaultPageSize}
                              showAddPostForm={false} availableFilterNames={availableFilterNames} showFilteringMenu/>
                    :
                    <Navigate to={"/"}/>
            }
        </div>

    );
};

export {TopicPage};
