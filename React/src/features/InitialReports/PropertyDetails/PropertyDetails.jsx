import React from 'react';
import { useSelector } from 'react-redux';
import { useHandleInput } from '../../../helpers/useHandleInput';
import {
  updateSchedule,
  changeSeparator,
} from '../HelpSchedule/SchedulesSlice';
import { itemTypes } from '../../../consts/itemType';
import PropertyHeader from './PropertyHeader';
import PropertyList from './PropertyList';
import { landPropertyID, housePropertyID } from './PropertyDetailsSlice';

const headerValues = {
  property: ["表示", "財産名称", "評価額", "控除額", "相続額",],
  debt: ["表示", "債務名称", "評価額", "", "相続額",],
};
const PropertyDetails = () => {
  const { properties, debts } = useSelector(state => state.propertyDetails);
  const schedules = useSelector(state => state.schedules);
  const inheritanceStartDate = changeSeparator(schedules.inheritanceStartDate, '/', '-');
  const inputScheduleChanged = useHandleInput(updateSchedule, 'schedules', schedules).inputValueChanged;
  const landName = [...properties]
    .find(v => v.propertyID === landPropertyID)
    .propertyName;
  const houseName = [...properties]
    .find(v => v.propertyID === housePropertyID)
    .propertyName;

  return (
    <>
      <form id="property">
        <label>相続開始</label>
        <input type="date" mix="1-1-1" max="9999-12-31"
          value={inheritanceStartDate} onChange={inputScheduleChanged('inheritanceStartDate')} />
        <div className="property-table-wrap">
          <PropertyHeader headerValues={headerValues.property} />
          <PropertyList itemType={itemTypes.PROPERTY} details={properties} />
        </div>
        <div>
          {`※${landName}は、土地・借地権の評価明細一覧表の入力に応じて上書きされます。`}
        </div>
        <div>
          {`※${houseName}は、家屋の評価明細一覧表の入力に応じて上書きされます。`}
        </div>
        <div className="property-table-wrap">
          <PropertyHeader headerValues={headerValues.debt} />
          <PropertyList itemType={itemTypes.DEBT} details={debts} />
        </div>
      </form>
    </>
  );
}

export default PropertyDetails;