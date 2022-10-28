import { CursorPagedRequest } from "../../shared/api/types/paging/cursorPaging";

export interface BlogReelProps {
    userId: number,
    pageSize: number,
    reelWidth: string,
    pagingConditions: CursorPagedRequest
}