import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Button, Table } from 'semantic-ui-react'
import { useStore } from '../stores/store';
import LoadingComponent from './LoadingComponent';

export default observer(function UsersTable()
{
const {profileStore: {banProfile, loadingProfile, usersRegistry, getUsers, loadingUsers}} = useStore();
const users = Array.from(usersRegistry.values());

useEffect(() => {
  if( usersRegistry.size <= 1)  getUsers();
}, [usersRegistry.size, getUsers])

if (loadingUsers) return <LoadingComponent content='Loading users...' />

return (
  <Table celled complact definition>
    <Table.Header fulWidth>
      <Table.Row>
        <Table.HeaderCell />
        <Table.HeaderCell>Name</Table.HeaderCell>
        <Table.HeaderCell>UserName</Table.HeaderCell>
        <Table.HeaderCell>E-mail address</Table.HeaderCell>
        <Table.HeaderCell>Admin</Table.HeaderCell>
        <Table.HeaderCell>Banned</Table.HeaderCell>
      </Table.Row>
    </Table.Header>
    <Table.Body>
    {users.map((user) => 
              <Table.Row key={user.username}>
              <Table.Cell collapsing key={user.username}>
                {
                  !user.isBanned ?               
                  <Button negative 
                  onClick={() => banProfile(user)} 
                  disabled={loadingProfile}
                  loading={loadingProfile}
                  content='Ban the user'
                  /> : 
                  <Button positive 
                  onClick={() => banProfile(user)} 
                  disabled={loadingProfile}
                  loading={loadingProfile}
                  content='Recover user'
                  /> 
                }
              </Table.Cell>
              <Table.Cell>{user.displayName}</Table.Cell>
              <Table.Cell>{user.username}</Table.Cell>
              <Table.Cell>{user.email}</Table.Cell>
              <Table.Cell>{user.isAdmin ? 'Yes' : 'No'}</Table.Cell>
              <Table.Cell>{user.isBanned ? 'Yes' : 'No'}</Table.Cell>
            </Table.Row>
    )}
    </Table.Body>
    <Table.Footer fullWidth>
      <Table.Row>
        <Table.HeaderCell />
        <Table.HeaderCell colSpan='4'>
        </Table.HeaderCell>
      </Table.Row>
    </Table.Footer>
  </Table>
)})