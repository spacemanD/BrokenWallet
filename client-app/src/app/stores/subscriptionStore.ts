import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Subscription } from "../models/subscription";

export default class SubscriptionStore {
    subscriptions: Subscription [] = [];
    selectedSubscriptions : Subscription | undefined = undefined
    loading: boolean = false;

    constructor() {
        makeAutoObservable(this);
    }

    getSubscriptions = async () => {
        try{
            const result = await agent.Subscriptions.get();
            runInAction(() => {
                this.subscriptions = result;
                this.selectedSubscriptions = this.subscriptions.find(x => x.isDefault);
            })
            return this.subscriptions;
        } catch(error) {
            console.log(error);
        }
    }

    setSubscription = async (sub : Subscription) => {
        this.loading = true;
        try{
            await agent.Subscriptions.put(sub);
            runInAction(() => {
                this.selectedSubscriptions = sub;
                this.loading = false;
            })
        } catch(error) {
            this.loading = false;
            console.log(error);
        }
    }
}