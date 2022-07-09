import { makeAutoObservable, reaction, runInAction } from "mobx";
import agent from "../api/agent";
import { Coin, ActivityFormValues } from "../models/activity";
import { Pagination, PagingParams } from "../models/pagination";
import { Profile } from "../models/profile";
import { store } from "./store";

export default class ActivityStore{
    activityRegistry = new Map<string, Coin>();
    selectedActivity: Coin | undefined = undefined;
    editMode = false;
    loadingTracking = false;
    loadingDeleting = false;
    loadingInitial = false;
    pagination: Pagination | null = null;
    pagingParams = new PagingParams();
    predicate = new Map().set('predicate', 'all');
    selectedPredicate = 'all';

    constructor() {
        makeAutoObservable(this);

        reaction(
            () => this.predicate.keys(),
            () => {
                this.pagingParams = new PagingParams();
                this.activityRegistry.clear();
                this.loadActivities();
            }
        )
    }

    setPagingParams = (pagingParams : PagingParams) => {
        this.pagingParams = pagingParams;
    }

    setPredicate = (predicate: string, value : string | null) => {
        const resetPredicate = () => {
            this.predicate.forEach((key) => {
                this.predicate.delete(key);
            })
        }
        switch (predicate) {
            case 'all':
                resetPredicate();
                this.predicate.set('predicate','all');
                this.selectedPredicate = 'all';
                break;
            case 'popular':
                resetPredicate();
                this.predicate.set('predicate', 'popular');
                this.selectedPredicate = 'popular';
                break;
            case 'trending':
                resetPredicate();
                this.predicate.set('predicate','trending');
                this.selectedPredicate = 'trending';
                break;
            case 'coinname':
                resetPredicate();
                this.predicate.set('coinname', value);
                this.selectedPredicate = 'coinname';
            break;
        }
    }

    get axiosParams() {
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        this.predicate.forEach((value, key) => {
            if (key === 'startDate') {
                params.append(key, (value as Date).toISOString())
            } else {
                params.append(key, value);
            }
        })
        return params;
    }
    get activitiesByCode() {
        return Array.from(this.activityRegistry.values()).sort((a, b) => a.code.localeCompare(b.code));
    }

    loadActivities = async () => {
        this.loadingInitial = true;
        try{
            const result = await agent.Activities.list(this.axiosParams);
            result.data.forEach(activity => {
                this.setActivity(activity);
            }) 
            this.setPagination(result.pagination);
            this.setLoadingInitial(false);
        }catch(error){
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    setPagination = (pagination: Pagination) => {
        this.pagination = pagination;
    }

    loadActivity = async (id:string) => {
        let activity = this.getActivity(id);
        if(activity){
            this.selectedActivity = activity;
            return activity;
        }else{
            this.loadingTracking = true;
            try{
                activity = await agent.Activities.details(id);
                this.setActivity(activity);
                runInAction(() => {
                    this.selectedActivity = activity;
                    this.loadingTracking = false;
                })
                this.setLoadingInitial(false);
                return activity;
            } catch(error) {
                console.log(error);
                this.setLoadingInitial(false);
            }
        }
    }

    private getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }

    private setActivity = (activity: Coin) => {
        const user = store.userStore.user;
        if(user) {
            activity.isFollowing = activity.followers!.some(
                a => a.username === user?.username
            )
        }
        this.activityRegistry.set(activity.id, activity);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    createActivity = async(activity: ActivityFormValues) => {
        const user = store.userStore.user;
        const attendee = new Profile(user!);      
        try{
            await agent.Activities.create(activity);
            const newActivity = new Coin(activity);
            newActivity.followers = [attendee];
            this.setActivity(newActivity);
            runInAction(() => {
                this.selectedActivity = newActivity;
            })
        }catch (error) {
            console.log(error);
        }
    }

    updateActivity = async(activity: ActivityFormValues) => {
        try{
            await agent.Activities.update(activity);
            runInAction(() => {
                if(activity.id) {
                    let updatedActivity = {...this.getActivity(activity.id), ...activity}
                    this.activityRegistry.set(activity.id, updatedActivity as Coin);
                    this.selectedActivity = updatedActivity as Coin;
                }
            })
        }catch (error) {
            console.log(error);

        }
    }

    deleteActivity = async (id : string) => {
        this.loadingDeleting = true;
        try{
            await agent.Activities.delete(id);
            runInAction(() => {
                this.activityRegistry.delete(id);
                this.loadingDeleting = false;
            })
        }catch(error) {
            console.log(error);
            runInAction(() => {
                this.loadingDeleting = false;
            })
        }
    }

    updateAttendance = async() => {
        this.loadingTracking = true;
        const user = store.userStore.user;
        try{
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction (() => {
                if(this.selectedActivity?.isFollowing) {
                    this.selectedActivity.followers = 
                    this.selectedActivity.followers?.filter(x => x.username !== user?.username);
                    this.selectedActivity.isFollowing = false;
                }else {
                    const attendee = new Profile(user!);
                    this.selectedActivity?.followers?.push(attendee);
                    this.selectedActivity!.isFollowing = true;
                }
                this.activityRegistry.set(this.selectedActivity!.id, this.selectedActivity!);
            })
        }catch (error) {
            console.log(error);
        } finally {
            runInAction(() => {
                this.loadingTracking = false;
            })
        }
    }

    clearSelectedActivity = () => {
        this.selectedActivity = undefined;
    }

    updateAttendeeFollowing = (username: string) => {
        this.activityRegistry.forEach(activity => {
            activity.followers.forEach(attendee => {
                if(attendee.username === username) {
                    attendee.following ? attendee.followersCount-- : attendee.followersCount++;
                    attendee.following = !attendee.following;
                }
            })
        })
    }
}