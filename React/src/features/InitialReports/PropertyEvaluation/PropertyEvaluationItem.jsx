import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updatePropertyEvaluation } from './PropertyEvaluationSlice';

const PropertyEvaluationItem = props => {
    const { propertyEvaluation } = props;
    const { inputValueChanged } = useHandleInput(updatePropertyEvaluation, 'propertyEvaluation', propertyEvaluation);
    return (
        <div>
            <div>
                <input type="text" value={propertyEvaluation.heading} onChange={inputValueChanged('heading')} size="70" style={{ marginBottom: 5, width: 500 }} />
            </div>
            <div>
                <textarea value={propertyEvaluation.text} cols="100" rows="5" onChange={inputValueChanged('text')} style={{ resize: 'vertical' }}></textarea>
            </div>
        </div>
    );
};

export default PropertyEvaluationItem;