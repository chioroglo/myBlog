import {CommentModel} from "../../shared/api/types/comment";
import {PostModel} from "../../shared/api/types/post";

export interface CommentCardProps {
    initialComment: CommentModel,
    post?: PostModel,
    width: string,
    disappearCommentCallback: () => void
}