import { Segment, List, Label, Item, Image } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import { observer } from 'mobx-react-lite'
import { Coin } from '../../../../app/models/activity';
import { useStore } from '../../../../app/stores/store';

interface Props {
    activity: Coin;
}

export default observer(function ActivityDetailedSidebar ({activity :{followers}}: Props) {
    const {userStore} = useStore();
    if(!followers) return null;
    return (
        <>
            <Segment
                textAlign='center'
                style={{ border: 'none' }}
                attached='top'
                secondary
                inverted
                color='teal'
            >
                {followers.length === 0 ? (
                    <span> This coin has no followers. You can be the first one!</span>
                ) : (
                    <span> This coin followed by {followers.length} {followers.length === 1 ? 'user' : 'users'}.</span>
                )}
            </Segment>
            <Segment attached>
                <List relaxed divided>
                    {followers.map(attendee => (
                        <Item style={{ position: 'relative' }} key={attendee.username}>
                            {attendee.isAdmin &&
                        <Label
                            style={{ position: 'absolute' }}
                            color='orange'
                            ribbon='right'
                        >
                            Admin
                        </Label>}
                        <Image size='tiny' src={attendee.image ||'/assets/user.png'} />
                        <Item.Content verticalAlign='middle'>
                            <Item.Header as='h3'>
                                <Link to={`/profiles/${attendee.username}`}>{attendee.displayName}</Link>
                            </Item.Header>
                            {attendee.following && 
                            <Item.Extra style={{ color: 'orange' }}>Following</Item.Extra>
                        }
                        </Item.Content>
                    </Item>
                    ))}
                </List>
            </Segment>
        </>

    )
})