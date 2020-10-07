import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { changeTaxInvestigationMeasures } from './TaxInvestigationMeasuresSlice';
import { TaxInvestigationMeasuresType } from './TaxInvestigationMeasuresType';
import img_detail from '../../../images/7-detail.png';
import img_simple from '../../../images/7-simple.png';

const TaxInvestigationMeasuresSelection = () => {
    const { taxInvestigationMeasuresType } = useSelector(
        state => state.taxInvestigationMeasures
    );

    const isDetails = taxInvestigationMeasuresType === TaxInvestigationMeasuresType.Details
    const detailsStyle = { opacity: isDetails ? 1 : 0.3 };
    const simpleStyle = { opacity: !isDetails ? 1 : 0.3 };
    const dispatch = useDispatch();
    const handleClick = type => {
        if (type === taxInvestigationMeasuresType) {
            return;
        }

        dispatch(changeTaxInvestigationMeasures({ taxInvestigationMeasuresType: type }));
    }

    return (
        <>
            <table style={{ borderCollapse: "collapse" }}>
                <tbody>
                    <tr>
                        <td style={{ border: "1px solid rgb(80, 80, 80)" }}>
                            <input id="radio_detail" type="radio" name="taxInvestigationMeasures" checked={isDetails}
                                onChange={() => handleClick(TaxInvestigationMeasuresType.Details)} />
                            <label htmlFor="radio_detail">詳細版</label>
                        </td>
                        <td style={{ border: "1px solid rgb(80, 80, 80)" }}>
                            <input id="radio_simple" type="radio" name="taxInvestigationMeasures" checked={!isDetails}
                                onChange={() => handleClick(TaxInvestigationMeasuresType.Simple)} />
                            <label htmlFor="radio_simple">簡易版</label>
                        </td>
                    </tr>
                    <tr>
                        <td style={{ width: 450, border: "1px solid rgb(80, 80, 80)" }}>
                            <img src={img_detail} alt="詳細版" id="img_detail" width="440" style={detailsStyle} />
                        </td>
                        <td style={{ width: 450, border: "1px solid rgb(80, 80, 80)" }}>
                            <img src={img_simple} alt="簡易版" id="img_simple" width="440" style={simpleStyle} />
                        </td>
                    </tr>
                </tbody>
            </table>
        </>
    );
}

export default TaxInvestigationMeasuresSelection;