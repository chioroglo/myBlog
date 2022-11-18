import React from 'react';
import {Navigate, Route, Routes} from 'react-router-dom';
import OnlyForUnauthorized from './guards/OnlyForUnauthorized';
import {RequireAuth} from './guards/RequireAuth';
import {HomePage, Layout, LoginPage, NotFoundPage, PostPage, ProfilePage, RegisterPage} from './pages';
import "./App.css";
import {UserInfoCache} from "./shared/types";
import {useSelector} from 'react-redux';
import {ApplicationState} from './redux';


function App() {

    const user = useSelector<ApplicationState, (UserInfoCache | null)>(state => state.user);
    const userId = user ? user.id : 0;

    return (
        <div className="App">
            <Routes>
                <Route path="/" element={<Layout/>}>
                    <Route path="me" element={<RequireAuth children={<Navigate to={`/user/${userId}`}></Navigate>}/>}/>
                    <Route path="login" element={<OnlyForUnauthorized children={<LoginPage/>}/>}/>
                    <Route path="register" element={<OnlyForUnauthorized children={<RegisterPage/>}/>}/>
                    <Route path="/" element={<HomePage/>}/>
                    <Route path="post/:postId" element={<PostPage/>}/>
                    <Route path="user/:userId" element={<ProfilePage/>}/>
                    <Route path="*" element={<NotFoundPage/>}/>
                </Route>
            </Routes>
        </div>
    );
}

export default App;