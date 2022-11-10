import {CommentDto, CommentModel} from "../../shared/api/types/comment";
import {AxiosResponse} from "axios";
import {PostModel} from "../../shared/api/types/post";


export interface CommentFormProps {
    formActionCallback: (comment: CommentDto) => Promise<AxiosResponse<CommentModel>>,
    formCloseHandler: () => void,
    post?: PostModel
    width?: string,
    caption?: string,
    initialComment?: CommentDto,
}