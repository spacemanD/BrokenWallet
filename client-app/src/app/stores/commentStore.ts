import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { makeAutoObservable, runInAction } from "mobx";
import { ChatComment } from "../models/comment";
import { store } from "./store";

export default class CommentStore {
    comments: ChatComment[] = [];
    hubConnection: HubConnection | null = null;

    constructor (){
        makeAutoObservable(this);
    }

    createHubConnection = (coinId : string) => {
        if(store.activityStore.selectedActivity){
           this.hubConnection = new HubConnectionBuilder()
            .withUrl(process.env.REACT_APP_CHAT_URL + '?coinId=' + coinId, {
                accessTokenFactory: () => store.userStore.user?.token!
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();
            
            
            this.hubConnection.start().catch(error => console.log('Error establishing the connection: ', error));

            this.hubConnection.on('LoadComments', (comments: ChatComment[]) => {
                runInAction(() => {
                    comments.forEach(comment => {
                        comment.createdAt = new Date(comment.createdAt);
                    })
                    this.comments = comments
                });
            })
              
            this.hubConnection.on('ReceiveComment', (comment: ChatComment) => {
                runInAction(() => {
                    comment.createdAt = new Date(comment.createdAt);
                    this.comments.unshift(comment);
                });
            })
        }
    }

    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error => console.log('Error stopping: ', error))
    }

    clearComments = () => {
        this.comments = [];
        this.hubConnection?.stop().catch(error => console.log('Error stopping: ', error));
    }

    addComments = async (values: any) => {
        values.coinId = store.activityStore.selectedActivity?.id;
        try {
            await this.hubConnection?.invoke('SendComment', values);
        } catch (error) {
            console.log(error);
        }
    }
}