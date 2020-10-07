import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updateDescription } from './EstimatedInheritanceTaxSlice';

const Description = props => {
    const { description } = props;
    const { inputValueChanged, inputCheckedChanged } = useHandleInput(updateDescription, 'description', description);
    const checkboxId = `description${description.index}`;
    return (
        <div>
            <input type="checkbox" id={checkboxId} checked={description.isShow} onChange={inputCheckedChanged('isShow')} />
            <label htmlFor={checkboxId}>表示</label>
            <textarea value={description.text} onChange={inputValueChanged('text')} cols="100" rows="5" style={{ resize: 'vertical' }}></textarea>
        </div>
    );
};

export default Description;