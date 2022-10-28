import { RequestFilters } from "../RequestFilters";

export interface CursorPagedRequest {
    "pageSize": number,
    "pivotElementId"?: number,
    "getNewer": boolean,
    "requestFilters"?: RequestFilters
}