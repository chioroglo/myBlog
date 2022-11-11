import React from 'react';
import {Box, Typography} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";

const EmptyReelPlate = ({width}:{width:string}) => {
    return (
        <Box width={width} style={{margin: "0 auto"}}>
            <Typography textAlign={"center"} variant="h5">
                <EditIcon sx={{width: "100px", height: "100px", display: "block", margin: "0 auto"}}/>
                Unfortunately,this reel is empty
            </Typography>
        </Box>
    );
};

export {EmptyReelPlate};