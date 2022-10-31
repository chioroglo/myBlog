import {RequestFilters} from "../../shared/api/types/paging";

export interface FilterMenuProps {
    width: string,
    requestFilters: RequestFilters,
    availableFilters: string[]

    setFilters(requestFilters: RequestFilters): void
}