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
    const result = await apiClient.PostTaskGroup({name: "New Group"});
    setTaskGroups(oldarr => [...oldarr, result]);
    history.push(`/taskgroup/${result.id}`)
  }, [history])

  return (
    <>
      <Typography>Task Groups</Typography>
      <Button onClick={addGroup}>Add</Button>
      {taskGroups && taskGroups.map(tg =>
        <a key={tg.id} href={`taskgroup/${tg.id}`}>
          <div >
            {tg.name} ({tg.taskCount})
        </div>
        </a>
      )}
    </>
  )
}


export default withStyles(styles, { withTheme: true })(TaskGroupList);
