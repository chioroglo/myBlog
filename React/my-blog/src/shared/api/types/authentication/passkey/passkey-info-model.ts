export interface PasskeyInfoModel {
    id: number;
    name: string;
    registrationDate: string;
}

export interface PasskeyListModel {
    passkeys: PasskeyInfoModel[]
}