import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';

const RationaleDeduction = ({ rationale, updateAction }) => {
  const { inputValueChanged } = useHandleInput(updateAction, 'detail', rationale);
  return (
    <div className="input-table-row rationale-row">
      <div className="input-table-header-cell">
        <p>{rationale.deductionTitle}</p>
      </div>
      <div className="input-table-cell">
        <input type="text" value={rationale.deductionCurrentExplanation} onChange={inputValueChanged('deductionCurrentExplanation')} />
      </div>
      <div className="input-table-cell">
        <input type="text" value={rationale.deductionInheritanceExplanation} onChange={inputValueChanged('deductionInheritanceExplanation')} />
      </div>
    </div>
  );
}

export default RationaleDeduction;