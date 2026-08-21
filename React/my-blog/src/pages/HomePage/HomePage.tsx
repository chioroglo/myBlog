import React from 'react';
import {useSearchParams} from 'react-router-dom';
import {BlogReel} from '../../components/BlogReel';
import {fetchFiltersFromUrlSearchParams, PostFilterNames} from '../../shared/assets';
import {DefaultPageSize} from '../../shared/config';

const HomePage = () => {

    const [searchParams, setSearchParams] = useSearchParams();

    const availableFilterNames: PostFilterNames[] = [PostFilterNames.Content, PostFilterNames.Title, PostFilterNames.Topic];

    const homePagingConditions = {
        pageSize: DefaultPageSize,
        getNewer: false,
        requestFilters: fetchFiltersFromUrlSearchParams(searchParams, availableFilterNames)
    };


    return (
        <div className="page-shell">
            <BlogReel showAddPostForm={true} reelWidth={'min(100%, 760px)'} pagingRequestDefault={homePagingConditions} showFilteringMenu
                      availableFilterNames={availableFilterNames}/>
        </div>
    );
};

export {HomePage};
