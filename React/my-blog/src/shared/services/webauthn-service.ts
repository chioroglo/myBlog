import { PasskeyAuthenticationOptions } from "../api/types/authentication/passkey/passkey-authentication-options";
import { PasskeyRegistrationOptions } from "../api/types/authentication/passkey/passkey-registration-options";
import { PasskeyWebauthnResult } from "../api/types/authentication/passkey/passkey-webauthn-response";
import { stringToByteArray } from "../assets/array-buffer-utils";

export class WebauthnService {

    constructor(
        private readonly navigator: Navigator,
        private readonly window: Window
    ) {

    }
    private isConditionalMediationAvailable = (): Promise<boolean> =>
        PublicKeyCredential['isConditionalMediationAvailable']?.()
            .then((isAvailable: boolean) => isAvailable)
        ?? new Promise<boolean>(() => false)

    private isWebauthnAvailable = (): boolean =>
        ('PublicKeyCredential' in this.window) &&
        ('isConditionalMediationAvailable' in PublicKeyCredential);

    public generateCredential = (options: PasskeyRegistrationOptions): Promise<PublicKeyCredential> => {
    
        if (!this.isWebauthnAvailable()) {
            throw new Error("WebAuthn not available");
        }

        const challenge = stringToByteArray(options.challenge);

        const excludeCredentials = options.excludeCredentials
            .map(c => ({ id: Uint8Array.from(atob(c.id), c => c.charCodeAt(0)), type: c.type } as PublicKeyCredentialDescriptor));


        const credentialCreationOptions: PublicKeyCredentialCreationOptions = {
            challenge,
            rp: {
                id: options.relyingParty.id,
                name: options.relyingParty.name,
                icon: options.relyingParty.icon
            },
            user: {
                name: options.user.name,
                displayName: options.user.displayName,
                id: Uint8Array.from(btoa(options.user.id), c => c.charCodeAt(0))
            },
            pubKeyCredParams: [
                {
                    type: "public-key",
                    alg: -7 // ES256
                },
                {
                    type: "public-key",
                    alg: -257 // RS256
                }
            ],
            authenticatorSelection: {
                userVerification: "preferred",
                requireResidentKey: true,
            },
            extensions: {
                credProps: true
            },
            attestation: "direct",
            excludeCredentials: excludeCredentials
        }

        return this.navigator.credentials.create({ publicKey: credentialCreationOptions })
            .then(obj => obj as PublicKeyCredential);
    }

    public authenticateCredentialRequest = (options: PasskeyAuthenticationOptions, ac: AbortController): Promise<PasskeyWebauthnResult | null> => {
        const challengeBytes = stringToByteArray(options.challenge);
        const requestOptions: PublicKeyCredentialRequestOptions = {
            challenge: challengeBytes,
            allowCredentials: [], // Empty array tells API to allow any of the passkeys available within device
            rpId: options.relyingParty.id,
            userVerification: "required"
        };

        return this.isConditionalMediationAvailable()
            .then((isAvailable: boolean) => {
                if (!isAvailable) {
                    return null;
                }

                return this.navigator.credentials.get({
                    publicKey: requestOptions,
                    mediation: "conditional",
                    signal: ac.signal
                }).then(response => {
                    return {
                        credential: response as PublicKeyCredential,
                        challenge: options.challenge
                    } as PasskeyWebauthnResult
                });
            })
    } 
}