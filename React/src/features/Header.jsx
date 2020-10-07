import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { getPdf } from '../api/legacyDocument';
import { savePdf } from '../api/platformSystem';
import { pdfParamaToJson } from '../app/pdfParamSelector';
import { setApiFailure, setError, hasResponseError } from './Errors/ErrorSlice';
import { isSelectedAny } from './NavigationSlice';

const Header = ({ pageTitle }) => {
  const dispatch = useDispatch();
  const history = useHistory();
  const navigation = useSelector(state => state.navigation);
  const paramValues = useSelector(pdfParamaToJson);
  const { accessToken } = useSelector(state => state.login);
  const list = useSelector(state => state.platformSystem);
  const { ankenId, ankenName, subject } = list;

  const registerPdf = (paramValues, accessToken) => async () => {
    try {
      // 目次は初期報告書、見積書のどちらか片方のみを必須とする
      // 初期報告書のみの場合や見積書のみの場合を想定
      const outputInitialReport = isSelectedAny('initialReport', navigation);
      const outputEstimatesReport = isSelectedAny('estimatesReport', navigation);
      if (!outputInitialReport && !outputEstimatesReport) {
        dispatch(setError({ errorMessages: ['目次をひとつ以上選択してください。'] }));
        return;
      }

      let initialReport = null;
      let estimatesReport = null;

      if (outputInitialReport && outputEstimatesReport) {
        const reports = await Promise.all([
          getPdf('initialReport', paramValues, accessToken),
          getPdf('estimatesReport', paramValues, accessToken),
        ]);

        initialReport = reports[0];
        estimatesReport = reports[1];
        window.open(URL.createObjectURL(initialReport));
        window.open(URL.createObjectURL(estimatesReport));

      } else if (outputInitialReport && !outputEstimatesReport) {
        initialReport = await getPdf('initialReport', paramValues, accessToken);
        const url = URL.createObjectURL(initialReport);
        window.open(url);

      } else if (!outputInitialReport && outputEstimatesReport) {
        estimatesReport = await getPdf('estimatesReport', paramValues, accessToken);
        const url = URL.createObjectURL(estimatesReport);
        window.open(url);
      }

      await savePdf(
        list,
        initialReport,
        estimatesReport,
        paramValues
      );

    } catch (err) {
      console.error(err);
      if (hasResponseError(err)) {
        dispatch(setApiFailure({ response: err.response }));
      } else {
        dispatch(setError({ errorMessages: ['処理でエラーが発生したため、再度実行してください。'] }));
      }
    }
  }
  const returnAnkenList = () => history.push({ pathname: '/list', state: { referrer: '/input' } });

  return (
    <header>
      <div className="header-anken">
        <div></div>
        <div>{`案件No：${ankenId}`}</div>
        <div>{`案件名：${ankenName}`}</div>
        <div>{`件名：${subject}`}</div>
      </div>
      <div className="header-title">
        <h1>{pageTitle.title}</h1>
        <h2>{pageTitle.reportTitle}</h2>
      </div>
      <div className="header-nav">
        <ul>
          <li>
            <button type="button" className="header-nav-link" onClick={registerPdf(paramValues, accessToken)}>保存</button>
          </li>
          <li>
            <button type="button" className="header-nav-link" onClick={returnAnkenList}>一覧に戻る</button>
          </li>
        </ul>
      </div>
    </header>
  );
}

export default Header;
