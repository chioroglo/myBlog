export class UserInfoCache {
    id: number
    username: string
    avatar?: string

    constructor(id: number, username: string, avatar?: string) {
        this.id = id;
        this.avatar = avatar;
        this.username = username;
    }
};