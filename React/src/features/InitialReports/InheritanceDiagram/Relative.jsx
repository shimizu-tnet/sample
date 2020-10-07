import React, { useRef } from 'react';
import { useDrag, useDrop } from 'react-dnd'
import { itemTypes } from '../../../consts/itemType'
import Relationship from './Relationship'
import { useHandleInput } from '../../../helpers/useHandleInput';
import {
  updateReative,
} from '../InheritanceDiagram/InheritanceDiagramSlice';
import RelationshipSelect from './RelationshipSelect';
import TargetRelativeSelect from './TargetRelativeSelect';

const Relative = (props) => {
  const { index, relative, targetRelatives, heirPattern, handleDeleteRelative, moveRelative } = props;
  const relativeID = relative.relativeID;
  const { inputValueChanged, inputCheckedChanged } = useHandleInput(updateReative, 'relative', relative);

  const ref = useRef(null)
  const [, drop] = useDrop({
    accept: itemTypes.RELATIVE,
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
      moveRelative(dragIndex, hoverIndex)
      // Note: we're mutating the monitor item here!
      // Generally it's better to avoid mutations,
      // but it's good here for the sake of performance
      // to avoid expensive index searches.
      item.index = hoverIndex
    },
  })
  const [{ isDragging }, drag] = useDrag({
    item: {
      type: itemTypes.RELATIVE,
      relativeID,
      index
    },
    collect: monitor => ({
      isDragging: monitor.isDragging(),
    }),
  })
  drag(drop(ref))

  const opacity = isDragging ? 0 : 1
  const style = { cursor: 'move' }
  const isDeceased = (relative.relationship === Relationship.Deceased);

  // 相続権チェックボックス スタイル
  const IsHeirStyle = {
    visibility: isDeceased ? "hidden" : "visible"
  }
  const IsHeirDisabled = relative.abandonedInheritance;

  // 相続放棄チェックボックス スタイル
  const AbandonedInheritanceStyle = {
    visibility: isDeceased ? "hidden" : "visible"
  }

  return (
    <div className="input-table-row inheritance-row" ref={ref} style={{ ...style, opacity }}>
      <div className="input-table-cell">
        <div>
          <RelationshipSelect heirPattern={heirPattern} relationship={relative.relationship} handleRelativeValueChanged={inputValueChanged('relationship')} />
          <input type="text" value={relative.supplement} onChange={inputValueChanged('supplement')} size={10} />
        </div>
      </div>
      <div className="input-table-cell">
        <div>
          <input type="text" placeholder="氏（漢字）" value={relative.lastName} onChange={inputValueChanged('lastName')} size={5} />
          <input type="text" placeholder="名（漢字）" value={relative.firstName} onChange={inputValueChanged('firstName')} size={5} />
        </div>
      </div>
      <div className="input-table-cell">
        <div>
          <input type="text" placeholder="氏（カナ）" value={relative.lastNameKana} onChange={inputValueChanged('lastNameKana')} size={5} />
          <input type="text" placeholder="名（カナ）" value={relative.firstNameKana} onChange={inputValueChanged('firstNameKana')} size={5} />
        </div>
      </div>
      <div className="input-table-cell">
        <input type="number" value={relative.age} onChange={inputValueChanged('age')} min={1} max={999} />
      </div>
      <div className="input-table-cell">
        <input type="checkbox" id={`isHeir${relativeID}`} checked={relative.isHeir} onChange={inputCheckedChanged('isHeir')} style={IsHeirStyle} disabled={IsHeirDisabled} />
        <label htmlFor={`isHeir${relativeID}`}></label>
      </div>
      <div className="input-table-cell">
        <input type="checkbox" id={`abandonedInheritance${relativeID}`} checked={relative.abandonedInheritance} onChange={inputCheckedChanged('abandonedInheritance')} style={AbandonedInheritanceStyle} />
        <label htmlFor={`abandonedInheritance${relativeID}`}></label>
      </div>
      <div className="input-table-cell">
        <TargetRelativeSelect relative={relative} targetRelatives={targetRelatives} handleTargetRelativeValueChanged={inputValueChanged('targetRelativeID')} />
      </div>
      <div className="input-table-cell">
        {!isDeceased &&
          <button type="button" className="btn edit-btn cancel" onClick={() => handleDeleteRelative(relativeID)}>削除</button>}
      </div>
    </div>
  )
}

export default Relative;