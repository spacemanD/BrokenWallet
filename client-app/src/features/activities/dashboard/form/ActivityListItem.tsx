import React from 'react';
import { Link } from 'react-router-dom';
import { Button, Icon, Item, Label, Segment } from 'semantic-ui-react';
import {format} from 'date-fns';
import { Coin } from '../../../../app/models/activity';
import ActivityListItemAttendee from '../ActivityListItemAttendee';
import { useStore } from '../../../../app/stores/store';

interface Props {
    activity: Coin
}

export default function ActivityListItem({ activity }: Props) {
    const {userStore : {user}} = useStore();
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image style={{marginBottom: 3}} size='tiny' circular src={`/assets/categoryImages/${activity.identifier}.jpg`} />
                        <Item.Content>
                            <Item.Header as={Link} to={`/coins/${activity.id}`}>
                                {activity.code}
                            </Item.Header>                      
                            {activity?.isFollowing  && (
                                <Item.Description>
                                    <Label basic color='green'>
                                        You are following this crypto
                                    </Label>
                                </Item.Description>
                            )}
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <span>
                    <Icon name='marker' />Followers count: {activity.followers.length}
                </span>
            </Segment>
            <Segment secondary>
                <ActivityListItemAttendee attendees={activity.followers!} />
            </Segment>
            <Segment clearing>
                <span>{activity.code}</span>
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