import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updateOption } from './FeeSlice';

const Option = props => {
  const { option } = props;
  const { inputValueChanged } = useHandleInput(updateOption, 'option', option);
  return (
    <div className="input-table-row fee-row">
      <div></div>
      <input type="text" value={option.optionName} onChange={inputValueChanged('optionName')} />
      <input type="text" value={option.moneyAmount || ''} onChange={inputValueChanged('moneyAmount')} />
      <div>円</div>
    </div>
  );
}

export default Option;