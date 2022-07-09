import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { history } from "../..";
import { Coin, ActivityFormValues } from "../models/activity";
import { NotificationCustom } from "../models/notificationCustom";
import { PaginatedResult } from "../models/pagination";
import { Photo, Profile, UserCoin } from "../models/profile";
import { Subscription } from "../models/subscription";
import { User, UserFormValues, UserListItem } from "../models/user";
import { store } from "../stores/store";

const sleep = (delay: number) => {
    return new Promise((resolve) => 
        setTimeout(resolve, delay)
    )}

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if (token) config.headers!.Authorization = `Bearer ${token}`
    return config;
})

axios.interceptors.response.use(async response => {
    if(process.env.NODE_ENV === 'development') await sleep(1000);
        const pagination = response.headers['pagination'];
        if(pagination){
            response.data = new PaginatedResult(response.data, JSON.parse(pagination));
            return response as AxiosResponse<PaginatedResult<any>>
        }
        return response;
}, (error: AxiosError) => {
    const {data, status, config} = error.response!;
    switch (status){
        case 400:
            if (typeof data === 'string') {
                toast.error(data);
            }
            if(config.method === 'get' && data.errors.hasOwnProperty('id')){
                history.push('/not-found');
            }
            if(data.errors){
                const modalStateErrors = [];
                for (const key in data.errors){
                    modalStateErrors.push(data.errors[key])
                }
                throw modalStateErrors.flat();
            }
            break;
        case 401:
            toast.error('unauthoriized')
            break;
        case 404:
            history.push('/not-found');
            break;
        case 500:
            store.commonStore.setServerError(data);
            history.push('/server-error')
            break;        
    }
    return Promise.reject(error);
})

const responseBody = <T> (response: AxiosResponse<T>) => response.data;

const requests = {
    get : <T> (url : string) => axios.get<T>(url).then(responseBody),
    post : <T>  (url : string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put : <T> (url : string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del : <T>  (url : string) => axios.delete<T>(url).then(responseBody),
}

const Activities = {
    list: (params: URLSearchParams) => axios.get<PaginatedResult<Coin[]>>('/coins', {params})
    .then(responseBody),
    details: (id: string) => requests.get<Coin>(`/coins/${id}`),
    create: (activity: ActivityFormValues) => requests.post<void>(`/coins`, activity),
    update: (activity: ActivityFormValues) => requests.put<void>(`/coins/${activity.id}`, activity),
    delete: (id: string) => requests.del<void>(`/coins/${id}`),
    attend: (id: string) => requests.post<void>(`/coins/${id}/track`, {})
}

const Account = {
    current: () => requests.get<User>('/account'),
    login: (user: UserFormValues) => requests.post<User>('/account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/account/register', user),
    reset: (user: UserFormValues) => requests.post<User>('/account/reset', user),
    get: () => requests.get<UserListItem[]>('/profiles/users')
}

const Subscriptions = {
    get: () => requests.get<Subscription[]>('/profiles/subscriptions'),
    put: (sub: Subscription) => requests.put<void>(`/profiles/subscriptions/${sub.id}`, sub),
}

const Notifications = {
    get: () => requests.get<NotificationCustom[]>('/profiles/notifications'),
    put: () => requests.put<NotificationCustom>(`/profiles/notifications/create`, {}),
}

const Profiles = {
    get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
    uploadPhoto: (file: Blob) => {
        let formData = new FormData();
        formData.append('File', file);
        return axios.post<Photo>('photos', formData, {
            headers: {'Content-type': 'multipart/form-data'}
        });
    },
    setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
    deletePhoto : (id: string) => requests.del(`/photos/${id}`),
    edit: (user: Partial<Profile>) => requests.put<Partial<Profile>>(`/profiles/${user.username}`, user),
    ban: (user: UserListItem) => requests.put<UserListItem>(`/profiles/ban`, user),
    updateFollowing: (username: string) => requests.post(`/follow/${username}`, {}),
    listFollowings: (username: string, predicate: string) => 
        requests.get<Profile[]>(`/follow/${username}?predicate=${predicate}`),
    listActivities: (username: string, predicate: string) => 
    requests.get<UserCoin[]>(`/profiles/${username}/coins?predicate=${predicate}`)
}
const agent = {
    Activities,
    Account,
    Profiles,
    Subscriptions,
    Notifications
}

export default agent;