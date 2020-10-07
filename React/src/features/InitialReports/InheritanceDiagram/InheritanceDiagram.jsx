import React, { useCallback } from 'react';
import Relative from './Relative';
import { useSelector, useDispatch } from 'react-redux';
import {
  changeHeirPattern,
  addRelative,
  deleteRelative,
  setRelatives,
} from '../InheritanceDiagram/InheritanceDiagramSlice';
import update from 'immutability-helper'
import { isTargetRelative } from './Relationship'

const InheritanceDiagram = props => {
  const dispatch = useDispatch();
  const { heirPattern, relatives } = useSelector(
    state => state.inheritanceDiagram
  );
  const handleHeirPatternChanged = e => {
    dispatch(changeHeirPattern({ heirPattern: e.target.value }));
  };
  const handleAddRelative = () => dispatch(addRelative());
  const handleDeleteRelative = relativeID => dispatch(deleteRelative({ relativeID: relativeID }));
  const handleOpenHelpFile = () => window.open('/data/相続関係図操作説明.pdf');

  const moveRelative = useCallback(
    (dragIndex, hoverIndex) => {
      const dragRelative = relatives[dragIndex];
      const replacedRelatives = update(relatives, {
        $splice: [
          [dragIndex, 1],
          [hoverIndex, 0, dragRelative],
        ],
      });
      dispatch(setRelatives({ replacedRelatives }));
    },
    [dispatch, relatives],
  );

  return (
    <form id="inheritance">
      <div className="inheritance-pattern">
        <div className="inheritance-pattern-title">
          <label htmlFor="heirPattern">相続関係図パターン</label>
        </div>
        <div className="inheritance-pattern-select">
          <select id="heirPattern" value={heirPattern} onChange={handleHeirPatternChanged}>
            <option value={1}>第1順位（直系卑属）</option>
            <option value={2}>第2順位（直系尊属）</option>
            <option value={3}>第3順位（兄弟姉妹）</option>
          </select>
        </div>
        <div className="inheritance-help">
          <button type="button" onClick={handleOpenHelpFile} className="btn edit-btn">相続関係図操作説明</button>
        </div>
      </div>
      <div className="input-table">
        <div className="input-table-header-row inheritance-row">
          <div className="input-table-header-cell">
            <p>関係・続柄</p>
          </div>
          <div className="input-table-header-cell">
            <p>氏名（漢字）</p>
          </div>
          <div className="input-table-header-cell">
            <p>氏名（カナ）</p>
          </div>
          <div className="input-table-header-cell">
            <p>年齢</p>
          </div>
          <div className="input-table-header-cell">
            <p>相続権</p>
          </div>
          <div className="input-table-header-cell">
            <p>放棄</p>
          </div>
          <div className="input-table-header-cell">
            <p>血縁者</p>
          </div>
          <div className="input-table-header-cell">
            <p>削除</p>
          </div>
        </div>
        {relatives.map((relative, index) => {
          const targetRelatives = relatives.filter(isTargetRelative(relative.relationship));
          return (
            <Relative
              key={relative.relativeID}
              index={index}
              heirPattern={heirPattern}
              relative={relative}
              targetRelatives={targetRelatives}
              moveRelative={moveRelative}
              handleDeleteRelative={handleDeleteRelative}
            />
          )
        })}
        <div className="row-addition">
          <button type="button" className="btn edit-btn" onClick={handleAddRelative}>行を追加</button>
        </div>
      </div>
    </form>
  )
}

export default InheritanceDiagram;