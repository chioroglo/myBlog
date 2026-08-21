import React from 'react';
import {Avatar, Typography} from "@mui/material";
import {FormHeaderProps} from './FormHeaderProps';
import {palette} from '../../shared/assets';


const FormHeader = ({iconColor, caption, icon}: FormHeaderProps) => {
    return (
        <div style={{justifyContent: "flex-start", display: "flex", flexDirection: "column", alignItems: "center"}}>
            <Avatar style={{margin: "0 auto", padding: "10px", backgroundColor: iconColor}} variant="circular">
                {icon}
            </Avatar>

            <Typography variant="h5" sx={{textAlign: "center", color: palette.JET, mt: 1}}>
                {caption}
            </Typography>

        </div>
    );
};

export {FormHeader};
