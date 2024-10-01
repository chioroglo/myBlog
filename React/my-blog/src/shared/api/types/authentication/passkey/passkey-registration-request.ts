export interface IPasskeyRegistrationRequest {
    id: string,
    rawId: string,
    attestationObject: string,
    clientDataJson: string,
    type: string
}