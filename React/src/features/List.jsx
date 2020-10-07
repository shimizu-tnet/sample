import React, { useEffect, useRef, useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { fetchList } from '../api/platformSystem';
import Versions from './PlatformSystem/Versions';
import ErrorDialog from './Errors/ErrorDialog';
import { setApiFailure, setError, hasResponseError } from './Errors/ErrorSlice';
import { updateAnken } from './PlatformSystem/PlatformSystemSlice';
import { updatePageNo } from './PlatformSystem/AnkenListSlice';
import { isNullOrEmpty } from '../helpers/stringHelper';
const pageType = { previous: -1, next: 1 }

const List = ({ location }) => {
  const dispatch = useDispatch();
  const platformSystem = useSelector(state => state.platformSystem);
  const platformToken = platformSystem.platformToken;
  const proposalId = useRef(platformSystem.proposalId);
  const ankenId = useRef(platformSystem.ankenId);
  const ankenList = useSelector(state => state.ankenList);

  const handleAnkenIdChanged = e => ankenId.current = e.target.value;
  const handleGetListClicked = () => getAnkenList(true);
  const handlePreviousPageClicked = () => dispatch(updatePageNo({ pageNo: ankenList.pageNo + pageType.previous }));
  const handleNextPageClicked = () => dispatch(updatePageNo({ pageNo: ankenList.pageNo + pageType.next }));
  const getAnkenList = useCallback(async (doInit) => {
    if (isNullOrEmpty(ankenId.current)) {
      dispatch(setError({ errorMessages: ['案件Noを入力してください。'] }));
      return;
    }
    try {
      let ankenName = '';
      let loginAdminName = '';
      let pages = null;
      for (let pageNo = 0; pageNo < 100; pageNo++) {
        const anken = await fetchList(ankenId.current, pageNo, platformToken);

        if (pageNo === 0) {
          ankenName = anken.ankenName;
          loginAdminName = anken.loginAdminName;
        }

        if (pages === null) {
          pages = [];
        }

        const list = [...anken.list];
        if (list.length === 0) {
          break;
        }
        pages.push(list);
      }

      dispatch(updateAnken({
        ankenId: ankenId.current,
        ankenName,
        loginAdminName,
        pages,
        proposalId: proposalId.current,
        doInit
      }));
    } catch (err) {
      console.error(err);
      if (hasResponseError(err)) {
        dispatch(setApiFailure({ response: err.response }));
      } else {
        dispatch(setError({ errorMessages: ['処理でエラーが発生したため、再度実行してください。'] }));
      }
    }
  }, [dispatch, platformToken]);

  const referrer = useRef(`${location.state && location.state.referrer}`);
  useEffect(() => {
    const doInit = referrer.current === '/login';
    getAnkenList(doInit);
  }, [getAnkenList]);

  return (
    <div className="anken-list">
      <div className="anken-serach-wrap">
        <label htmlFor="anken_id">案件No</label>
        <input type="text" id="anken_id" defaultValue={ankenId.current} onChange={handleAnkenIdChanged} />
        <button type="button" id="get_anken_list" className="btn edit-btn" onClick={handleGetListClicked}>一覧取得</button>
      </div>
      <div className="anken-title-wrap">
        <div className="anken-title">履歴ファイル一覧</div>
        <div className="anken-label1">案件No：</div>
        <div className="anken-name1">{platformSystem.ankenId}</div>
        <div className="anken-label2">案件名：</div>
        <div className="anken-name2">{platformSystem.ankenName}</div>
      </div>
      <Versions className="anken-versions-wrap" />
      <div className="anken-paging-wrap">
        <div className="anken-paging">
          {ankenList.existsPreviousPage && <button className="btn" id="footer-R" onClick={handlePreviousPageClicked}>前のページへ</button>}
          {ankenList.existsNextPage && <button className="btn" id="footer-L" onClick={handleNextPageClicked}>次のページへ</button>}
        </div>
      </div>
      <ErrorDialog />
    </div>
  );
}

export default List;
