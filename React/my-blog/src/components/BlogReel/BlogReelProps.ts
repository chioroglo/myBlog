import {CursorPagedRequest} from "../../shared/api/types/paging/cursorPaging";

export interface BlogReelProps {
    pageSize: number,
    reelWidth: string,
    pagingRequestDefault: CursorPagedRequest,
    showFilteringMenu?: boolean,
    availableFilterNames?: string[]
}