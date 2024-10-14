import { UserWarningModel } from "../../../shared/api/types/warning/user-warning-model";

export interface WarningProfileProps {
    warnings: UserWarningModel[];
    isBanned: boolean;
}