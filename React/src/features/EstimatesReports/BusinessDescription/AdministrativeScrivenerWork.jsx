import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updatetaxAdministrativeScrivenerWorks } from './BusinessDescriptionSlice';

const AdministrativeScrivenerWork = props => {
  const { administrativeScrivenerWork } = props;
  const { inputValueChanged } = useHandleInput(updatetaxAdministrativeScrivenerWorks, 'administrativeScrivenerWork', administrativeScrivenerWork);

  return (
    <div>
      <input type="text" value={administrativeScrivenerWork.text} onChange={inputValueChanged('text')} />
    </div>
  );
};

export default AdministrativeScrivenerWork;