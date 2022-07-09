import { createContext, useContext } from "react";
import CoinStore from "./activityStore";
import CommentStore from "./commentStore";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import NotificationStore from "./notificationStore";
import ProfileStore from "./profileStore";
import SubscriptionStore from "./subscriptionStore";
import UserStore from "./userSrore";

interface Store{
    activityStore: CoinStore;
    commonStore: CommonStore;
    userStore: UserStore;
    modalStore: ModalStore;
    profileStore: ProfileStore;
    commentStore: CommentStore;
    subscriptionStore: SubscriptionStore;
    notificationStore : NotificationStore;
}

export const store: Store ={
    activityStore: new CoinStore(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    profileStore: new ProfileStore(),
    commentStore: new CommentStore(),
    subscriptionStore: new SubscriptionStore(),
    notificationStore : new NotificationStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}