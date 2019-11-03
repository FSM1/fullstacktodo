import React, { useState, useEffect, useCallback } from 'react';
import { createStyles, withStyles, WithStyles, Theme } from '@material-ui/core/styles';
import { Typography, Button } from '@material-ui/core';
import { ApiClient, TaskGroupViewModel } from '../../apiClient/apiClient';
import { useHistory } from 'react-router-dom';

const styles = (theme: Theme) => createStyles({});

interface Props extends WithStyles<typeof styles> { }

const TaskGroupList: React.FC<Props> = ({ classes }: Props) => {
  // TODO Get all Task Groups here and list by Id

  const [taskGroups, setTaskGroups] = useState<Array<TaskGroupViewModel>>([]);
  const [sortField, setSortField] = useState<'name' | 'taskCount'>('name')

  useEffect(() => {
    const fetchTaskGroups = async () => {
      const apiClient = new ApiClient();
      const result = await apiClient.GetTaskGroups();
      setTaskGroups(result);
    }
    fetchTaskGroups();
  }, []);

  const history = useHistory();

  const addGroup = useCallback(async () => {
    const apiClient = new ApiClient();
    const result = await apiClient.PostTaskGroup({ name: "New Group" });
    setTaskGroups(oldarr => [...oldarr, result]);
    history.push(`/taskgroup/${result.id}`)
  }, [history])

  const deleteGroup = useCallback(async (id: number) => {
    const apiClient = new ApiClient();
    await apiClient.DeleteTaskGroup(id);
    setTaskGroups(previous => previous.filter(tg => tg.id !== id));
  }, [])

  return (
    <>
      <Typography>Task Groups</Typography>
      <Button onClick={addGroup}>Add</Button>
      <Button onClick={() => setSortField('name')}>Sort: Name</Button>
      <Button onClick={() => setSortField('taskCount')}>Sort: Tasks</Button>
      {taskGroups && taskGroups.sort((a, b) => (a[sortField] > b[sortField]) ? 1 : -1).map(tg =>
        <div key={tg.id}>
          <a href={`taskgroup/${tg.id}`}>{tg.name} ({tg.taskCount})</a> 
          <Button onClick={() => deleteGroup(tg.id)}>Delete</Button>
        </div>
      )}
    </>
  )
}


export default withStyles(styles, { withTheme: true })(TaskGroupList);
