import {RequestFilters} from "../api/types/paging";

export const requestFiltersToBrowserQueryString = (filterArray: RequestFilters): string => {
    return `LogicalOperator=${filterArray.logicalOperator}` + filterArray.filters.reduce((accumulator, current) => {
        return accumulator + `&${current.path}=${current.value}`
    }, "");
}