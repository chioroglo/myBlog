import React from 'react';
import {Box, CircularProgress} from '@mui/material';

const CenteredLoader = () => {
    return (
        <Box style={{margin: "50px auto", width: "fit-content"}}>
            <CircularProgress/>
        </Box>
    );
};

export {CenteredLoader};