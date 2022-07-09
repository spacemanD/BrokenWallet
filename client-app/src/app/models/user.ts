export interface User {
    username: string;
    displayName: string;
    token: string;
    image?:string;
    email?: string;
    isAdmin?:boolean;
    isBanned?: boolean;
}
export interface UserListItem {
    id: string,
    displayName: string,
    username: string,
    email: string,
    isAdmin: boolean,
    isBanned: boolean,
}
export interface UserFormValues {
    email: string;
    password?: string;
    displayName?: string;
    username?:string;
    isAdmin?:boolean;
    isBanned?: boolean;
}