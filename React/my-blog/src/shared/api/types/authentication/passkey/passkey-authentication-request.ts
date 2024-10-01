export interface PasskeyAuthenticationRequest {
    credentialId: string,
    authenticatorData: string,
    clientDataJson: string,
    signature: string,
    userHandle: string,
    type: string,
    challenge: string
}