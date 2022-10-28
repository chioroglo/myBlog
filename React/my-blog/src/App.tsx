import React from 'react';
import { Navigate, Route, Routes } from 'react-router-dom';
import OnlyForUnauthorized from './guards/OnlyForUnauthorized';
import { RequireAuth } from './guards/RequireAuth';
import { useAuthorizedUserInfo } from './hooks';
import { HomePage, Layout, LoginPage, NotFoundPage, PostPage, ProfilePage, RegisterPage, UserPage } from './pages';
import "./App.css";



function App() {

    const user = useAuthorizedUserInfo()
    const userId = user ? user.id : 0;

  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Layout/>}>
          <Route path="me" element={<RequireAuth children={<Navigate to={`/user/${userId}`} ></Navigate>}/>}/>
          <Route path="login" element={<OnlyForUnauthorized children={<LoginPage/>}/>}/>
          <Route path="register" element={<OnlyForUnauthorized children={<RegisterPage/>}/>}/>
          <Route path="/" element={<RequireAuth children={<HomePage/>}/>}/>
          <Route path="post/:postId" element={<PostPage/>}/>
          <Route path="user/:userId" element={<ProfilePage/>}/>
          <Route path="*" element={<NotFoundPage/>}/>
        </Route>
      </Routes>
    </div>
  );
}

export default App;
