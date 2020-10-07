import React from 'react';
import { isNumber, isNegative } from '../helpers/numberHelper';

const NumberParagraph = ({ value }) => {
  if (!isNumber(value)) {
    return (<p></p>);
  }

  const converted = (+value);
  const style = {
    color: (isNegative(converted) ? '#ff0000' : '#000000')
  };
  return (
    <p style={style}>{value}</p>
  );
}

export default NumberParagraph;
