import React from 'react';
import { toCurrencyValue } from '../helpers/stringHelper';
import { isNumber, isNegative } from '../helpers/numberHelper';

const CurrencyParagraph = ({ value, showUnit }) => {
  if (!isNumber(value)) {
    return (<p></p>);
  }

  const converted = (+value);
  const style = {
    color: (isNegative(converted) ? '#ff0000' : '#000000')
  };
  return (
    <p style={style}>
      {toCurrencyValue(converted)}
      {showUnit && <span> 円</span>}
    </p>
  );
}

export default CurrencyParagraph;
