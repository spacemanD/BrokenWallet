import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Grid} from "semantic-ui-react";
import LoadingComponent from "../../../../app/layout/LoadingComponent";
import { useStore } from "../../../../app/stores/store";
import ActivityDetaliedChat from "./ActivityDetailedChat";
import ActivityDetaliedHeader from "./ActivityDetailedHeader";
import ActivityDetaliedInfo from "./ActivityDetailedInfo";
import ActivityDetaliedSideBar from "./ActivityDetailedSideBar";


export default observer(function ActivityDetails(){
  const {activityStore} = useStore();
  const {selectedActivity: activity, loadActivity, loadingInitial, clearSelectedActivity} = activityStore;
  const {id} = useParams<{id: string}>();

  useEffect(() => {
    if (id) loadActivity(id);
    return () => clearSelectedActivity();
  }, [id, loadActivity, clearSelectedActivity])

    if (!activity || loadingInitial) return <LoadingComponent/>;

    return(
        <Grid>
          <Grid.Column width={10}>
            <ActivityDetaliedHeader activity={activity}/>
            <ActivityDetaliedInfo activity={activity}/>
            <ActivityDetaliedChat activityId={activity.id}/>
          </Grid.Column>
          <Grid.Column width={6}>
            <ActivityDetaliedSideBar activity={activity}/>
          </Grid.Column>
        </Grid>
    )
})