import React, { useEffect } from 'react';
import {  Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import { observer } from 'mobx-react-lite/dist';
import { Route, Switch, useLocation } from 'react-router-dom';
import HomePage from '../../features/home/HomePage';
import ActivityForm from '../../features/activities/dashboard/form/ActivityForm';
import ActivityDetails from '../../features/activities/dashboard/details/ActivityDetails';
import TestErrors from '../../features/errors/TestError';
import { ToastContainer } from 'react-toastify';
import NotFound from '../../features/errors/NotFound';
import ServerError from '../../features/errors/ServerError';
import { useStore } from '../stores/store';
import LoadingComponent from './LoadingComponent';
import ModalContainer from '../common/modals/ModalContainer';
import ProfilePage from '../../features/profiles/ProfilePage';
import PrivateRoute from './PrivateRoute';
import MarketProvider from '../../store/MarketProvider';
import Market from '../../containers/Market';
import Coins from '../../containers/Coins';
import UsersTable from './UsersTable';
import 'react-notifications-component/dist/theme.css'

function App() {
  const location = useLocation();
  const {commonStore, userStore} = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore])

  if (!commonStore.appLoaded) return <LoadingComponent content='Loading app...' />

  return (
      <>
      <ToastContainer 
      position="bottom-right"
      autoClose={5000}
      hideProgressBar={false}
      newestOnTop={false}
      closeOnClick
      rtl={false}
      pauseOnFocusLoss
      draggable
      pauseOnHover/>
      <ModalContainer />
        <Route exact path='/' component={HomePage} />
        <Route 
            path={'/(.+)'} 
            render={() => (
              <>
              <NavBar />
              <Container style={{ marginTop: '7em' }}>
                <Switch>
                <PrivateRoute exact path='/coins' component={ActivityDashboard} />
                <PrivateRoute path='/coins/:id' component={ActivityDetails} />
                <PrivateRoute key={location.key} path={['/createCoin', '/manage/:id']} component={ActivityForm} />
                <PrivateRoute path='/profiles/:username' component={ProfilePage}/>
                <PrivateRoute path='/errors' component={TestErrors}/>
                <PrivateRoute path='/userList' component={UsersTable}/>
                <Route path='/server-error' component={ServerError}/>
                <Route exact path="/market">
                  <MarketProvider>
                    <Market />
                  </MarketProvider>
                </Route>
                <Route exact path="/coinsList">
                  <Coins />
                </Route>
                <Route component={NotFound} />
                </Switch>
              </Container>
            </>
            )} 
         />
    </>
  );
}

export default observer(App);
