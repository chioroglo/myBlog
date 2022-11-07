import React from 'react';
import {Link} from "react-router-dom";
import {palette} from '../../shared/assets';
import {UserMentioningInlineProps} from "./UserMentioningInlineProps";

const UserMentioningInline = ({username, userId}: UserMentioningInlineProps) => {
    return (
        <Link style={{color: palette.JET}} to={`/user/${userId}`}>{username}</Link>
    );
};

export {UserMentioningInline};