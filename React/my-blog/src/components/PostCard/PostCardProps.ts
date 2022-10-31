import React from "react";
import {PostModel} from "../../shared/api/types/post";

export interface PostCardProps {
    post: PostModel,
    width: string,
    ref?: React.RefObject<HTMLElement>
}