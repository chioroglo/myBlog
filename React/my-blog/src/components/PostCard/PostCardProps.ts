import {PostModel} from "../../shared/api/types/post";

export interface PostCardProps {
    post: PostModel,
    width: string,
    commentPortionSize: number
}