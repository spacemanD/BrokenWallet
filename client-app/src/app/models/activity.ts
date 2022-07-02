import { Profile } from "./profile";

export interface Coin {
    id: string;
    identifier: string;
    displayName: string;
    code: string;
    isFollowing: boolean;
    followers: Profile [];
}

export class Coin implements Coin {
    constructor(init?: ActivityFormValues) {
        Object.assign(this,init);
    }
}
export class ActivityFormValues {
    id?: string = undefined;
    identifier: string = '';
    displayName: string = '';
    code: string = '';

    constructor(activity?: ActivityFormValues) {
        if(activity) {
            this.id = activity.id;
            this.identifier = activity.identifier;
            this.displayName = activity.displayName;
            this.code = activity.code;
        }
    }

}
