import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import {Button, Header, Item, Segment, Image, Label} from 'semantic-ui-react'
import { Coin } from '../../../../app/models/activity';
import { useStore } from '../../../../app/stores/store';

const activityImageStyle = {
    filter: 'brightness(30%)'
};

const activityImageTextStyle = {
    position: 'absolute',
    bottom: '1%',
    left: '1%',
    width: '100%',
    height: 'auto',
    color: 'white'
};

interface Props {
    activity: Coin
}

export default observer (function ActivityDetailedHeader({activity}: Props) {
    const {activityStore: {updateAttendance, loadingTracking: loadingTracking, loadingDeleting: loadingDeleting, deleteActivity},userStore: {user}} = useStore();
    return (
        <Segment.Group>
            <Segment basic attached='top' style={{padding: '0'}}>
                <Image src={`/assets/categoryImages/${activity.identifier}.jpg`} fluid style={activityImageStyle}/>
                <Segment style={activityImageTextStyle} basic>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header
                                    size='huge'
                                    content={`${activity.displayName} (${activity.code})`}
                                    style={{color: 'white'}}
                                />
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
            <Segment clearing attached='bottom'>
                {activity.isFollowing ? (
                    <Button loading={loadingTracking} onClick={updateAttendance}>Cancel subcription</Button>
                ) : (
                    <Button
                        loading={loadingTracking}
                        onClick={updateAttendance}
                        color='teal'
                        floated='left'
                        >
                        Track crypto
                    </Button>
                )}
                {user?.isAdmin && (
                    <>
                        <Button
                            color='red'
                            floated='right'
                            basic
                            content='Delete crypto'
                            onClick={() => deleteActivity(activity.id)}
                            loading={loadingDeleting}
                        />
                        <Button as={Link}
                            to={`/manage/${activity.id}`}
                            color='orange'
                            floated='right'
                        >
                            Update crypto
                        </Button>
                    </>
                )}
            </Segment>
        </Segment.Group>
    )
})