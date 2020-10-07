import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { initializeError } from './ErrorSlice';
import ModalDialog from '../../components/ModalDialog';

const ErrorDialog = () => {
  const error = useSelector(state => state.error);
  const dispatch = useDispatch();
  const handleClick = () => dispatch(initializeError());
  if (!error.errorMessages) {
    return null;
  }

  const messages = error.errorMessages
    .map((message, index) => (<p key={index}>{message}</p>));
  return (
    <ModalDialog handleClick={handleClick}>
      <div className="error-messages">
        {messages}
      </div>
    </ModalDialog>
  );
}

export default ErrorDialog;