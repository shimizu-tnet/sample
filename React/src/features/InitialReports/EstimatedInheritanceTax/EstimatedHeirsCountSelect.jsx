import React from 'react';
import { updateEstimatedHeirsCount } from './EstimatedInheritanceTaxSlice';
import { useDispatch } from 'react-redux';

const EstimatedHeirsCountSelect = ({ maxEstimatedHeirsCount, estimatedHeirsCount }) => {
  const dispatch = useDispatch();
  const handleSelectChanged = e => dispatch(updateEstimatedHeirsCount({ estimatedHeirsCount: e.target.value }));

  const options = [...Array(maxEstimatedHeirsCount).keys()].map((v, i) => {
    const value = v + 1;
    return (<option key={i} value={value}>{value}</option>);
  });

  return (
    <>
      <label htmlFor="estimatedHeirsCount" style={{ width: 135, marginTop: 3 }}>見積上の相続人数</label>
      <select id="estimatedHeirsCount" value={estimatedHeirsCount} onChange={handleSelectChanged}>
        {options}
      </select>
    </>
  );
}

export default EstimatedHeirsCountSelect;