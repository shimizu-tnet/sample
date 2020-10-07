import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { initializePropertyEvaluations } from './PropertyEvaluationSlice';
import PropertyEvaluationItem from './PropertyEvaluationItem';

const PropertyEvaluationList = () => {
    const { propertyEvaluations } = useSelector(
        state => state.propertyEvaluation
    );
    const rows = propertyEvaluations.map((item, index) =>
        <PropertyEvaluationItem key={index} propertyEvaluation={item} />);
    const dispatch = useDispatch();
    const handleInitialize = () => dispatch(initializePropertyEvaluations());

    return (
        <>
            <div>
                <button type="button" className="btn edit-btn" onClick={handleInitialize}>初期値に戻す</button>
            </div>
            <div className="onecolumn">
                {rows}
            </div>
        </>
    );
}

export default PropertyEvaluationList;