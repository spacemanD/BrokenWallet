import { observer } from "mobx-react-lite";
import React from "react";
import { Link, NavLink } from "react-router-dom";
import { Button, Container, Dropdown, Image, Menu, MenuItem } from "semantic-ui-react";
import { useStore } from "../stores/store";

export default observer(function NavBar(){
    const {userStore: {user, logout}} = useStore();
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item as={NavLink} to='/' exact header>
                 <img src="/assets/wallet.png" alt="logo" style={{marginRight:'10px'}}/>
                  Broken Wallet
                </Menu.Item>
                <Menu.Item as={NavLink} to='/coins'name='Crypto'/>
                <Menu.Item as={NavLink} to='/coinsList'name='Crypto List'/>
                <Menu.Item>
                    <Button as={NavLink} to='/createCoin' positive content='Create crypto'/>
                </Menu.Item>
                <Menu.Item>
                <Button as={NavLink} to='/buyVip' positive content='Buy Vip'/>
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