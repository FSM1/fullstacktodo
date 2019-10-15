import React, { useEffect, useState, useCallback } from 'react';
import { createStyles, withStyles, WithStyles, Theme } from '@material-ui/core/styles';
import { useHistory, useRouteMatch } from 'react-router-dom';
import { Button, TextField, Typography, Grid } from '@material-ui/core';
import { ApiClient, UserTaskViewModel } from '../../apiClient/apiClient';
import UserTaskForm from '../UserTaskForm';

const styles = (theme: Theme) => createStyles({});

interface Props extends WithStyles<typeof styles> {}

const TaskGroupDetails: React.FC<Props> = ({ classes }: Props) => {
  const [groupName, setGroupName] = useState('');
  const [selectedTask, setSelectedTask] = useState<number | undefined>(undefined);
  const [userTasks, setUserTasks] = useState<Array<UserTaskViewModel>>([]);

  const history = useHistory();
  const match = useRouteMatch<{id: string}>({
    path: '/taskgroup/:id',
    strict: true,
    sensitive: true
  });
  const groupId = (match && match.params) ? Number.parseInt(match.params.id, 10) : 0;

  useEffect(() => {
    const fetchTaskGroup = async () => {
      const apiClient = new ApiClient();
      const result = await apiClient.GetTaskGroup(groupId);
      setGroupName(result.name);
    }
    fetchTaskGroup();
    const fetchUserTasks = async () => {
      const apiClient = new ApiClient();
      const result = await apiClient.GetTaskGroupUserTasks(groupId);
      setUserTasks(result);
    }
    fetchUserTasks();
  }, [groupId]);

  const saveGroupName = useCallback(async () => {
    const apiClient = new ApiClient();
    await apiClient.PatchTaskGroup(groupId, {name: groupName});
  }, [groupId, groupName])

  const addNewTask = useCallback(async () => {
    const apiClient = new ApiClient();
    const result = await apiClient.PostUserTask({groupId: groupId});
    setUserTasks((previous) => [...previous, result]);
    setSelectedTask(result.id);
  }, [groupId])

  return (
  <Grid container>
    <Grid item sm={6}>
    <Button onClick={() => history.goBack()}>Go Back</Button>
    <TextField value={groupName} onChange={(e) => setGroupName(e.target.value)} />
    <Button onClick={saveGroupName}>Save</Button>
    <Typography>Tasks</Typography>
    <Button onClick={addNewTask}>Add new task</Button>
    {userTasks && userTasks.length > 0 ? 
      userTasks.map(ut => 
        <div key={ut.id} onClick={() => setSelectedTask(ut.id)}>
          {ut.name}
          <Button>Remove</Button>
        </div>
      ) :
      <div>No tasks in this group</div>
    }
    </Grid>
    <Grid item sm={6}>
      {selectedTask && <UserTaskForm task={userTasks.filter(ut => ut.id === selectedTask)[0]} />}
    </Grid>
  </Grid>
)}


export default withStyles(styles, { withTheme: true })(TaskGroupDetails);
