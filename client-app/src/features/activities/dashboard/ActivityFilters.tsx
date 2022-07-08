import { observer } from "mobx-react-lite";
import React, { Fragment } from "react";
import Calendar from "react-calendar";
import { Header, Menu } from "semantic-ui-react";
import InputExampleIconElement from "../../../app/common/form/InputSearch";
import { useStore } from "../../../app/stores/store";

export default observer(function ActivityFilters() {
    const {activityStore: {setPredicate, selectedPredicate}} = useStore();
    return(
        <>
        <Menu vertical size='large' style={{width: '100%', marginTop: 25}}>
            <Header icon='filter' attached color='teal' content='Filters'/>
            <Menu.Item 
                content= 'All crypto'
                active={selectedPredicate === 'all'}
                onClick={() => setPredicate('all', 'true')}
            />
            <Menu.Item 
                content= "Popular crypto"
                active={selectedPredicate === 'popular'}
                onClick={() => setPredicate('popular', 'true')}    
            />
            <Menu.Item 
                content= "Newly trending"
                active={selectedPredicate === 'trending'}
                onClick={() => setPredicate('trending', 'true')}  
            />
            <Menu.Item >
                <InputExampleIconElement/>
            </Menu.Item>
        </Menu>
        <Header/>
        </>
    )
})