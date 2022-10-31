import {Outlet} from 'react-router-dom';
import {Header} from '../../components/Header';

const Layout = () => {
    return (
        <>
            <div style={{height: "5vh", minHeight: "70px"}}></div>
            <Header/>
            <main>
                <Outlet/>
            </main>
        </>
    );
};

export {Layout};