import React, { useEffect, useState } from 'react';
import { Container } from 'semantic-ui-react';
import { Activity } from '../models/activity';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import {v4 as uuid} from 'uuid';
import agent from '../api/agent';
import LoadingComponent from './LoadingComponent';

function App() {
const [activities, setActivities] = useState<Activity[]>([]);
const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
const [editMode, setEditMode] = useState(false);
const [loading, setloading] = useState(true);
const [submiting, setSubmiting] = useState(false);


useEffect(() => {
    agent.Activities.list().then(response => {
    let activities: Activity [] = [];
    response.forEach(activity => {
      activity.date = activity.date.split('T')[0]
      activities.push(activity);
    }) 
    setActivities(activities)
    setloading(false);
  })
}, [])

  function handleSelectedActivity(id: string){
  setSelectedActivity(activities.find(x => x.id === id));
  }

  function handleCancelSelectedActivity(){
    setSelectedActivity(undefined);
  };

  function handleFormOpen(id?: string){
    id ? handleSelectedActivity(id) : handleCancelSelectedActivity();
    setEditMode(true);
  };

  function handleFormClose(){
    setEditMode(false);
  };

  function handleDeleteActivity(id: string){
    setSubmiting(true);
    agent.Activities.delete(id).then(() => {
      setActivities([...activities.filter(x => x.id !== id)]);
      setSubmiting(false);
    });
  }

  function handleCreateOrEditActicity(activity: Activity){
    setSubmiting(true);
    if(activity.id){
      agent.Activities.update(activity).then(() => {
       setActivities([...activities.filter(x => x.id !== activity.id),activity])
       setSelectedActivity(activity);
       setEditMode(false);
       setSubmiting(false)
      })
    }
    else{
      activity.id = uuid();
      agent.Activities.create(activity).then(() => {
        setActivities([...activities,activity])
        setSelectedActivity(activity);
        setEditMode(false);
        setSubmiting(false)
      })
    }
  }

  if(loading) return <LoadingComponent content='Loading app'/>
  return (
    <div>
      <>
        <NavBar openForm={handleFormOpen}/>
        <Container style={{marginTop: '7em'}}>
          <ActivityDashboard activities={activities}
                    selectedActivity={selectedActivity}
                    selectActivity={handleSelectedActivity}
                    cancelSelectActivity={handleCancelSelectedActivity}
                    editMode={editMode}
                    openForm={handleFormOpen}
                    closeForm={handleFormClose}
                    createOrEdit={handleCreateOrEditActicity}
                    deleteActivity={handleDeleteActivity}
                    submiting={submiting}
                    />
        </Container>
      </>
    </div>
  );
}

export default App;
