import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { pdfParamaToObject } from '../../app/pdfParamSelector';
import { updatePdfParam } from '../UpdatePdfParamAction';
import { fetchRefer } from '../../api/platformSystem';
import { setApiFailure, setError, hasResponseError } from '../Errors/ErrorSlice';
import { deepMerge } from '../../helpers/parameterHelper';

const VersionsDialog = () => {
  const list = useSelector(state => state.platformSystem);
  const { ankenId, platformToken } = list;
  const parameters = useSelector(pdfParamaToObject);
  const dispatch = useDispatch();
  const history = useHistory();
  const handeleCreateNewEstimatesClicked = async () => {
    try {
      const proposal = await fetchRefer(ankenId, null, platformToken);
      const merged = deepMerge(parameters, proposal);

      dispatch(updatePdfParam(merged));
      history.push('/input');
    } catch (err) {
      console.error(err);
      if (hasResponseError(err)) {
        dispatch(setApiFailure({ response: err.response }));
      } else {
        dispatch(setError({ errorMessages: ['処理でエラーが発生したため、再度実行してください。'] }));
      }
    }
  }
  const handleCloseClicked = () => {
    document.getElementById('versionsDialog').close();
  }

  return (
    <dialog id="versionsDialog" className="versions-dialog">
      <h1>新規作成確認</h1>
      <h2>新規で提案書/見積書を作成しますか？</h2>
      <p className="dialog-anken-row">
        {`案件No：${ankenId}　案件名：${list.ankenName}`}
      </p>
      <button type="button" className="btn edit-btn" onClick={handeleCreateNewEstimatesClicked}>作成する</button>
      <button type="button" className="btn edit-btn cancel" onClick={handleCloseClicked}>作成しない</button>
    </dialog>
  );
}

export default VersionsDialog;
