import {Filter, RequestFilters} from "../api/types/paging";

export const fetchFiltersFromUrlSearchParams = (searchParams: URLSearchParams, availableFilterNames: string[]): RequestFilters => {

    let output: Filter[] = [];
    let logicalOperator = parseInt(searchParams.get("LogicalOperator") || "0");

    availableFilterNames.forEach((filterPath) => {
        let filterValues: string[] = searchParams.getAll(filterPath);

        filterValues.forEach((filterValue) => {
            output.push({path: filterPath, value: filterValue});
        })
    })

    return {filters: output, logicalOperator: logicalOperator};
}