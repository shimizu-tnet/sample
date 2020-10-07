import React, { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import update from 'immutability-helper';
import LandEvaluationDetail from './LandEvaluationDetail';
import { setDetails } from './LandEvaluationsSlice';

const LandEvaluationList = ({ details }) => {
  const dispatch = useDispatch();
  const moveDetail = useCallback(
    (dragIndex, hoverIndex) => {
      const dragDetail = details[dragIndex];
      const replacedDetails = update(details, {
        $splice: [
          [dragIndex, 1],
          [hoverIndex, 0, dragDetail],
        ],
      });
      dispatch(setDetails({ replacedDetails }));
    },
    [dispatch, details],
  );

  return (details.map((detail, index) => {
    return (<LandEvaluationDetail key={detail.landID} index={index} detail={detail} moveDetail={moveDetail} />)
  }));
}

export default LandEvaluationList;