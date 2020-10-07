import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { fetchInheritanceTax } from './EstimatedInheritanceTaxSlice';
import {
    calculateInheritanceTotalAmount,
    calculatePropertyTotalAmount,
} from '../PropertyDetails/PropertyDetailsSlice';
import EstimatedHeirsCountSelect from './EstimatedHeirsCountSelect';
import Description from './Description';

const maxEstimatedHeirsCount = 4;
const EstimatedInheritanceTax = () => {
    const { accessToken } = useSelector(state => state.login);
    const { descriptions, estimatedHeirsCount } = useSelector(state => state.estimatedInheritanceTax);
    const { relatives, heirPattern } = useSelector(state => state.inheritanceDiagram);
    const inheritanceTotalAmount = useSelector(calculateInheritanceTotalAmount);
    const propertyTotalAmount = useSelector(calculatePropertyTotalAmount);
    const rows = descriptions.map((item, index) => <Description key={index} description={item} />);
    const dispatch = useDispatch();
    const handleClick = () => {
        const inheritanceTaxParameters = {
            accessToken,
            inheritanceTotalAmount,
            propertyTotalAmount,
            relatives,
            heirPattern,
            estimatedHeirsCount
        };
        dispatch(fetchInheritanceTax(inheritanceTaxParameters));
    }


    return (
        <>
            <EstimatedHeirsCountSelect maxEstimatedHeirsCount={maxEstimatedHeirsCount} estimatedHeirsCount={estimatedHeirsCount} />
            <div style={{
                display: "grid",
                gridTemplateRows: "auto auto",
                gridTemplateColumns: "auto auto 1fr"
            }}>
                <div style={{ gridRow: "1 / 3", gridColumn: "1 / 2", marginRight: 5 }}>
                    <button type="button" className="btn edit-btn" onClick={handleClick}>相続税計算</button>
                </div>
                <div style={{ gridRow: "1 / 3", gridColumn: "2 / 3" }}>
                    <span>※</span>
                </div>
                <div style={{ gridRow: "1 / 2", gridColumn: "3 / 4" }}>
                    <span>相続税計算ボタン押下により当ページと納税方法のページが文章を含め初期画面にリセットされます。</span>
                </div>
                <div style={{ gridRow: "2 / 3", gridColumn: "3 / 4" }}>
                    <span>報酬ページの基本報酬額、相続人加算の金額を再計算し、上書きします。</span>
                </div>
            </div>
            <div className="onecolumn">
                {rows}
            </div>
        </>
    );
}

export default EstimatedInheritanceTax;