import { IPasskeyCredentialEntity } from "./passkey-credential-entity";
import { PasskeyRelyingParty } from "./passkey-relying-party";
import { IPasskeyUserEntity } from "./passkey-user-entity";

export interface PasskeyRegistrationOptions {
    challenge: string,
    relyingParty: PasskeyRelyingParty,
    user: IPasskeyUserEntity,
    excludeCredentials: IPasskeyCredentialEntity[],
}