import React from 'react';
import {IconButton, IconButtonProps, styled} from "@mui/material";

interface ExpandMoreCardProps extends IconButtonProps {
    expanded: boolean
}

const ExpandMoreCard = styled((props: ExpandMoreCardProps) => {
    const {expanded, ...other} = props;

    return <IconButton {...other}/>
})(
    (
        {theme, expanded}) => ({
        transform: expanded ? "rotate(180deg)" : "rotate(0deg)",
        marginLeft: 'auto',
        transition: theme.transitions.create("transform", {
            duration: theme.transitions.duration.shortest,
        }),
    }));

export {ExpandMoreCard};