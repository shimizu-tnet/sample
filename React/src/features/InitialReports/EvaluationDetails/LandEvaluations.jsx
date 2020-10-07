import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import LandEvaluationHeader from './LandEvaluationHeader';
import LandEvaluationList from './LandEvaluationList';
import {
  addDetail,
  addComment,
  deleteComment,
  updateComment,
} from './LandEvaluationsSlice';
import CurrencyParagraph from '../../../components/CurrencyParagraph';

/** 土地・借地権の評価明細一覧表 */
const LandEvaluations = props => {

  const {
    details,
    comments,
    totalEvaluation,
    totalDevaluation,
    taxableAmount,
  } = useSelector(
    state => state.landEvaluations
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
    <form id="land">
      <LandEvaluationHeader />
      <LandEvaluationList details={details} />
      <div className="row-addition">
        <div>
          <button type="button" className="btn edit-btn" onClick={handleAddDetailClicked}>行を追加</button>
        </div>
      </div>
      <div className="total">
        <div className="evaluation-amount-sum">
          <div className="title">
            <p>評価額合計</p>
          </div>
          <div className="total-amount">
            <CurrencyParagraph value={totalEvaluation} />
          </div>
        </div>
        <div className="evaluation-decrease-sum">
          <div className="title">
            <p>評価減額合計</p>
          </div>
          <div className="total-amount">
            <CurrencyParagraph value={totalDevaluation} />
          </div>
        </div>
        <div className="deduction-taxation-target">
          <div className="title">
            <p>差引課税対象</p>
          </div>
          <div className="total-amount">
            <CurrencyParagraph value={taxableAmount} />
          </div>
        </div>
      </div>
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
      <div id="dialogArea">
      </div>
    </form>
  )
}

export default LandEvaluations;