import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updateTaxAccountantWorks } from './BusinessDescriptionSlice';

const TaxAccountantWork = props => {
  const { taxAccountantWork } = props;
  const { inputValueChanged } = useHandleInput(updateTaxAccountantWorks, 'taxAccountantWork', taxAccountantWork);

  return (
    <div>
      <input type="text" value={taxAccountantWork.text} onChange={inputValueChanged('text')} />
    </div>
  );
};

export default TaxAccountantWork;