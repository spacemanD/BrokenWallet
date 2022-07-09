import { Link } from 'react-router-dom';
import { Button, Icon, Item, Label, Segment } from 'semantic-ui-react';
import { Coin } from '../../../../app/models/activity';
import ActivityListItemAttendee from '../ActivityListItemAttendee';
import { useStore } from '../../../../app/stores/store';

interface Props {
    activity: Coin
}

export default function ActivityListItem({ activity }: Props) {
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image style={{marginBottom: 3}} size='tiny' circular src={`/assets/categoryImages/${activity.identifier}.jpg`} />
                        <Item.Content>
                            <Item.Header as={Link} to={`/coins/${activity.id}`}>
                                {activity.displayName}
                            </Item.Header>                      
                            {activity?.isFollowing ? (
                                <Item.Description>
                                    <Label basic color='green'>
                                        You are following this coin
                                    </Label>
                                </Item.Description>
                            ) : (
                                <Item.Description>
                                <Label basic color='black'>
                                    {activity.code}
                                </Label>
                            </Item.Description> 
                            )}
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment
                verticalAlign = 'middle'
            >
                <span>
                    <Icon name='users' /> 
                    {activity.followers.length === 0 ? (
                        <span> This coin has no followers. You can be the first one!</span>
                    ) : (
                        <span> This coin followed by {activity.followers.length} {activity.followers.length === 1 ? 'user' : 'users'}.</span>
                    )}
                </span>
            </Segment>
            <Segment secondary>
                <ActivityListItemAttendee attendees={activity.followers!} />
            </Segment>
            <Segment clearing>
                <Button 
                    as={Link}
                    to={`/coins/${activity.id}`}
                    color='teal'
                    floated='right'
                    content='View'
                />
            </Segment>
        </Segment.Group>
    )
}