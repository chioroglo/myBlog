export interface PasskeyWebauthnResult {
    credential: PublicKeyCredential,
    challenge: string
}