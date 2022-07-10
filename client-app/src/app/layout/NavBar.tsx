import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Link, NavLink } from "react-router-dom";
import { toast } from "react-toastify";
import { Button, Container, Dropdown, Icon, Image, Menu, MenuItem } from "semantic-ui-react";
import { useStore } from "../stores/store";
import PopupView from "./PopupView";

export default observer(function NavBar(){
    const {userStore: {user, logout}, notificationStore: {notifications, setNotification, getNotifications}} = useStore();
    
    const notify = () => {
        setNotification();
        setTimeout(function() {
            toast(`💰 ${notifications[0].message}`, {
                position: "bottom-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                })
          }, 1000);
    }
    
    useEffect(() => {
        const interval = setInterval(() => {
            notify();
        }, 100000);
        return () => {
            clearInterval(interval);};
      }, []);

    return (
        <Menu inverted fixed='top' >
            <Container>
                <Menu.Item as={NavLink} to='/' exact header>
                 <img src="/assets/wallet.png" alt="logo" style={{marginRight:'10px'}}/>
                  Broken Wallet
                </Menu.Item>
                <Menu.Item as={NavLink} to='/coins'name='Crypto'/>
                <Menu.Item as={NavLink} to='/coinsList'name='Rates'/>
                {user!.isAdmin && (
                    <Menu.Item as={NavLink} to='/userList' name='Users'/>
                    )                   
                }
                {user!.isAdmin && (
                    <Menu.Item>
                    <Button as={NavLink} to='/createCoin' positive content='Create crypto'/>
                    </Menu.Item>
                    )     
                }
                <Menu.Item>
                <PopupView/>
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
                <MenuItem onClick={() => getNotifications()}>
                <Icon name="bell" />
                    <Dropdown pointing='left' lazyLoad scrolling>
                        <Dropdown.Menu>
                            {notifications.length > 0 ? 
                                notifications.map((not) => (
                                    <Dropdown.Item as={Link} to={`/coins/${not?.coinId}`} 
                                    content={not.message} 
                                    icon='bitcoin'/>
                                )) : ( 
                                <Dropdown.Item  
                                content={"You don't have still notifications"} 
                                icon='bitcoin'/>) 
                            }

                        </Dropdown.Menu>
                    </Dropdown>
                </MenuItem>
            </Container>
        </Menu>
    )
})