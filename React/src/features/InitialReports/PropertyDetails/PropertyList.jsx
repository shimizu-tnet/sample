import React, { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import update from 'immutability-helper';
import { updateProperty, updateDebt, setProperty, setDebt } from './PropertyDetailsSlice';
import { itemTypes } from '../../../consts/itemType';
import PropertyRow from './PropertyRow';

const PropertyList = ({ itemType, details }) => {
  const updateAction = itemType === itemTypes.PROPERTY ? updateProperty : updateDebt;
  const dragAction = itemType === itemTypes.PROPERTY ? setProperty : setDebt;
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
      dispatch(dragAction({ replacedDetails }));
    },
    [dispatch, details, dragAction],
  );

  return (details.map((detail, index) => {
    return (<PropertyRow key={`${itemType}_${detail.propertyID}`} itemType={itemType} index={index} detail={detail} updateAction={updateAction} moveDetail={moveDetail} />)
  }));
}

export default PropertyList;