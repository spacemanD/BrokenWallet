import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Container, Header, Segment, Image, Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

export default observer(function HomePage() {
    const {userStore, modalStore} = useStore();
    return (
        <Segment inverted textAlign='center' vertical className="masthead">
            <Container text>
                <Header as='h1' inverted>
                    <Image size='massive' src='/assets/wallet.png' alt='logo' style={{marginBottom: 12}} />
                    Broken wallet
                </Header>
                {userStore.isLoggedIn ? (
                    <>
                        <Button as={Link} to='/coins' size='huge' inverted>
                            Go to Broken wallet!
                        </Button>
                    </>
                ) : (
                    <>
                            <Button onClick={() => modalStore.openModal(<LoginForm/>)} size='huge' inverted>
                                Login!
                        </Button>
                            <Button onClick={() => modalStore.openModal(<RegisterForm />)} size='huge' inverted>
                                Register!
                        </Button>
                    </>
                )}

            </Container>
        </Segment>
    )
})