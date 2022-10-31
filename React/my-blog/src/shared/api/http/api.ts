import axios, { AxiosError } from "axios";

import { API_URL,JwtTokenKeyName, UserIdTokenKeyName, UsernameTokenKeyName } from "../../config"
import { AuthenticateRequest, AuthenticateResponse, RegistrationDto } from "../types"
import { CursorPagedRequest, CursorPagedResult } from "../types/paging/cursorPaging";


const instance = axios.create({
    withCredentials: false,
    baseURL: API_URL
});

instance.interceptors.request.use((config) => {
    
        if (config && config.headers)
            config.headers["Authorization"] = localStorage.getItem(JwtTokenKeyName) || sessionStorage.getItem(JwtTokenKeyName)
            ? `Bearer ${localStorage.getItem(JwtTokenKeyName) ?? sessionStorage.getItem(JwtTokenKeyName)}`
            : undefined

    return config;
})

export class axiosDebugApi {
    static getInstance() {
        return instance;
    }
}

export class userApi {
    static getAvatarUrlById(userId: number) {
        return instance.get(`/avatars/${userId}`);
    }

    static getUserById(userId: number) {
        return instance.get(`/users/${userId}`)
    }
}

export class authApi {
    
    static async getCurrent() {
        return await instance.get(`/users/current`);
    }

    static isAuthorized(): boolean {
        return localStorage.getItem(JwtTokenKeyName) !== null || sessionStorage.getItem(JwtTokenKeyName) !== null
    }

    static logout(): void {
        localStorage.removeItem(JwtTokenKeyName);
        sessionStorage.removeItem(JwtTokenKeyName);
        
        localStorage.removeItem(UserIdTokenKeyName);
        sessionStorage.removeItem(UserIdTokenKeyName);

        localStorage.removeItem(UsernameTokenKeyName);
        sessionStorage.removeItem(UsernameTokenKeyName);
    }

    static TryAuthenticateAndPayloadInHeaders(credentials: AuthenticateRequest,useLocalStorage: boolean) {
        return instance.post(`/login`,credentials).then((response) =>
        {
            const payload = response.data as AuthenticateResponse;

            this.setJwtAndPayloadInStorage(payload,useLocalStorage);

            return response;
        })
        .catch((reason) => reason as AxiosError);
    }

    static TryRegister(dto: RegistrationDto) {
        
        if (dto.firstName === "")
        {
            dto.firstName = null;
        }
        
        if (dto.lastName === "")
        {
            dto.lastName = null;
        }

        return instance.post(`/register`,dto)
        .then((response) => {
            return response;
        })
        .catch((reason) => reason as AxiosError);
    }

    private static setJwtAndPayloadInStorage (payload: AuthenticateResponse, useLocalStorage: boolean): void {

        let storage: Storage = useLocalStorage ? localStorage : sessionStorage;
    
        storage.setItem(JwtTokenKeyName,payload.token);
        storage.setItem(UserIdTokenKeyName,payload.id.toString());
        storage.setItem(UsernameTokenKeyName,payload.username);

    }
}

export class postApi {
    static getCursorPagedPosts(request: CursorPagedRequest) {
        return instance.post(`/posts/paginated-search-cursor`, request)
    }

    static getReactionsByPost(postId: number) {
        return instance.get(`/reactions/${postId}`);
    }
}