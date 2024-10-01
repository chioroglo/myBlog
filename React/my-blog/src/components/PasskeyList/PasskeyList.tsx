import { useEffect, useState } from "react";
import { PasskeyListModel } from "../../shared/api/types/authentication/passkey/passkey-info-model";
import { CenteredLoader } from "../CenteredLoader";
import { VpnKey, VpnKeyOff } from "@mui/icons-material";
import { userApi } from "../../shared/api/http/api";
import "../PasskeyList/PasskeyList.scss";
import moment from "moment";
import { dateTimeFormats } from "../../shared/assets/dateTimeUtils";

const PasskeyList = () => {
    const [isLoading, setLoading] = useState<boolean>(true);
    const [passkeys, setPasskeys] = useState<PasskeyListModel>();

    useEffect(() => {
        userApi.getPasskeysInfoByCurrentUserId().then((model) => {
            setPasskeys(model.data);
            setLoading(false);
        })
    }, []);

    return (
        isLoading ? <CenteredLoader/> : <div>
            {
                passkeys?.passkeys.map(p => <div
                        key={`passkey-info-${p.id}`}
                        id={`passkey-info-${p.id}`}
                        className={`passkey-info__item`}>
                    <VpnKey/>
                    <span className={`passkey-info__caption`}>{p.name + ' ' + moment(p.registrationDate).format(dateTimeFormats.SIMPLE_WITH_TIME)}</span>
                </div>)
            }
        </div>
    )
}

export {PasskeyList};