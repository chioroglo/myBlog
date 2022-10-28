import React from 'react';
import { Box, CircularProgress } from '@mui/material';

const WholePageLoader = () => {
    return (
        <Box sx={{display:"flex",justifyContent:"space-around",padding:"30vh 0"}}>
            <CircularProgress color="primary" />
        </Box>
    );
};

export {WholePageLoader};