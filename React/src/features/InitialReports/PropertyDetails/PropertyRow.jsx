import React, { useRef } from 'react';
import { useDrag, useDrop } from 'react-dnd';
import { useHandleInput } from '../../../helpers/useHandleInput';

const PropertyRow = ({ itemType, index, detail, updateAction, moveDetail }) => {
  const { inputValueChanged, inputCheckedChanged } = useHandleInput(updateAction, 'detail', detail);

  const ref = useRef(null)
  const [, drop] = useDrop({
    accept: itemType,
    hover(item, monitor) {
      if (!ref.current) {
        return
      }
      const dragIndex = item.index
      const hoverIndex = index
      // Don't replace items with themselves
      if (dragIndex === hoverIndex) {
        return
      }
      // Determine rectangle on screen
      const hoverBoundingRect = ref.current.getBoundingClientRect()
      // Get vertical middle
      const hoverMiddleY =
        (hoverBoundingRect.bottom - hoverBoundingRect.top) / 2
      // Determine mouse position
      const clientOffset = monitor.getClientOffset()
      // Get pixels to the top
      const hoverClientY = clientOffset.y - hoverBoundingRect.top
      // Only perform the move when the mouse has crossed half of the items height
      // When dragging downwards, only move when the cursor is below 50%
      // When dragging upwards, only move when the cursor is above 50%
      // Dragging downwards
      if (dragIndex < hoverIndex && hoverClientY < hoverMiddleY) {
        return
      }
      // Dragging upwards
      if (dragIndex > hoverIndex && hoverClientY > hoverMiddleY) {
        return
      }
      // Time to actually perform the action
      moveDetail(dragIndex, hoverIndex)
      // Note: we're mutating the monitor item here!
      // Generally it's better to avoid mutations,
      // but it's good here for the sake of performance
      // to avoid expensive index searches.
      item.index = hoverIndex
    },
  })
  const [{ isDragging }, drag] = useDrag({
    item: {
      type: itemType,
      propertyID: detail.propertyID,
      index
    },
    collect: monitor => ({
      isDragging: monitor.isDragging(),
    }),
  })
  const opacity = isDragging ? 0 : 1
  drag(drop(ref))

  const style = { cursor: 'move' };
  const propertyNameSize = detail.propertyName.length > 10 ? 13.5 : 15;

  const checkboxId = `checkbox_${itemType}_${detail.propertyID}`;
  return (
    <div className="property-table-data" ref={ref} style={{ ...style, opacity }}>
      <div className="property-1">
        <input type="checkbox" id={checkboxId} defaultChecked={detail.isShow} onChange={inputCheckedChanged('isShow')} />
        <label htmlFor={checkboxId}></label>
      </div>
      <div className="property-2">
        <input type="text" maxLength="12" value={detail.propertyName} onChange={inputValueChanged('propertyName')} style={{ fontSize: propertyNameSize }} />
      </div>
      <div className="property-3">
        <input type="number" min={0} max={9999999999} value={detail.evaluationAmount} onChange={inputValueChanged('evaluationAmount')} />
      </div>
      <div className="property-4">
        {(() => {
          if (detail.isDeductionTarget) {
            return (
              <input type="number" min={0} max={9999999999} value={detail.deductionAmount} onChange={inputValueChanged('deductionAmount')} placeholder={detail.deductionTitle} />
            );
          } else {
            return (
              <></>
            );
          }
        })()}
      </div>
      <div className="property-5">
        <p>{detail.inheritanceAmount}</p>
      </div>
    </div>
  );
}

export default PropertyRow;