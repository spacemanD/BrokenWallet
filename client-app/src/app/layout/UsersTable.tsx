import { useEffect } from 'react';
import { Button, Checkbox, Icon, Table } from 'semantic-ui-react'
import { useStore } from '../stores/store';

export default function UsersTable()
{
const {profileStore: {banProfile, loadingProfile, usersRegistry, getUsers}} = useStore();
const users = Array.from(usersRegistry.values());

useEffect(() => {
  if( usersRegistry.size <= 1)  getUsers();
}, [usersRegistry.size, getUsers])

return (
  <Table celled compact definition>
    <Table.Header fullWidth>
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
              <Table.Row key={user.userName}>
              <Table.Cell collapsing key={user.userName}>
                {
                  !user.isBanned ?               
                  <Button negative 
                  onClick={() => banProfile(user)} 
                  loading={loadingProfile}
                  content='Ban the user'
                  /> : 
                  <Button positive 
                  onClick={() => banProfile(user)} 
                  loading={loadingProfile}
                  content='Recover user'
                  /> 
                }
              </Table.Cell>
              <Table.Cell>{user.displayName}</Table.Cell>
              <Table.Cell>{user.userName}</Table.Cell>
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
)
}
