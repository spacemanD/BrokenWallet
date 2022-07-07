export interface User {
    username: string;
    displayName: string;
    token: string;
    image?:string;
    email?: string;
    isAdmin?:boolean;
    IsBanned?: boolean;
}
export interface UserListItem {
    displayName: string,
    bio: string,
    isAdmin: boolean,
    isBanned: boolean,
    subscription: null,
    photos: null,
    coinFollowings: null,
    followings: null,
    followers: null,
    id: string,
    userName: string,
    email: string,
}
export interface UserFormValues {
    email: string;
    password?: string;
    displayName?: string;
    username?:string;
    isAdmin?:boolean;
    IsBanned?: boolean;
}