import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updateTaxPayment } from './TaxPaymentSlice';

const TaxPaymentItem = props => {
    const { taxPayment } = props;
    const { inputValueChanged, inputCheckedChanged } = useHandleInput(updateTaxPayment, 'taxPayment', taxPayment);
    const checkboxId = `taxPayment${taxPayment.index}`;

    return (
        <div>
            <div>
                <input type="checkbox" id={checkboxId} checked={taxPayment.isShow} onChange={inputCheckedChanged('isShow')} />
                <label htmlFor={checkboxId}>表示</label>
            </div>
            <div>
                <textarea value={taxPayment.text} onChange={inputValueChanged('text')} cols="100" rows="5" style={{ resize: 'vertical' }}></textarea>
            </div>
        </div>
    );
};

export default TaxPaymentItem;