import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { Button, Header, Segment } from "semantic-ui-react";
import LoadingComponent from "../../../../app/layout/LoadingComponent";
import { useStore } from "../../../../app/stores/store";
import {v4 as uuid} from "uuid";
import { Formik, Form } from "formik";
import * as Yup from 'yup';
import MyTextInput from "../../../../app/common/form/MyTextInput";
import { ActivityFormValues } from "../../../../app/models/activity";

export default observer(function ActivityForm(){
    const history = useHistory();
    const {activityStore} = useStore();
    const {createActivity, updateActivity,
          loadActivity, loadingInitial} = activityStore;
    const {id} = useParams<{id: string}>();

    const [activity, setActivity] = useState<ActivityFormValues>(new ActivityFormValues());

    const validationSchema = Yup.object({
        identifier: Yup.string().required('Identifier is required'),
        displayName: Yup.string().required(),
        code: Yup.string().required()
    })

    useEffect(() => {
        if (id) {
            loadActivity(id).then(activity => setActivity(new ActivityFormValues(activity)));
        }
    }, [id, loadActivity]);

    function handleFormSubmit(activity: ActivityFormValues){
        if (!activity.id) {
            let newactivity = {
                ...activity,
                id: uuid()
            };
            createActivity(newactivity).then(() => history.push(`/coins/${newactivity.id}`))
        } else {
            updateActivity(activity).then(() => history.push(`/coins/${activity.id}`))
        }  
    }

    if (loadingInitial) return <LoadingComponent content="Loading coins..."/>;

    return(
    <Segment clearing>
        <Header content='Crypto Details' sub color='teal' />
        <Formik
            validationSchema={validationSchema} 
            enableReinitialize 
            initialValues={activity} 
            onSubmit={values => handleFormSubmit(values)}>
            {({handleSubmit, isValid, isSubmitting, dirty}) => (
                <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                <MyTextInput name='identifier' placeholder="Identifier"/>
                <MyTextInput placeholder='DisplayName' name='displayName' />
                <MyTextInput placeholder='Code' name='code' />
                <Button 
                    disabled={isSubmitting || !isValid || !dirty}
                    loading={isSubmitting} floated="right" 
                    positive type='submit' content='Submit'/>
                <Button as={Link} to='/coins' floated="right" type='button' content='Cancel'/>
            </Form>
            )}
        </Formik>
    </Segment>
    )
})