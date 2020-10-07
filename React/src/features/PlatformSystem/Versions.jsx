import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { pdfParamaToObject } from '../../app/pdfParamSelector';
import { fetchRefer } from '../../api/platformSystem';
import { updatePdfParam } from '../UpdatePdfParamAction';
import { updateProposal } from '../PlatformSystem/PlatformSystemSlice';
import VersionsDialog from './VersionsDialog';
import { setApiFailure, setError, hasResponseError } from '../Errors/ErrorSlice';
import { deepMerge } from '../../helpers/parameterHelper';

const Versions = ({ className }) => {
  const platformSystem = useSelector(state => state.platformSystem);
  const { platformToken, ankenId, proposalId } = platformSystem;
  const ankenList = useSelector(state => state.ankenList);
  const parameters = useSelector(pdfParamaToObject);
  const dispatch = useDispatch();
  const history = useHistory();
  const handleShowDialogClicked = () => {
    dispatch(updateProposal({
      proposalId: null,
      subject: '新規作成'
    }));
    document.getElementById('versionsDialog').showModal();
  }
  const handleCreateEstimatesClicked = async () => {
    const selectors = 'input[type=radio][name=proposalRadio]:checked';
    const selectedProposal = document.querySelector(selectors);
    if (!selectedProposal) {
      dispatch(setError({ errorMessages: ['見積作成するファイルを選択してください。'] }));
      return;
    }

    try {
      const proposalId = selectedProposal.value;
      const proposal = await fetchRefer(ankenId, proposalId, platformToken);
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
  const handleProposalClicked = elementId => e => {
    const selectedProposal = document.getElementById(elementId);
    selectedProposal.checked = true;
    const proposalId = selectedProposal.value;
    const subject = selectedProposal.dataset.title;
    dispatch(updateProposal({
      proposalId,
      subject
    }));
  }
  const pages = ankenList.pages;
  const isReading = pages === null;
  const isNoPages = isReading || pages.length === 0;
  const ankenVersionsHeader = (() => {
    if (isReading) {
      return (<>読込中．．．</>);
    }
    if (pages.length === 0) {
      return (
        <button type="button" className="btn edit-btn confirm-btn" onClick={handleShowDialogClicked}>新規作成</button>
      );
    }
    return (
      <button type="button" className="btn edit-btn confirm-btn" onClick={handleCreateEstimatesClicked}>見積作成</button>
    );
  })();
  const ankenVersionsDetail = (() => {
    if (isNoPages) {
      return (<></>);
    }

    const list = ankenList.pages[ankenList.pageNo];
    return ([...list].map((proposal, index) => {
      const elementId = `proposalId${proposal.id}`;
      const subject = `${proposal.subject}`;
      return (
        <div className="detail" key={index} onClick={handleProposalClicked(elementId)}>
          <div className="radio">
            <input type="radio" name="proposalRadio" id={elementId} value={proposal.id} data-title={subject}
              checked={`${proposalId}` === `${proposal.id}`} onChange={handleProposalClicked(elementId)} />
            <label style={{ width: 'auto' }}></label>
            <span>&nbsp;{`件名：${subject}`}</span>
          </div>
          <div className="updatedAt">{proposal.updatedAt}</div>
        </div>
      );
    }));
  })();

  return (
    <div className={className}>
      <div className="header">
        {ankenVersionsHeader}
      </div>
      <div className={"anken-versions" + (isNoPages ? " anken-create-new" : "")}>
        <div className="anken-versions-inner">
          {ankenVersionsDetail}
        </div>
      </div>
      <VersionsDialog />
    </div>
  );
}

export default Versions;
