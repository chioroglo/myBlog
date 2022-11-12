import {CursorPagedRequest} from "../../shared/api/types/paging/cursorPaging";
import {PostFilterNames} from "../../shared/assets";

export interface BlogReelProps {
    pageSize?: number,
    reelWidth: string,
    pagingRequestDefault: CursorPagedRequest,
    showFilteringMenu?: boolean,
    availableFilterNames?: PostFilterNames[],
    showAddPostForm?: boolean
}