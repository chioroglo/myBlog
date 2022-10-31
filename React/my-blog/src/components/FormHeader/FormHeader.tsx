import React from 'react';
import {Avatar} from "@mui/material";
import {FormHeaderProps} from './FormHeaderProps';
import {palette} from '../../shared/assets';


const FormHeader = ({iconColor, caption, icon}: FormHeaderProps) => {
    return (
        <div>
            <Avatar style={{margin: "0 auto", padding: "10px", backgroundColor: iconColor}} variant="circular">
                {icon}
            </Avatar>

            <h2 style={{textAlign: "center", color: palette.JET}}>
                {caption}
            </h2>

        </div>
    );
};

export {FormHeader};