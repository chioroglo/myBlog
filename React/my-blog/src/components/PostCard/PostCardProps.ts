import {PostModel} from "../../shared/api/types/post";
import React from "react";

export interface PostCardProps {
    initialPost: PostModel,
    disappearPostCallback : () => void,
    width: string,
    commentPortionSize: number
}