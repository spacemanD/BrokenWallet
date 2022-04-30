import { Formik } from "formik";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { Button, Form, Grid, Header, Item, Segment, Tab } from "semantic-ui-react";
import MyTextArea from "../../app/common/form/MyTextArea";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import * as Yup from 'yup';


interface Props {
    profile: Partial<Profile> | null;
}

export default observer(function ProfileEditForm({profile}: Props) {
    const {profileStore: {isCurrentUser,editProfile}} = useStore();
    const history = useHistory();
    const [addEditMode, setEditMode] = useState(false);
    const validationSchema = Yup.object({
        displayName: Yup.string().required('Profile displayName is required'),
    });

    function handleFormSubmit(profile: Partial<Profile>){
        editProfile(profile as Profile).then(() => history.push(`/profiles/${profile.username}`))
    }
    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated="left" icon='user' content={"About " + profile?.displayName} />
                    {isCurrentUser && (
                            <Button floated="right" 
                            basic 
                            content={addEditMode ? 'Cancel' : 'Edit Profile'} 
                            onClick={() => setEditMode(!addEditMode)}
                            />
                        )                      
                    }
                    </Grid.Column>
                    <Grid.Column width={16}>
                    {addEditMode ? (
                            <Segment clearing>
                            <Formik
                                validationSchema={validationSchema} 
                                enableReinitialize 
                                initialValues={profile!} 
                                onSubmit={values => handleFormSubmit(values)}>
                                {({handleSubmit, isValid, isSubmitting, dirty}) => (
                                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                                    <MyTextInput name='displayName' placeholder="DisplayName"/>
                                    <MyTextArea rows={3} placeholder='Bio' name='bio' />
                                    <Button 
                                        disabled={isSubmitting || !isValid || !dirty}
                                        loading={isSubmitting} floated="right" 
                                        positive type='submit' content='Submit'/>
                                </Form>
                                )}
                            </Formik>
                        </Segment>
                        ) : (
                            <Item.Content>
                                {profile?.bio}
                            </Item.Content>
                        )                 
                    }                 
                    </Grid.Column>
            </Grid>
        </Tab.Pane>

    )
})