import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  updateSchedule,
  fetchInheritanceSchedule,
  fetchProposalSchedule,
  changeSeparator,
} from './SchedulesSlice';
import { updateNavigation } from '../../NavigationSlice';
import { setError } from '../../Errors/ErrorSlice';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { isDate } from '../../../helpers/stringHelper';


const HelpSchedule = () => {
  const schedules = useSelector(state => state.schedules);
  const navigation = useSelector(state => state.navigation);

  const inputScheduleChanged = useHandleInput(updateSchedule, 'schedules', schedules).inputValueChanged;
  const { inputCheckedChanged } = useHandleInput(updateNavigation, 'navigation', navigation);
  const dispatch = useDispatch();
  const { accessToken } = useSelector(state => state.login);
  const handleCalculateInheritanceSchedule = () => {
    if (!isDate(schedules.inheritanceStartDate)) {
      dispatch(setError({ errorMessages: ['相続開始に日付を入力してください。'] }));
      return;
    }
    dispatch(fetchInheritanceSchedule(accessToken, schedules))
  };
  const handleCalculateProposalSchedule = () => {
    if (!isDate(schedules.proposalDate)) {
      dispatch(setError({ errorMessages: ['ご提案・ご契約に日付を入力してください。'] }));
      return;
    }
    dispatch(fetchProposalSchedule(accessToken, schedules))
  };

  const inheritanceStartDate = changeSeparator(schedules.inheritanceStartDate, '/', '-');
  const proposalDate = changeSeparator(schedules.proposalDate, '/', '-');
  const semiFinalTaxReturnDate = changeSeparator(schedules.semiFinalTaxReturnDate, '/', '-');
  const interimReportDate = changeSeparator(schedules.interimReportDate, '/', '-');
  const paymentDate = changeSeparator(schedules.paymentDate, '/', '-');
  const finalTaxReturnDate = changeSeparator(schedules.finalTaxReturnDate, '/', '-');

  return (
    <>
      <table>
        <tbody>
          <tr>
            <th style={{ textAlign: "left" }}>相続開始</th>
            <td>
              <input type="date" mix="1-1-1" max="9999-12-31"
                value={inheritanceStartDate} onChange={inputScheduleChanged('inheritanceStartDate')} />
            </td>
            <td>
              <button type="button" className="btn edit-btn"
                onClick={handleCalculateInheritanceSchedule}
                title="準確定申告、相続税の申告の日付を算出します。">日付を算出</button>
            </td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>ご提案・ご契約</th>
            <td>
              <input type="date" mix="1-1-1" max="9999-12-31"
                value={proposalDate} onChange={inputScheduleChanged('proposalDate')} />
            </td>
            <td>
              <button type="button" className="btn edit-btn"
                onClick={handleCalculateProposalSchedule}
                title="中間報告、相続税申告と納付の日付を算出します。">日付を算出</button>
            </td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>不動産コンサルティング</th>
            <td>
              <input id="showInheritanceRealEstateConsulting" type="checkbox"
                checked={navigation.showInheritanceRealEstateConsulting}
                onChange={inputCheckedChanged('showInheritanceRealEstateConsulting')} />
              <label htmlFor="showInheritanceRealEstateConsulting">表示する</label>
            </td>
            <td></td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>準確定申告</th>
            <td>
              <input type="date" mix="1-1-1" max="9999-12-31"
                value={semiFinalTaxReturnDate} onChange={inputScheduleChanged('semiFinalTaxReturnDate')} />
            </td>
            <td></td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>中間報告</th>
            <td>
              <input type="date" mix="1-1-1" max="9999-12-31"
                value={interimReportDate} onChange={inputScheduleChanged('interimReportDate')} />
            </td>
            <td></td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>税務調査対策</th>
            <td colSpan="2">
              <input id="showTaxInvestigationMeasures" type="checkbox"
                checked={navigation.showTaxInvestigationMeasures}
                onChange={inputCheckedChanged('showTaxInvestigationMeasures')} />
              <label style={{ width: 'auto' }} htmlFor="showTaxInvestigationMeasures">表示する（簡易版を選択時は表示されません）</label>
            </td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>遺産分割協議案の作成</th>
            <td>
              <input id="showHeritageDivisionDiscussion" type="checkbox"
                checked={navigation.showHeritageDivisionDiscussion}
                onChange={inputCheckedChanged('showHeritageDivisionDiscussion')} />
              <label htmlFor="showHeritageDivisionDiscussion">表示する</label>
            </td>
            <td></td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>納税の仕方のご相談</th>
            <td>
              <input id="showTaxPayment" type="checkbox"
                checked={navigation.showTaxPayment}
                onChange={inputCheckedChanged('showTaxPayment')} />
              <label htmlFor="showTaxPayment">表示する</label>
            </td>
            <td></td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>相続税申告と納付</th>
            <td>
              <input type="date" mix="1-1-1" max="9999-12-31"
                value={paymentDate} onChange={inputScheduleChanged('paymentDate')} />
            </td>
            <td></td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>相続税の申告</th>
            <td>
              <input type="date" mix="1-1-1" max="9999-12-31"
                value={finalTaxReturnDate} onChange={inputScheduleChanged('finalTaxReturnDate')} />
            </td>
            <td></td>
          </tr>
          <tr>
            <th style={{ textAlign: "left" }}>相続登記その他</th>
            <td>
              <input id="showInheritanceRegistration" type="checkbox"
                checked={navigation.showInheritanceRegistration}
                onChange={inputCheckedChanged('showInheritanceRegistration')} />
              <label htmlFor="showInheritanceRegistration">表示する</label>
            </td>
            <td></td>
          </tr>
        </tbody>
      </table>
    </>
  )
}

export default HelpSchedule;