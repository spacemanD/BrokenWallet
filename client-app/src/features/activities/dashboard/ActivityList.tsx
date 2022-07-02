import { observer } from "mobx-react-lite";
import { Fragment } from "react";
import { Header } from "semantic-ui-react";
import agent from "../../../app/api/agent";
import { useStore } from "../../../app/stores/store";
import ActivityListItem from "./form/ActivityListItem";

export default observer(function ActivityList(){
        const {activityStore: {activitiesByCode}} = useStore();


    return(
        <>
        {activitiesByCode.map(activity => (
            <Fragment>
                <ActivityListItem key={activity.id} activity={activity}/>
            </Fragment>
        ))}
        </>

    )
})