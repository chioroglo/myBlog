import {PostDto, PostModel} from "../../shared/api/types/post";
import {AxiosResponse} from "axios";

export interface PostFormProps {
    callback: (post: PostDto) => Promise<AxiosResponse<PostModel>>,
    width?: string,
    caption?: string
}