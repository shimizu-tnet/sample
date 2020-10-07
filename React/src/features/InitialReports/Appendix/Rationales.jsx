import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { itemTypes } from '../../../consts/itemType';
import Rationale from './Rationale';
import RationaleDeduction from './RationaleDeduction';
import {
  updateProperty,
  updateDebt,
  initializeRationales,
  landPropertyID,
} from '../PropertyDetails/PropertyDetailsSlice';

const Rationales = () => {
  const { properties, debts } = useSelector(state => state.propertyDetails);
  const dispatch = useDispatch();
  const handleInitializeClicked = () => dispatch(initializeRationales());
  const landEvaluations = useSelector(state => state.landEvaluations.details);
  const showLandDeductionRationale = () => [...landEvaluations].filter(detail => detail.showDevaluation).length > 0;

  return (
    <form>
      <div>
        <button type="button" className="btn edit-btn" onClick={handleInitializeClicked}>初期値に戻す</button>
      </div>
      <div className="input-table">
        <div className="input-table-header-row rationale-row">
          <div className="input-table-header-cell">
            <p>財産名称</p>
          </div>
          <div className="input-table-header-cell">
            <p>特例・非課税額控除前の<br />相続評価額</p>
          </div>
          <div className="input-table-header-cell">
            <p>相続税評価額ベース</p>
          </div>
        </div>

        {properties.filter(x => x.isShow).map(detail => {
          return (
            <React.Fragment key={`${itemTypes.PROPERTY}_${detail.propertyID}`}>
              <Rationale rationale={detail} updateAction={updateProperty} />
              {detail.propertyID === landPropertyID && showLandDeductionRationale()
                && <RationaleDeduction rationale={detail} updateAction={updateProperty} />}
            </React.Fragment>
          );
        })}
        {debts.filter(x => x.isShow).map(detail => {
          return (<Rationale key={`${itemTypes.DEBT}_${detail.propertyID}`} rationale={detail} updateAction={updateDebt} />);
        })}
      </div>
    </form>
  )
}

export default Rationales;