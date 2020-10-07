import React from 'react';

const ModalDialog = props => {
  const { children, handleClick } = props;

  return (
    <>
      <div className="modal-overlay" />
      <div className="modal-wrapper">
        <div className="modal-dialog">
          {children}
          <div className="buttons">
            <button type="button" className="btn cancel" onClick={handleClick}>閉じる</button>
          </div>
        </div>
      </div>
    </>
  );
}

export default ModalDialog;