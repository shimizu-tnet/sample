import React from 'react';
import { HashRouter, Route, Switch } from 'react-router-dom';
import Login from './Login';
import List from './List';
import Input from './Input';

const Router = () => {
  return (
    <HashRouter>
      <Switch>
        <Route exact path='/' component={Login} />
        <Route path='/list' component={List} />
        <Route path='/input' component={Input} />
      </Switch>
    </HashRouter>
  );
}

export default Router;