import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Subscription } from "../models/subscription";



export default class SubscriptionStore {
    subscriptions: Subscription [] = [];
    selectedSubscriptions : Subscription | undefined = undefined

    constructor() {
        makeAutoObservable(this);
    }

    getSubscriptions = async () => {
        try{
            this.subscriptions = await agent.Subscriptions.get();
            runInAction(() => {
                this.selectedSubscriptions =  this.subscriptions[0];
            })
            return this.subscriptions;
        } catch(error) {
            console.log(error);
        }
    }
}