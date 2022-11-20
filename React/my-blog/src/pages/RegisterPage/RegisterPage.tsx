import React from 'react';
import {RegistrationForm} from '../../components/RegistrationForm';

const RegisterPage = () => {
    return (
        <div style={{padding: "15vh 0 0 0", margin: "0 auto", display: "flex", justifyContent: "space-around"}}>
            <RegistrationForm/>
        </div>
    );
};

export {RegisterPage};