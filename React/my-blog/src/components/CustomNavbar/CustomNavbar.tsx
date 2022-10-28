import { AppBar } from '@mui/material';
import React from 'react';
import { CustomNavbarProps } from './CustomNavbarProps';


const CustomNavbar = ({children}: CustomNavbarProps) => {
    return (
        <AppBar position="absolute">
            {children}
        </AppBar>
    );
};

export {CustomNavbar};