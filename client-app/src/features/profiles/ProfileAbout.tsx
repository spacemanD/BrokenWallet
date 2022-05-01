import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Button, Grid, Header, Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import ProfileEditForm from "./ProfileEditForm";


interface Props {
    profile: Partial<Profile> | null;
}

export default observer(function ProfileAbout({profile}: Props) {
    const {profileStore: {isCurrentUser}} = useStore();
    const [editMode, setEditMode] = useState(false);

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated="left" icon='user' content={"About " + profile?.displayName} />
                    {isCurrentUser && (
                            <Button floated="right" 
                            basic 
                            content={editMode ? 'Cancel' : 'Edit Profile'} 
                            onClick={() => setEditMode(!editMode)}
                            />
                        )                      
                    }
                    </Grid.Column>
                    <Grid.Column width={16}>
                    {editMode ? <ProfileEditForm setEditMode={setEditMode} /> :
                        <span style={{whiteSpace: 'pre-wrap'}}>{profile?.bio}</span>}        
                    </Grid.Column>
            </Grid>
        </Tab.Pane>

    )
})