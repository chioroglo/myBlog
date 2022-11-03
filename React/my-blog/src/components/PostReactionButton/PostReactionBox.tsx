import React, {useState} from 'react';
import {Box, IconButtonProps} from "@mui/material";
import {useSelector} from "react-redux";
import {ApplicationState} from "../../redux";
import {useAuthorizedUserInfo} from "../../hooks";
import {PostReactionModel} from "../../shared/api/types/postReaction/PostReactionModel";

const PostReactionButton = (props: IconButtonProps) => {

    const isAuthorized = useSelector<ApplicationState>(state => state.isAuthorized);
    const user = useAuthorizedUserInfo();
    const [reactions, setReactions] = useState<PostReactionModel[]>([]);

    return (
        <Box>

        </Box>
    );
};

export {PostReactionButton};