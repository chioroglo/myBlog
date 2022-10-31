import React from 'react';
import {Box} from '@mui/material';
import {TabPanelProps} from './TabPanelProps';


const CustomTabPanel = ({index, value, children, ...other}: TabPanelProps) => {
    return (
        <div role="tabpanel" hidden={value !== index} id={`simple-tabpanel-${index}`}
             aria-labelledby={`simple-tab-${index}`} {...other}>
            {value === index && (
                <Box>
                    {children}
                </Box>)
            }
        </div>
    );
};

export {CustomTabPanel};