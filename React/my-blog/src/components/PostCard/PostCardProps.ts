import {PostModel} from "../../shared/api/types/post";

export interface PostCardProps {
    initialPost: PostModel,
    width: string,
    commentPortionSize: number
}