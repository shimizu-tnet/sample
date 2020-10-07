import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import HouseEvaluationHeader from './HouseEvaluationHeader';
import HouseEvaluationList from './HouseEvaluationList';
import {
  addDetail,
  addComment,
  deleteComment,
  updateComment,
} from './HouseEvaluationsSlice';
import CurrencyParagraph from '../../../components/CurrencyParagraph';

const HouseEvaluations = () => {
  const {
    comments,
    totalEvaluation,
  } = useSelector(
    state => state.houseEvaluations
  );

  const dispatch = useDispatch();

  /** 評価明細追加ボタン押下 */
  const handleAddDetailClicked = () => {
    dispatch(addDetail());
  };

  /** コメント追加ボタン押下 */
  const handleAddCommentClicked = () => {
    dispatch(addComment());
  };

  /** コメント削除ボタン押下 */
  const handleDeleteCommentClicked = index => () => {
    dispatch(deleteComment({ index }));
  };

  /** コメントの変更 */
  const handleCommentChanged = index => e => {
    dispatch(updateComment({ index, value: e.target.value }));
  }

  return (
    <form id="house">
      <HouseEvaluationHeader />
      <HouseEvaluationList />
      <div className="row-addition">
        <button type="button" className="btn edit-btn" onClick={handleAddDetailClicked}>行を追加</button>
      </div>
      <div className="total">
        <div className="evaluation-amount-sum">
          <div className="title">
            <p>合計</p>
          </div>
          <div className="total-amount">
            <CurrencyParagraph value={totalEvaluation} />
          </div>
        </div>
      </div>
      <br />
      <div className="comment-area">
        <div className="title">
          <p>注釈</p>
        </div>
        {comments.map((v, i) => {
          return (
            <div key={i} className="comment">
              <input type="text" value={v} onChange={handleCommentChanged(i)} />
              <button type="button" className="btn edit-btn" onClick={handleDeleteCommentClicked(i)}>削除</button>
            </div>
          )
        })}
        <div className="comment-addition">
          <button type="button" className="btn edit-btn" onClick={handleAddCommentClicked}>入力欄を追加</button>
        </div>
      </div>
    </form>
  )
}

export default HouseEvaluations;