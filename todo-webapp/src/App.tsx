import React from 'react';
import AppWrapper from './components/AppWrapper';
import { Switch, Route } from 'react-router-dom';
import TaskGroupList from './components/TaskGroupList';
import TaskGroupDetails from './components/TaskGroupDetails';

const App: React.FC = () => {
  return (
    <AppWrapper>
      <Switch>
        <Route path='/' exact component={TaskGroupList}/>
        <Route path='/taskgroup/:id' exact component={TaskGroupDetails} />
      </Switch>
    </AppWrapper>
  );
}

export default App;
