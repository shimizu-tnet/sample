import React, { useState, useEffect, useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { updateAccessToken } from './LoginSlice';
import { updatePlatformParameters } from './PlatformSystem/PlatformSystemSlice';
import { authenticate } from '../api/legacyDocument';
import { splitByLineFeed } from '../helpers/stringHelper';

const Login = () => {
  const [errorMessage, setErrorMessage] = useState([]);
  const dispatch = useDispatch();
  const history = useHistory();
  const executeLogin = useCallback(async () => {
    setErrorMessage([]);

    try {
      const accessToken = await authenticate('system1', 'legacy1');
      const url = new URL(document.location);
      const params = url.searchParams;
      const platformToken = params.get('token');
      const ankenId = params.get('anken-id');
      dispatch(updateAccessToken({ accessToken }));
      dispatch(updatePlatformParameters({ platformToken, ankenId }));
      window.history.replaceState(null, null, '/');
      history.push({ pathname: '/list', state: { referrer: '/login' } });
    } catch (err) {
      const messages = splitByLineFeed(err.message);
      setErrorMessage(messages);
    }
  }, [dispatch, history]);

  useEffect(() => {
    executeLogin();
  }, [executeLogin]);

  return (
    <form className="login-form">
      <div className="login-errormessage-wrap error-messages">
        {errorMessage.map((message, index) => (<p key={index}>{message}</p>))}
      </div>
    </form>
  );
}

export default Login;
