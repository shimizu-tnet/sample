import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { initializeTaxPayments, createTaxPaymentsParameter } from './TaxPaymentSlice';
import TaxPaymentItem from './TaxPaymentItem';

const TaxPaymentList = () => {
    const state = useSelector(state => state);
    const { taxPayments } = state.taxPayment;
    const rows = taxPayments.map((item, index) =>
        <TaxPaymentItem key={index} taxPayment={item} />);
    const dispatch = useDispatch();
    const taxPaymentsParameter = createTaxPaymentsParameter(state);
    const handleInitialize = () => dispatch(initializeTaxPayments(taxPaymentsParameter));

    return (
        <>
            <div>
                <button type="button" className="btn edit-btn" onClick={handleInitialize}>初期値に戻す</button>
                <span>※初期値に戻すボタン押下により当ページが文章を含め初期画面にリセットされます。</span>
            </div>
            <div className="onecolumn">
                {rows}
            </div>
        </>
    );
}

export default TaxPaymentList;