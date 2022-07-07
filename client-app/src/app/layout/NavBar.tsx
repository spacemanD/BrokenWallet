import { observer } from "mobx-react-lite";
import React from "react";
import { Link, NavLink } from "react-router-dom";
import { Button, Container, Dropdown, Grid, Header, Image, Menu, MenuItem, Popup } from "semantic-ui-react";
import { Subscription } from "../models/subscription";
import { useStore } from "../stores/store";
import UsersTable from "./UsersTable";

export default observer(function NavBar(){
    const {userStore: {user, logout}} = useStore();
    const {subscriptionStore: {subscriptions, setSubscription}} = useStore();
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item as={NavLink} to='/' exact header>
                 <img src="/assets/wallet.png" alt="logo" style={{marginRight:'10px'}}/>
                  Broken Wallet
                </Menu.Item>
                <Menu.Item as={NavLink} to='/coins'name='Crypto'/>
                <Menu.Item as={NavLink} to='/coinsList'name='Crypto List'/>
                <Menu.Item as={NavLink} to='/userList' name='Users'/>
                    {user?.isAdmin && (
                        <Menu.Item>
                        <Button as={NavLink} to='/createCoin' positive content='Create crypto'/>
                        </Menu.Item>
                        )
                    }
                <Menu.Item>
                <Popup trigger={<Button positive content='Buy Vip'/>} flowing hoverable>
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
                                    disabled = {sub.isDefault}/>
                                <br/>
                                <b>-------------------------------------------------</b>
                            </Grid.Column>
                            ))
                        }
                    </Grid>
                </Popup>
                
                </Menu.Item>
                <MenuItem position="right">
                    <Image src={user?.image || '/assets/user.png'} avatar spaced='right'/>
                    <Dropdown pointing='top left' text={user?.displayName}>
                        <Dropdown.Menu>
                           <Dropdown.Item as={Link} to={`/profiles/${user?.username}`} 
                            text='My Profile' icon='user'/>
                           <Dropdown.Item onClick={logout} text='Logout' icon='power'/>
                        </Dropdown.Menu>
                    </Dropdown>
                </MenuItem>
            </Container>
        </Menu>
    )
})