import React, { useState, useEffect } from 'react';
import { createStyles, withStyles, WithStyles, Theme } from '@material-ui/core/styles';
import { Formik, Form, Field } from 'formik';
import { UserTaskViewModel, ApiClient, TaskStatus, UserViewModel } from '../../apiClient/apiClient';
import * as Yup from 'yup';
import dayjs from 'dayjs';
import { Button, MenuItem } from '@material-ui/core';
import { TextField, Select } from 'formik-material-ui';

const styles = (theme: Theme) => createStyles({

});

interface Props extends WithStyles<typeof styles> {
  task: UserTaskViewModel
}


const UserTaskForm: React.FC<Props> = ({ classes, task }: Props) => {
  const [users, setUsers] = useState<Array<UserViewModel>>([])
  useEffect(() => {
    const fetchUsers = async () => {
      const apiClient = new ApiClient();
      const result = await apiClient.GetUsers();
      setUsers(result);
    }
    fetchUsers();
  }, []);

  const taskFormSchema = Yup.object().shape({
    name: Yup.string().required(),
    deadline: Yup.date().required(),
    userId: Yup.number(),
    status: Yup.number().required()
  })

  return (
    <Formik
      initialValues={{
        name: task.name,
        deadline: dayjs(task.deadline).format('YYYY-MM-DD'),
        userId: task.userId || 0,
        //@ts-ignore
        status: TaskStatus[task.status] || 0,
      }}
      onSubmit={async (values) => {
        const apiClient = new ApiClient();
        try {
          await apiClient.PatchUserTask(task.id, { ...values, groupId: task.groupId });
        } catch (error) {
          console.log(error);
        }
      }}
      validationSchema={taskFormSchema}
      render={() =>
        <Form>
          <Field
            name='name'
            component={TextField} />
          <Field
            name='deadline'
            type='date'
            component={TextField} />
          <Field
            name='userId'
            component={Select}>
            {users.map((u, i) =>
              <MenuItem key={i} value={u.id}>{u.fullName}</MenuItem>)}
          </Field>
          <Field
            name='status'
            component={Select}>
            {Object.keys(TaskStatus).filter(k => !isNaN(Number.parseInt(k))).map((ts, i) =>
              <MenuItem key={i} value={Number.parseInt(ts)}>{TaskStatus[Number.parseInt(ts)]}</MenuItem>
            )}
          </Field>
          <Button type='submit'>Save</Button>
        </Form>
      }
    />
  )
}

export default withStyles(styles, { withTheme: true })(UserTaskForm);
