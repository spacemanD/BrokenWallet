export interface Subscription{
    id: number,
    name: string,
    price: number,
    description: string,
    duration: Date,
    isDefault: boolean
}

export class Subscription implements Subscription {
    constructor(sub: Subscription) {
        this.id = sub.id;
        this.name = sub.name;
        this.price = sub.price;
        this.description = sub.description;
        this.duration = sub.duration,
        this.isDefault = sub.isDefault
    }
}