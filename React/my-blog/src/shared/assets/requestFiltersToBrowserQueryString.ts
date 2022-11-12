import {RequestFilters} from "../api/types/paging";

export const requestFiltersToBrowserQueryString = (filterArray: RequestFilters): string => {
    return `LogicalOperator=${filterArray.logicalOperator}` + filterArray.filters.reduce((accumulator, current, index) => {
        return accumulator + `&${current.path}=${current.value}`
    }, "");
}