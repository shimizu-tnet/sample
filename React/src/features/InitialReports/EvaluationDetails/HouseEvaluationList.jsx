import React, { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import update from 'immutability-helper';
import HouseEvaluationDetail from './HouseEvaluationDetail';
import { setDetails } from './HouseEvaluationsSlice';

const HouseEvaluationList = () => {
  const {
    details,
  } = useSelector(
    state => state.houseEvaluations
  );

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
    return (<HouseEvaluationDetail key={`houseID_${detail.houseID}`} index={index} detail={detail} moveDetail={moveDetail} />);
  }));
}

export default HouseEvaluationList;