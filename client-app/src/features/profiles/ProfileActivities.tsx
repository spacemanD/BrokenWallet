import React, { SyntheticEvent, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Tab, Grid, Header, Card, Image, TabProps } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { UserCoin } from '../../app/models/profile';
import { useStore } from "../../app/stores/store";
const panes = [
    { menuItem: 'Popular', pane: { key: 'popular' } },
    { menuItem: 'Trend', pane: { key: 'trend' } },
    { menuItem: 'All', pane: { key: 'all' } }
];
export default observer(function ProfileActivities() {
    const { profileStore } = useStore();

    const {loadUserActivities,profile,loadingActivities,userActivities} = profileStore;
    useEffect(() => {
        loadUserActivities(profile!.username);
    }, [loadUserActivities, profile]);

    const handleTabChange = (e: SyntheticEvent, data: TabProps) => {
        loadUserActivities(profile!.username, panes[data.activeIndex as
            number].pane.key);
    };
    
    return (
        <Tab.Pane loading={loadingActivities}>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated='left' icon='calendar'
                        content={'Activities'} />
                </Grid.Column>
                <Grid.Column width={16}>
                    <Tab
                        panes={panes}
                        menu={{ secondary: true, pointing: true }}
                        onTabChange={(e, data) => handleTabChange(e, data)}
                    />
                    <br />
                    <Card.Group itemsPerRow={4}>
                        {userActivities.map((activity: UserCoin) => (
                            <Card
                                as={Link}
                                to={`/coins/${activity.id}`}
                                key={activity.id}
                           >
                             <Image
                                    src={`/assets/categoryImages/${activity.identifier}.jpg`}
                                    style={{
                                        minHeight: 100, objectFit:'cover'
                                    }}
                                />
                                <Card.Content>
                                    <Card.Header
                                        textAlign='center'>{activity.code}</Card.Header>
                                    <Card.Meta textAlign='center'>
                                        <div>{activity.displayName}</div>
                                    </Card.Meta>
                                </Card.Content>
                            </Card>
                        ))}
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    );
})