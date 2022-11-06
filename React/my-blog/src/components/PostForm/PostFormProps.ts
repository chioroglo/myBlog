import {PostDto, PostModel} from "../../shared/api/types/post";
import {AxiosResponse} from "axios";

export interface PostFormProps {
    formActionCallback: (post: PostDto) => Promise<AxiosResponse<PostModel>>,
    formCloseHandler: () => void,
    initialPost?: PostDto,
    width?: string,
    caption?: string
}