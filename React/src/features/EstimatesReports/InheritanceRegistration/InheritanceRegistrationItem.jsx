import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updateInheritanceRegistration } from './InheritanceRegistrationSlice';

const InheritanceRegistrationItem = props => {
  const { inheritanceRegistration } = props;
  const { inputValueChanged } = useHandleInput(updateInheritanceRegistration, 'inheritanceRegistration', inheritanceRegistration);

  return (
    <div>
      <textarea rows="5" onChange={inputValueChanged('text')} value={inheritanceRegistration.text}></textarea>
    </div>
  );
};

export default InheritanceRegistrationItem;
