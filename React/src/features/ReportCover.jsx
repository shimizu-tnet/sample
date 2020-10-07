import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHandleInput } from '../helpers/useHandleInput';
import {
  updateSchedule,
  changeSeparator,
} from './InitialReports/HelpSchedule/SchedulesSlice';
import { updateReative, } from './InitialReports/InheritanceDiagram/InheritanceDiagramSlice';
import {
  updateChiefTaxAccountantType,
  updateChiefTaxAccountantName,
  updateTaxAccountant,
  chiefTaxAccountantTypeNameOptions,
  chiefTaxAccountantNameOptions,
  taxAccountantNameOptions,
} from './TaxAccountantSlice';
import { updateNavigation } from './NavigationSlice';
import { Relationship } from './InitialReports/InheritanceDiagram/Relationship';

const ReportCover = () => {
  const schedules = useSelector(state => state.schedules);
  const proposalDate = changeSeparator(schedules.proposalDate, '/', '-');
  const {
    chiefTaxAccountantType,
    chiefTaxAccountantName,
    taxAccountantNames
  } = useSelector(state => state.taxAccountant);
  const { relatives } = useSelector(state => state.inheritanceDiagram);
  const deceased = [...relatives].find(x => x.relationship === Relationship.Deceased);
  const inputValueChangedReative = useHandleInput(updateReative, 'relative', deceased).inputValueChanged;
  const navigation = useSelector(state => state.navigation);
  const inputCheckedChangedNavigation = useHandleInput(updateNavigation, 'navigation', navigation).inputCheckedChanged;
  const inputScheduleChanged = useHandleInput(updateSchedule, 'schedules', schedules).inputValueChanged;

  const dispatch = useDispatch();
  const handleChiefTaxAccountantTypeChanged = e => {
    dispatch(updateChiefTaxAccountantType({ chiefTaxAccountantType: e.target.value }));
  }
  const handleChiefTaxAccountantNameChanged = e => {
    dispatch(updateChiefTaxAccountantName({ chiefTaxAccountantName: e.target.value }));
  }
  const handleTaxAccountantChanged = index => e => {
    dispatch(updateTaxAccountant({ index, name: e.target.value }));
  }

  return (
    <form>
      <label>ご提案日</label>
      <div>
        <input type="date" mix="1-1-1" max="9999-12-31"
          value={proposalDate} onChange={inputScheduleChanged('proposalDate')} />
      </div>
      <label>被相続人氏名</label>
      <div>
        <span></span>
        <input type="text" value={deceased.lastName} onChange={inputValueChangedReative('lastName')} size={5} />
        <input type="text" value={deceased.firstName} onChange={inputValueChangedReative('firstName')} size={5} />
      </div>
      <label>担当社員税理士</label>
      <div>
        <select value={chiefTaxAccountantType} onChange={handleChiefTaxAccountantTypeChanged}>
          {chiefTaxAccountantTypeNameOptions.map((name, index) => {
            return (
              <option key={index} value={index}>{name}</option>
            );
          })}
        </select>
        <input type="text" value={chiefTaxAccountantName} onChange={handleChiefTaxAccountantNameChanged} list="chiefTaxAccountantNameOptions" />
        <datalist id="chiefTaxAccountantNameOptions">
          {chiefTaxAccountantNameOptions.map((name, index) => {
            return (
              <option key={index} value={name}>{name}</option>
            );
          })}
        </datalist>
      </div>
      <label className="input_items">担当者1</label>
      <div>
        <input type="text" value={taxAccountantNames[0]} onChange={handleTaxAccountantChanged(0)} list="taxAccountantNameOptions0" />
        <datalist id="taxAccountantNameOptions0">
          {taxAccountantNameOptions.map((name, index) => {
            return (
              <option key={index} value={name}>{name}</option>
            );
          })}
        </datalist>
      </div>
      <label className="input_items">担当者２</label>
      <div>
        <input type="text" value={taxAccountantNames[1]} onChange={handleTaxAccountantChanged(1)} list="taxAccountantNameOptions1" />
        <datalist id="taxAccountantNameOptions1">
          {taxAccountantNameOptions.map((name, index) => {
            return (
              <option key={index} value={name}>{name}</option>
            );
          })}
        </datalist>
      </div>
      <label className="input_items">担当者３</label>
      <div>
        <input type="text" value={taxAccountantNames[2]} onChange={handleTaxAccountantChanged(2)} list="taxAccountantNameOptions2" />
        <datalist id="taxAccountantNameOptions2">
          {taxAccountantNameOptions.map((name, index) => {
            return (
              <option key={index} value={name}>{name}</option>
            );
          })}
        </datalist>
      </div>
      <input type="checkbox" id="showPageIndex"
        checked={navigation.showPageIndex} onChange={inputCheckedChangedNavigation('showPageIndex')} />
      <label htmlFor="showPageIndex" className="input_items">ページ番号を表示</label>
    </form>
  )
}
export default ReportCover;