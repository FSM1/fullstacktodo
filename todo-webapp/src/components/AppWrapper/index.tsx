import React, { Fragment } from 'react';
import { Typography, AppBar, CssBaseline, Toolbar } from '@material-ui/core';
import { createStyles, withStyles, WithStyles, Theme } from '@material-ui/core/styles';

const styles = (theme: Theme) => createStyles({
  root: {
    display: 'flex',
  },
  appBar: {
    zIndex: theme.zIndex.drawer + 1,
  },
  appHeading: {
    flexGrow: 1,
    textAlign: 'center',
    color: 'white',
  },

  content: {
    flexGrow: 1,
    minHeight: '100vh',
    padding: `${theme.spacing(9)}px ${theme.spacing(3)}px ${theme.spacing(3)}px `,
  },
});

interface Props extends WithStyles<typeof styles> {
  children: React.ReactNode,
}

const AppWrapper: React.FunctionComponent<Props> = ({ classes, children }: Props) => (
  <Fragment>
    <div className={classes.root}>
      <CssBaseline />
      <AppBar position="fixed" className={classes.appBar}>
        <Toolbar>
          <Typography variant='h3' className={classes.appHeading}>Todo App</Typography>
        </Toolbar>
      </AppBar>
      <main className={classes.content}>
        {children}
      </main>
    </div>
  </Fragment>
)

export default withStyles(styles, { withTheme: true })(AppWrapper);
