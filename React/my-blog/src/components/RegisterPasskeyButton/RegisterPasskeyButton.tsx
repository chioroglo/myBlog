import { Button } from "@mui/material"
import { PasskeyApi } from "../../shared/api/http/passkey-api";
import { WebauthnService } from "../../shared/services/webauthn-service";
import { UserInfoCache } from "../../shared/types";
import { ApplicationState } from "../../redux";
import { useSelector } from "react-redux";
import { RegisterPasskeyButtonProps } from "./RegisterPasskeyButtonProps";
import { VpnKeySharp } from "@mui/icons-material";
import '../RegisterPasskeyButton/RegisterPasskeyButton.scss';
import { IPasskeyRegistrationRequest } from "../../shared/api/types/authentication/passkey/passkey-registration-request";
import { arrayBufferToBase64 } from "../../shared/assets/array-buffer-utils";
import { useNotifier } from "../../hooks";

const RegisterPasskeyButton = ({ caption }: RegisterPasskeyButtonProps) => {

    const passkeyApi = PasskeyApi.create();
    const webauthnService = new WebauthnService(navigator, window);
    const user = useSelector<ApplicationState, (UserInfoCache | null)>(state => state.user);
    const notifyUser = useNotifier();
    const onClick = () => {
        passkeyApi.getRegistrationOptions().then(result => {

            webauthnService.generateCredential(result).then((credential: any) => {
                const request: IPasskeyRegistrationRequest = {
                    id: credential.id,
                    rawId: arrayBufferToBase64(credential.rawId),
                    clientDataJson: arrayBufferToBase64(credential.response.clientDataJSON),
                    attestationObject: arrayBufferToBase64(credential.response.attestationObject),
                    type: credential.type  
                };
                passkeyApi.register(request)
                .then(() => notifyUser("Passkey created successfully!", "success"))
                .catch((result) => notifyUser(result.response?.data.Message, "error"));
            }).catch((err) => {
                notifyUser("error", "info");
            })
        })
    };

    return (
        <div className="passkey-button-wrapper">
            {
                user ? <Button variant="contained" onClick={onClick}>{
                    <div className="passkey-button-content">
                        <VpnKeySharp />
                        <span>{caption}</span>
                    </div>
                }</Button> : <></>}
        </div>
    )
}

export { RegisterPasskeyButton };