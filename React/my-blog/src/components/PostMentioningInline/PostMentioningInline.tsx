import React from 'react';
import {Link} from "react-router-dom";
import {palette} from '../../shared/assets';
import {PostMentioningInlineProps} from "./PostMentioningInlineProps";

const PostMentioningInline = ({postId, postTitle}: PostMentioningInlineProps) => {
    return (
        <Link style={{color: palette.BAYERN_BLUE, fontStyle: "italic"}} to={`/post/${postId}`}>{`@${postTitle}`}</Link>
    );
};

export {PostMentioningInline};