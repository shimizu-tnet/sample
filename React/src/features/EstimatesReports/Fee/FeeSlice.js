import { createSlice } from '@reduxjs/toolkit';
import { createSelector } from 'reselect'
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

const defaultValue = { optionName: '', moneyAmount: null };
const initialState = {
  basicFee: null,
  heirAdditionalFee: null,
  landAdditionalUnit: '',
  landAdditionalFee: 0,
  adjustmentAmount: null,
  adjustmentAmountComment: '',
  options: [
    { index: 0, ...defaultValue },
    { index: 1, ...defaultValue },
    { index: 2, ...defaultValue },
    { index: 3, ...defaultValue },
    { index: 4, ...defaultValue },
    { index: 5, ...defaultValue },
    { index: 6, ...defaultValue },
    { index: 7, ...defaultValue },
    { index: 8, ...defaultValue },
    { index: 9, ...defaultValue },
  ],
};

const FeeSlice = createSlice({
  name: 'fee',
  initialState,
  reducers: {
    updateFee(state, action) {
      const { fee } = action.payload;
      state.basicFee = fee.basicFee;
      state.heirAdditionalFee = fee.heirAdditionalFee;
      state.adjustmentAmount = fee.adjustmentAmount;
      state.adjustmentAmountComment = fee.adjustmentAmountComment;
    },
    updateBasicFee(state, action) {
      const { fee } = action.payload;
      state.basicFee = fee.basicFee;
    },
    updateHeirAdditionalFee(state, action) {
      const { fee } = action.payload;
      state.heirAdditionalFee = fee.heirAdditionalFee;
    },
    updateLandAdditionalFee(state, action) {
      const { number } = action.payload;
      state.landAdditionalUnit = number;
      const landAdditionalUnit = Number(number);
      if (Number.isNaN(landAdditionalUnit) || landAdditionalUnit === 0) {
        state.landAdditionalFee = 0;
      } else {
        state.landAdditionalFee = landAdditionalUnit * 50000 - 10000;
      }
    },
    updateOption(state, action) {
      const { option } = action.payload;
      state.options[option.index] = option;
    },
  },
  extraReducers: {
    [updatePdfParam]: (state, action) => {
      const { fee } = action.payload;
      state.basicFee = fee.basicFee;
      state.heirAdditionalFee = fee.heirAdditionalFee;
      state.landAdditionalUnit = fee.landAdditionalUnit;
      state.landAdditionalFee = fee.landAdditionalFee;
      state.adjustmentAmount = fee.adjustmentAmount;
      state.adjustmentAmountComment = fee.adjustmentAmountComment;
      state.options.forEach(current => {
        const option = fee.options.find(item => item.index === current.index);
        if (!option) {
          current.optionName = defaultValue.optionName;
          current.moneyAmount = defaultValue.moneyAmount;
          return;
        }

        current.optionName = option.optionName;
        current.moneyAmount = option.moneyAmount;
      });
    },
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  updateFee,
  updateBasicFee,
  updateHeirAdditionalFee,
  updateLandAdditionalFee,
  updateOption
} = FeeSlice.actions;

export default FeeSlice.reducer;

const sumOptionMoneyAmount = options => options
  // 数字以外を含んだ値に+を接頭するとNaNに変換する
  .map(item => +item.moneyAmount)
  .reduce((total, current) => total + current, 0);

export const calculateTotalOptionFee = createSelector(
  state => state.fee.options,
  options => sumOptionMoneyAmount(options)
);

const sumTotalFee = fee => {
  if (Number.isNaN(+fee.basicFee)
    || Number.isNaN(+fee.heirAdditionalFee)
    || Number.isNaN(+fee.landAdditionalFee)
    || Number.isNaN(+sumOptionMoneyAmount(fee.options))) {
    return Number.NaN;
  }
  const basicFee = +fee.basicFee || 0;
  const heirAdditionalFee = +fee.heirAdditionalFee || 0;
  const landAdditionalFee = +fee.landAdditionalFee || 0;
  const totalOptionFee = +sumOptionMoneyAmount(fee.options);
  return basicFee + heirAdditionalFee + landAdditionalFee + totalOptionFee;
}

export const calculateTotalFee = createSelector(
  state => state.fee,
  fee => sumTotalFee(fee)
);

const sumEstimatedAmount = fee => {
  const adjustmentAmount = fee.adjustmentAmount || 0;
  const totalFee = sumTotalFee(fee);
  return totalFee - adjustmentAmount;
}

export const calculateEstimatedAmount = createSelector(
  state => state.fee,
  fee => sumEstimatedAmount(fee)
);
