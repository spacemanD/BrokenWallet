import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { NotificationCustom } from "../models/notificationCustom";

export default class NotificationStore {
    notifications: NotificationCustom [] = [];
    selectedNotification : NotificationCustom | null = null;
    loading: boolean = false;

    constructor() {
        makeAutoObservable(this);
    }

    getNotifications = async () => {
        try{
            const result = await agent.Notifications.get();
            runInAction(() => {
                this.notifications = result;
                this.selectedNotification =  this.notifications[result.length - 1];
            })

            return this.notifications;
        } catch(error) {
            console.log(error);
        }
    }

    setNotification = async () => {
        this.loading = true;
        try{
            var result = await agent.Notifications.put();
            this.setNotificationData(result);
        } catch(error) {
            this.loading = false;
            console.log(error);
        }
    }

    private setNotificationData = (notification: NotificationCustom) => {
        this.selectedNotification = notification;
        console.log(this.selectedNotification);
        this.notifications[0] = notification;
    }
}