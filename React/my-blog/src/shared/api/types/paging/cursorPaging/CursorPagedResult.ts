export interface CursorPagedResult<T> {
    "pageSize": number,
    "total": number,
    "headElementId": number,
    "tailElementId": number,
    "items": T[]
}