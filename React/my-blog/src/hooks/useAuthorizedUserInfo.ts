import { useSelector } from "react-redux";
import { ApplicationState } from "../redux";
import { JwtTokenKeyName, UserIdTokenKeyName, UsernameTokenKeyName } from "../shared/config";
import { UserInfoCache } from "../shared/types";

export const useAuthorizedUserInfo = (): UserInfoCache | null => { 
    const isAuthorized: boolean = useSelector<ApplicationState,boolean>(state => state.isAuthorized);

    if (isAuthorized) {
        return {
            id: parseInt(localStorage.getItem(UserIdTokenKeyName) || sessionStorage.getItem(UserIdTokenKeyName) || "0"),
            username: localStorage.getItem(UsernameTokenKeyName) || sessionStorage.getItem(UsernameTokenKeyName) || "",
            token: localStorage.getItem(JwtTokenKeyName) || sessionStorage.getItem(JwtTokenKeyName) || ""
        }
    } else {
        return null;
    }
}