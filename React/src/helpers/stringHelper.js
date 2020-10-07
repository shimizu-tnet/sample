import { isNumber } from './numberHelper';

export const excludeString = value => {
  let replacedValue = String(value);

  while (replacedValue.match(/\D/))
    replacedValue = replacedValue.replace(/\D/, "");

  return replacedValue;
}

export const separateComma = num => {
  if (num == null) {
    return '';
  }

  return String(num).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
}

export const splitByLineFeed = text => {
  if (text == null) {
    return '';
  }

  return String(text).split('\n');
}

export const toCurrencyValue = value => {
  if (isNumber(value)) {
    return Number(value).toLocaleString();
  }

  return '';
};

export const toJPDateString = value => {
  const d = new Date(value);
  if (isNumber(d.getTime())) {
    return `${d.getFullYear()} 年 ${d.getMonth() + 1} 月 ${d.getDate()} 日`;
  }

  return '';
}

export const isDate = value => /^\d{4}\/\d{1,2}\/\d{1,2}$/.test(`${value}`);

export const getDisplayDeceasedName = deceasedName => {
  if (isNullOrEmpty(deceasedName)) {
    return "　　　　";
  }
  else {
    return deceasedName;
  }
}

export const isNullOrEmpty = value => {
  if (value === null) {
    return true;
  }
  if (String(value).trim() === "") {
    return true;
  }

  return false;
}
