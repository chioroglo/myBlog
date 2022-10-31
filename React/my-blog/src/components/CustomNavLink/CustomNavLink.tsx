import React from 'react';
import {NavLink, useMatch} from 'react-router-dom';
import {palette} from '../../shared/assets';
import {CustomNavLinkProps} from './CustomNavLinkProps';


const CustomNavLink = ({
                           to,
                           children,
                           color = palette.WHITE,
                           highlightColor = palette.GRAY,
                           ...props
                       }: CustomNavLinkProps) => {

    const match = useMatch(to);

    return (
        <NavLink end style={{color: match ? highlightColor : color, cursor: "pointer"}} to={to} {...props}>
            {children}
        </NavLink>
    )
};

export {CustomNavLink};