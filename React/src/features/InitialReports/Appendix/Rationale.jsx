import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';

const Rationale = ({ rationale, updateAction }) => {
  const { inputValueChanged } = useHandleInput(updateAction, 'detail', rationale);
  return (
    <div className="input-table-row rationale-row">
      <div className="input-table-header-cell">
        <p>{rationale.propertyName}</p>
      </div>
      <div className="input-table-cell">
        <input type="text" value={rationale.currentExplanation} onChange={inputValueChanged('currentExplanation')} />
      </div>
      <div className="input-table-cell">
        <input type="text" value={rationale.inheritanceExplanation} onChange={inputValueChanged('inheritanceExplanation')} />
      </div>
    </div>
  );
}

export default Rationale;