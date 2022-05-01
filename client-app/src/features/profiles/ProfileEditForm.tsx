import { Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Button, Segment } from "semantic-ui-react";
import MyTextArea from "../../app/common/form/MyTextArea";
import MyTextInput from "../../app/common/form/MyTextInput";
import { useStore } from "../../app/stores/store";
import * as Yup from 'yup';
import { Profile } from "../../app/models/profile";
import { useHistory } from "react-router";

interface Props {
    setEditMode: (editMode: boolean) => void;
}
export default observer(function ProfileEditForm({ setEditMode }: Props) {
    const { profileStore: { profile, editProfile } } = useStore();
    const history = useHistory();
    const validationSchema = Yup.object({
        displayName: Yup.string().required('Profile displayName is required'),
    });

    function handleFormSubmit(profile: Partial<Profile>){
        editProfile(profile as Profile).then(() => history.push(`/profiles/${profile.username}`));
        setEditMode(false);
    }
    return (
        <Segment clearing>
            <Formik
                validationSchema={validationSchema}
                enableReinitialize
                initialValues={profile!}
                onSubmit={values => handleFormSubmit(values)}>
                {({ handleSubmit, isValid, isSubmitting, dirty }) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='displayName' placeholder="DisplayName" />
                        <MyTextArea rows={3} placeholder='Bio' name='bio' />
                        <Button
                            disabled={isSubmitting || !isValid || !dirty}
                            loading={isSubmitting} floated="right"
                            positive type='submit' content='Submit' />
                    </Form>
                )}
            </Formik>
        </Segment>
    )
})