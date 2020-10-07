import React, { useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useDrag, useDrop } from 'react-dnd';
import { itemTypes } from '../../../consts/itemType';
import LandEvaluationsForm from './LandEvaluationsForm';
import LandEvaluationDetailRow from './LandEvaluationDetailRow';
import {
  updateDetail,
  deleteDetail,
  sumEvaluations,
  sumDevaluations,
} from './LandEvaluationsSlice';
import { toZeroIfEmpty } from '../../../helpers/numberHelper';

/**
 * 評価明細から減額に表示する項目を抽出した新しいオブジェクトを返します。
 * @param detail 評価明細。
 */
const extractDevaluation = detail => {
  return ({
    ...detail,
    isEvaluation: false,
    symbol: detail.devaluationSymbol,
    address: detail.devaluationAddress,
    districtCategory: detail.devaluationDistrictCategory,
    area: detail.devaluationArea,
    ownLandEvaluation: detail.devaluationOwnLandEvaluation,
    rightsRatio: detail.devaluationRightsRatio,
    denominator: detail.devaluationDenominator,
    molecule: detail.devaluationMolecule,
    evaluation: detail.devaluation,
    showDevaluation: false,
  });
};

const LandEvaluationDetail = ({ index, detail, moveDetail }) => {
  const dispatch = useDispatch();

  /** 評価明細編集ボタン押下 */
  const handleEditClicked = controlID => e => {
    document.getElementById(controlID).showModal();
    e.preventDefault();
  };

  const details = useSelector(state => state.landEvaluations.details);
  /** 評価明細削除ボタン押下 */
  const handleDeleteDetailClicked = index => () => {
    const filtered = details.filter((v, i) => i !== index);
    const totalEvaluation = sumEvaluations(filtered);
    const totalDevaluation = sumDevaluations(filtered);
    const taxableAmount = totalEvaluation + totalDevaluation;
    dispatch(deleteDetail({
      details: filtered,
      totalEvaluation,
      totalDevaluation,
      taxableAmount,
    }));
  };

  /** 評価明細確定ボタン押下 */
  const handleSubmitClicked = controlID => detail => () => {
    const excludedDetails = details
      .filter(v => v.landID !== detail.landID);
    const totalEvaluation = sumEvaluations(excludedDetails)
      + toZeroIfEmpty(detail.evaluation);
    const totalDevaluation =
      detail.showDevaluation
        ? sumDevaluations(excludedDetails) + toZeroIfEmpty(detail.devaluation)
        : sumDevaluations(excludedDetails);
    const taxableAmount = totalEvaluation + totalDevaluation;

    dispatch(updateDetail({ detail, totalEvaluation, totalDevaluation, taxableAmount }));
    document.getElementById(controlID).close();
  }

  const ref = useRef(null)
  const [, drop] = useDrop({
    accept: itemTypes.LAND,
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
      type: itemTypes.LAND,
      landID: detail.landID,
      index
    },
    collect: monitor => ({
      isDragging: monitor.isDragging(),
    }),
  });
  const opacity = isDragging ? 0 : 1
  drag(drop(ref));

  const controlID = `land${detail.landID}`;
  const style = { cursor: 'move' }

  return (
    <div className="land-table-wrap" ref={ref} style={{ ...style, opacity }}>
      <div className="land-table-data">
        <LandEvaluationDetailRow key={`${detail.landID}${detail.showDevaluation}`} detail={detail} className="land-data" />
        {(() => {
          if (!detail.showDevaluation) {
            return (<></>);
          };
          const devaluation = extractDevaluation(detail);
          return (<LandEvaluationDetailRow key={`${devaluation.landID}${devaluation.showDevaluation}`} detail={devaluation} className="land-reduction-data" />);
        })()}
        <div className="land-data-edit-area">
          <div>
            <button className="btn edit-btn" onClick={handleEditClicked(controlID)}>編集</button>
          </div>
          <div>
            <button className="btn edit-btn" onClick={handleDeleteDetailClicked(index)}>削除</button>
          </div>
        </div>
        <LandEvaluationsForm controlID={controlID} detail={detail} handleSubmitClicked={handleSubmitClicked(controlID)}></LandEvaluationsForm>
      </div>
    </div>
  );
}

export default LandEvaluationDetail;