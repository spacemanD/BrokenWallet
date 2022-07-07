import { observer } from "mobx-react-lite";
import { NavLink } from "react-router-dom";
import { Button, Grid, Header, Popup } from "semantic-ui-react";
import { Subscription } from "../models/subscription";
import { useStore } from "../stores/store";

export default observer(function PopupView(){
    const {subscriptionStore: {subscriptions, setSubscription, selectedSubscriptions}} = useStore();

 return (   
 <Popup flowing hoverable 
 trigger={<Button positive content='Buy Vip'/>} 
 >
    <Grid centered divided columns={1}  inverted>
        {
            subscriptions.map((sub: Subscription) => (
                <Grid.Column textAlign='center'>
                <Header as='h4'>{sub.name}</Header>
                <p>
                <b>{sub.description}</b>  
                <br/>{sub.price} a month
                </p>
                <p>
                <b></b> Duration: {sub.duration}
                </p>
                <Button 
                onClick={() => setSubscription(sub)}
                as = {NavLink} to = '/coins'
                positive content = 'Buy'
                    disabled = {sub.id == selectedSubscriptions?.id}/>
                <br/>
                <b>-------------------------------------------------</b>
            </Grid.Column>
            ))
        }
    </Grid>
</Popup>
)})