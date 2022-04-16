import { observer } from "mobx-react-lite";
import React, { ChangeEvent, useEffect, useState } from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { Button, Form, Segment } from "semantic-ui-react";
import LoadingComponent from "../../../../app/layout/LoadingComponent";
import { useStore } from "../../../../app/stores/store";
import {v4 as uuid} from "uuid";

export default observer(function ActivityForm(){
    const history = useHistory();
    const {activityStore} = useStore();
    const {createActivity, updateActivity,
         loading, loadActivity, loadingInitial, setLoadingInitial} = activityStore;
    const {id} = useParams<{id: string}>();

    const [activity, setActivity] = useState({
        id: '',
        title: '',
        category: '',
        description: '',
        date: '',
        city: '',
        venue: ''
    });

    useEffect(() => {
        if (id) {
            loadActivity(id).then(activity => setActivity(activity!));
        }
        else {
            setLoadingInitial(false);
        }
    }, [id, loadActivity]);

    function hadleSubmit(){
        if (activity.id.length === 0) {
            let newactivity = {
                ...activity,
                id: uuid()
            };
            createActivity(newactivity).then(() => history.push(`/activities/${newactivity.id}`))
        } else {
            updateActivity(activity).then(() => history.push(`/activities/${activity.id}`))
        }  
    }

    function hadleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>){
        const {name, value} = event.target;
        setActivity({...activity, [name]: value})
    }

    if (loadingInitial) return <LoadingComponent content="Loading activity..."/>;

    return(
    <Segment clearing>
        <Form onSubmit={hadleSubmit} autoComplete='off'>
            <Form.Input placeholder='Title' value={activity.title} name='title' onChange={hadleInputChange}/>
            <Form.TextArea placeholder='Description'value={activity.description} name='description' onChange={hadleInputChange}/>
            <Form.Input placeholder='Category'value={activity.category} name='category' onChange={hadleInputChange}/>
            <Form.Input type='date' placeholder='Date'value={activity.date} name='date' onChange={hadleInputChange}/>
            <Form.Input placeholder='City'value={activity.city} name='city' onChange={hadleInputChange}/>
            <Form.Input placeholder='Venue'value={activity.venue} name='venue' onChange={hadleInputChange}/>
            <Button loading={loading} floated="right" positive type='submit' content='Submit'/>
            <Button as={Link} to='/activities' floated="right" type='button' content='Cancel'/>
        </Form>
    </Segment>
    )
})