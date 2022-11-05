import {PostDto} from "../../shared/api/types/post";

export interface PostFormProps {
    callback: (post: PostDto) => void,
    width?: string,
    caption?: string
}