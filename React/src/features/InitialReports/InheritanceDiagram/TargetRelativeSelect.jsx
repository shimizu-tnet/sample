import React from 'react';
const TargetRelativeSelect = ({ relative, targetRelatives, handleTargetRelativeValueChanged }) => {
  if (!targetRelatives.length) {
    return (<></>);
  }

  const defaultTargetRelativeKey = -1;
  const defaultTargetRelativeValue = null;
  return (
    <select value={relative.targetRelativeID} onChange={handleTargetRelativeValueChanged}>
      <option key={defaultTargetRelativeKey} value={defaultTargetRelativeValue}></option>
      {targetRelatives.map(targetRelative => {
        return (
          <option
            key={targetRelative.relativeID}
            value={targetRelative.relativeID}>{`${targetRelative.lastName} ${targetRelative.firstName}`}
          </option>
        )
      })}
    </select>
  );
}

export default TargetRelativeSelect;