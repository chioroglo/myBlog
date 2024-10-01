import { AxiosInstance } from "axios"
import { instance } from "./api";
import { PasskeyRegistrationOptions } from "../types/authentication/passkey/passkey-registration-options";
import { AuthenticateResponse } from "../types";
import { PasskeyAuthenticationOptions } from "../types/authentication/passkey/passkey-authentication-options";
import { IPasskeyRegistrationRequest } from "../types/authentication/passkey/passkey-registration-request";
import { PasskeyAuthenticationRequest } from "../types/authentication/passkey/passkey-authentication-request";

export class PasskeyApi {

    static create(): PasskeyApi {
        return new PasskeyApi(instance);
    }
    constructor(private readonly instance: AxiosInstance) {};

    public getAuthenticationOptions = (): Promise<PasskeyAuthenticationOptions> => this.instance.get(`/passkey/authentication-options`).then((response) => response.data);
    public authenticate = (request: PasskeyAuthenticationRequest): Promise<AuthenticateResponse> => this.instance.post(`/passkey/authenticate`, request).then((response) => response.data);
    public getRegistrationOptions = (): Promise<PasskeyRegistrationOptions> => this.instance.get<PasskeyRegistrationOptions>(`/passkey/registration-options`).then(response => response.data);
    public register = (options: IPasskeyRegistrationRequest): Promise<null> => this.instance.post(`/passkey/register`, options);
}

export const passkeyApi = new PasskeyApi(instance);