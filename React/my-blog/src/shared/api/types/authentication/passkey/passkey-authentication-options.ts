import { PasskeyRelyingParty } from "./passkey-relying-party";

export interface PasskeyAuthenticationOptions {
    challenge: string,
    relyingParty: PasskeyRelyingParty
}