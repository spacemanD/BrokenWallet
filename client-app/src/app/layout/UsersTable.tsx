import { observer } from 'mobx-react-lite'
import React from 'react'
import { Button, Checkbox, Icon, Table } from 'semantic-ui-react'
import { useStore } from '../stores/store';

export default function UsersTable()
{
const {profileStore: {users}} = useStore();

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
    {users.map((user) => {
              <Table.Row key={user.username}>
              <Table.Cell collapsing key={user.username}>
                <Checkbox slider />
              </Table.Cell>
              <Table.Cell>{user.displayName}</Table.Cell>
              <Table.Cell>{user.username}</Table.Cell>
              <Table.Cell>{user.email}</Table.Cell>
              <Table.Cell>{user.isAdmin ? 'Yes' : 'No'}</Table.Cell>
              <Table.Cell>{user.IsBanned ? 'Yes' : 'No'}</Table.Cell>
            </Table.Row>
    })}
    </Table.Body>
    <Table.Footer fullWidth>
      <Table.Row>
        <Table.HeaderCell />
        <Table.HeaderCell colSpan='4'>
          <Button
            floated='right'
            icon
            labelPosition='left'
            primary
            size='small'
          >
            <Icon name='user' /> Add User
          </Button>
          <Button size='small'>Approve</Button>
          <Button disabled size='small'>
            Approve All
          </Button>
        </Table.HeaderCell>
      </Table.Row>
    </Table.Footer>
  </Table>
)
}
