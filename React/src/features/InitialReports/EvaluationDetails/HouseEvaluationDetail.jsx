import React, { useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useDrag, useDrop } from 'react-dnd';
import { itemTypes } from '../../../consts/itemType';
import {
  deleteDetail,
  updateDetail,
  getMagnification,
  calculateEvaluation,
  calculateTotalEvaluation,
} from './HouseEvaluationsSlice';
import CurrencyParagraph from '../../../components/CurrencyParagraph';

/** 自用 */
const forOwn = "0";
/** 貸家 */
const forRent = "1";

const HouseEvaluationDetail = ({ index, detail, moveDetail }) => {
  const dispatch = useDispatch();
  const details = [...useSelector(state => state.houseEvaluations.details)];
  const updateHouseEvaluationDetail = editDetail => {
    editDetail.evaluation = calculateEvaluation(editDetail);
    const index = details
      .map(item => item.houseID)
      .indexOf(editDetail.houseID);
    details[index] = editDetail;

    const totalEvaluation = calculateTotalEvaluation(details);
    dispatch(updateDetail({
      index,
      detail: editDetail,
      totalEvaluation,
    }));
  };

  const handleUsageCategoryChanged = e => {
    const editDetail = { ...detail, usageCategory: e.target.value }
    editDetail.magnification = getMagnification(editDetail.usageCategory);
    updateHouseEvaluationDetail(editDetail);
  }

  const handleValueChanged = key => e => {
    const editDetail = { ...detail, [key]: e.target.value }
    updateHouseEvaluationDetail(editDetail);
  }

  /** 評価明細削除ボタン押下 */
  const handleDeleteDetailClicked = index => () => {
    const filtered = details.filter((v, i) => i !== index);
    const totalEvaluation = calculateTotalEvaluation(filtered);
    dispatch(deleteDetail({ details: filtered, totalEvaluation }));
  };

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

  const style = { cursor: 'move' };

  return (
    <div className="house-table-wrap" ref={ref} style={{ ...style, opacity }}>
      <div className="house-table-data">
        <div className="house-data">
          {/* 家屋No. */}
          <div className="house-10">
            <input type="text" size="5" value={detail.symbol} onChange={handleValueChanged("symbol")} />
          </div>
          {/* 所在地番 */}
          <div className="house-11">
            <textarea value={detail.address} onChange={handleValueChanged("address")} ></textarea>
          </div>
          {/* 計算方法 */}
          <div className="house-12">
            <input type="radio" name={`usageCategory_name${detail.houseID}`} className="radio-input" id={`usageCategory_forOwn${detail.houseID}`}
              value={forOwn} checked={detail.usageCategory === forOwn} onChange={handleUsageCategoryChanged} />
            <label htmlFor={`usageCategory_forOwn${detail.houseID}`} className="font-wightR">自用</label>
            <br />
            <input type="radio" name={`usageCategory_name${detail.houseID}`} className="radio-input" id={`usageCategory_forRent${detail.houseID}`}
              value={forRent} checked={detail.usageCategory === forRent} onChange={handleUsageCategoryChanged} />
            <label htmlFor={`usageCategory_forRent${detail.houseID}`} className="font-wightR">貸家</label>
          </div>
          {/* 固定資産評価額 */}
          <div className="house-13">
            <input type="number" size="7" value={detail.propertyEvaluation} onChange={handleValueChanged("propertyEvaluation")} />
          </div>
          {/* 倍率 */}
          <div className="house-14">
            <input type="number" min={0.00} max={999.99} step={0.01} value={detail.magnification} onChange={handleValueChanged("magnification")} />
          </div>
          {/* 持分 */}
          <div className="house-15">
            <input type="number" size="2" value={detail.molecule} onChange={handleValueChanged("molecule")} />
            <span>/</span>
            <input type="number" size="2" value={detail.denominator} onChange={handleValueChanged("denominator")} />
          </div>
          {/* 相続税評価額 */}
          <div className="house-16">
            <CurrencyParagraph value={detail.evaluation} />
          </div>
          {/* 編集 */}
          <div className="house-17">
            <div>
              <button type="button" className="btn edit-btn" onClick={handleDeleteDetailClicked(index)}>削除</button>
            </div>
          </div>
          {/* 物件名（使用者） */}
          <div className="house-18">
            <input type="text" size="20" value={detail.user} onChange={handleValueChanged("user")} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default HouseEvaluationDetail;