import { createSlice } from '@reduxjs/toolkit';
import { createSelector } from 'reselect'
import { updatePdfParam } from '../../UpdatePdfParamAction';
import {
  deleteDetail as landDeleteDetail,
  updateDetail as landUpdateDetail,
} from '../EvaluationDetails/LandEvaluationsSlice';
import {
  deleteDetail as houseDeleteDetail,
  updateDetail as houseUpdateDetail,
} from '../EvaluationDetails/HouseEvaluationsSlice';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';
import { toCurrencyValue } from '../../../helpers/stringHelper';
import { isNumber, calculatePropertyAmount } from '../../../helpers/numberHelper';

export const landPropertyID = 0;
export const housePropertyID = 1;
export const savingsAmountPropertyID = 4;
export const insuranceAmountPropertyID = 5;
const propertyNames = ["土地", "家屋", "有価証券", "自社株式", "現預金", "保険金等", "退職手当金", "その他財産", "名義預金", "未調査財産"];
const debtNames = ["銀行借入金", "預かり敷金・保証金", "債務", "葬式費用"];
const initialState = {
  properties: [
    { propertyID: landPropertyID, isShow: true, propertyName: propertyNames[0], propertyTitle: propertyNames[0], evaluationAmount: "", isDeductionTarget: true, deductionAmount: "", deductionTitle: "小規模宅地の特例", inheritanceAmount: "", inheritanceExplanation: "路線価等による概算相続税評価額", currentExplanation: "路線価等による概算相続税評価額", inheritanceExplanationInit: "路線価等による概算相続税評価額", currentExplanationInit: "路線価等による概算相続税評価額", deductionInheritanceExplanation: "特例の要件を満たしているものとして適用しています", deductionCurrentExplanation: "適用していません", deductionInheritanceExplanationInit: "特例の用件を満たしているものとして適用しています", deductionCurrentExplanationInit: "適用していません" },
    { propertyID: housePropertyID, isShow: true, propertyName: propertyNames[1], propertyTitle: propertyNames[1], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "固定資産税評価額による概算相続税評価額", currentExplanation: "固定資産税評価額による概算相続税評価額", inheritanceExplanationInit: "固定資産税評価額による概算相続税評価額", currentExplanationInit: "固定資産税評価額による概算相続税評価額" },
    { propertyID: 2, isShow: true, propertyName: propertyNames[2], propertyTitle: propertyNames[2], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額" },
    { propertyID: 3, isShow: true, propertyName: propertyNames[3], propertyTitle: propertyNames[3], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額" },
    { propertyID: savingsAmountPropertyID, isShow: true, propertyName: propertyNames[4], propertyTitle: propertyNames[4], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額" },
    { propertyID: insuranceAmountPropertyID, isShow: true, propertyName: propertyNames[5], propertyTitle: propertyNames[5], evaluationAmount: "", isDeductionTarget: true, deductionAmount: "", deductionTitle: "＜非課税金額＞", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "非課税限度500万円 × 法定相続人の数", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "非課税限度500万円 × 法定相続人の数", currentExplanationInit: "ヒアリングによる概算額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
    { propertyID: 6, isShow: true, propertyName: propertyNames[6], propertyTitle: propertyNames[6], evaluationAmount: "", isDeductionTarget: true, deductionAmount: "", deductionTitle: "＜非課税金額＞", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "非課税限度500万円 × 法定相続人の数", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "非課税限度500万円 × 法定相続人の数", currentExplanationInit: "ヒアリングによる概算額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
    { propertyID: 7, isShow: true, propertyName: propertyNames[7], propertyTitle: propertyNames[7], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額" },
    { propertyID: 8, isShow: true, propertyName: propertyNames[8], propertyTitle: propertyNames[8], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
    { propertyID: 9, isShow: true, propertyName: propertyNames[9], propertyTitle: propertyNames[9], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "詳細をお伺いしていない財産の仮計上額", currentExplanation: "詳細をお伺いしていない財産の仮計上額", inheritanceExplanationInit: "詳細をお伺いしていない財産の仮計上額", currentExplanationInit: "詳細をお伺いしていない財産の仮計上額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
  ],
  debts: [
    { propertyID: 0, isShow: true, propertyName: debtNames[0], propertyTitle: debtNames[0], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
    { propertyID: 1, isShow: true, propertyName: debtNames[1], propertyTitle: debtNames[1], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
    { propertyID: 2, isShow: true, propertyName: debtNames[2], propertyTitle: debtNames[2], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
    { propertyID: 3, isShow: true, propertyName: debtNames[3], propertyTitle: debtNames[3], evaluationAmount: "", isDeductionTarget: false, deductionAmount: "", deductionTitle: "", inheritanceAmount: "", showRationale: true, canDelete: false, inheritanceExplanation: "ヒアリングによる概算額", currentExplanation: "ヒアリングによる概算額", inheritanceExplanationInit: "ヒアリングによる概算額", currentExplanationInit: "ヒアリングによる概算額", deductionInheritanceExplanation: "", deductionCurrentExplanation: "", deductionInheritanceExplanationInit: "", deductionCurrentExplanationInit: "" },
  ],
}

const calculateInheritanceAmount = (evaluationAmount, deductionAmount) => {
  if (!isNumber(evaluationAmount))
    return null;

  if (!isNumber(deductionAmount))
    return +evaluationAmount;

  return (evaluationAmount - deductionAmount);
}

const displayedInheritanceAmount = (evaluationAmount, deductionAmount) => {
  const result = calculateInheritanceAmount(evaluationAmount, deductionAmount)
  return (result == null) ? '' : result.toLocaleString('ja');
}

const PropertyDetailsSlice = createSlice({
  name: 'propertyDetails',
  initialState,
  reducers: {
    updateProperty(state, action) {
      const detail = action.payload.detail;
      const { propertyID, evaluationAmount, deductionAmount } = detail;
      const index = state.properties.findIndex(x => x.propertyID === propertyID);
      state.properties[index] = {
        ...detail,
        inheritanceAmount: displayedInheritanceAmount(evaluationAmount, deductionAmount),
      };
    },
    updateDebt(state, action) {
      const detail = action.payload.detail;
      const { propertyID, evaluationAmount, deductionAmount } = detail;
      const index = state.debts.findIndex(x => x.propertyID === propertyID);
      state.debts[index] = {
        ...detail,
        inheritanceAmount: displayedInheritanceAmount(evaluationAmount, deductionAmount),
      };
    },
    setProperty(state, action) {
      state.properties = action.payload.replacedDetails;
    },
    setDebt(state, action) {
      state.debts = action.payload.replacedDetails;
    },
    initializeRationales(state) {
      state.properties.forEach((v, i) => {
        state.properties[i].inheritanceExplanation = state.properties[i].inheritanceExplanationInit;
        state.properties[i].currentExplanation = state.properties[i].currentExplanationInit;
        state.properties[i].deductionInheritanceExplanation = state.properties[i].deductionInheritanceExplanationInit;
        state.properties[i].deductionCurrentExplanation = state.properties[i].deductionCurrentExplanationInit;
      });
      state.debts.forEach((v, i) => {
        state.debts[i].inheritanceExplanation = state.debts[i].inheritanceExplanationInit;
        state.debts[i].currentExplanation = state.debts[i].currentExplanationInit;
        state.debts[i].deductionInheritanceExplanation = state.debts[i].deductionInheritanceExplanationInit;
        state.debts[i].deductionCurrentExplanation = state.debts[i].deductionCurrentExplanationInit;
      });
    },
  },
  extraReducers: {
    [updatePdfParam]: (state, action) => {
      const { propertyDetails } = action.payload;
      state.properties = propertyDetails.properties;
      state.debts = propertyDetails.debts;
    },
    [landDeleteDetail]: (state, action) => {
      const { totalEvaluation, totalDevaluation, taxableAmount } = action.payload;
      const index = state.properties.findIndex(v => v.propertyID === landPropertyID);
      state.properties[index].evaluationAmount = totalEvaluation;
      state.properties[index].deductionAmount = -totalDevaluation;
      state.properties[index].inheritanceAmount = toCurrencyValue(taxableAmount);
    },
    [landUpdateDetail]: (state, action) => {
      const { totalEvaluation, totalDevaluation, taxableAmount } = action.payload;
      const index = state.properties.findIndex(v => v.propertyID === landPropertyID);
      state.properties[index].evaluationAmount = totalEvaluation;
      state.properties[index].deductionAmount = -totalDevaluation;
      state.properties[index].inheritanceAmount = toCurrencyValue(taxableAmount);
    },
    [houseDeleteDetail]: (state, action) => {
      const { totalEvaluation } = action.payload;
      const index = state.properties.findIndex(v => v.propertyID === housePropertyID);
      state.properties[index].evaluationAmount = totalEvaluation;
      state.properties[index].deductionAmount = '';
      state.properties[index].inheritanceAmount = toCurrencyValue(totalEvaluation);
    },
    [houseUpdateDetail]: (state, action) => {
      const { totalEvaluation } = action.payload;
      const index = state.properties.findIndex(v => v.propertyID === housePropertyID);
      state.properties[index].evaluationAmount = totalEvaluation;
      state.properties[index].deductionAmount = '';
      state.properties[index].inheritanceAmount = toCurrencyValue(totalEvaluation);
    },
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  updateProperty,
  updateDebt,
  setProperty,
  setDebt,
  initializeRationales,
} = PropertyDetailsSlice.actions;

export default PropertyDetailsSlice.reducer;

const sumInheritanceAmount = propertyDetails => {
  const totalPropertiesAmount = propertyDetails.properties
    .filter(item => item.isShow)
    .filter(item => isNumber(item.evaluationAmount))
    .map(item => calculateInheritanceAmount(
      calculatePropertyAmount(item.evaluationAmount),
      calculatePropertyAmount(item.deductionAmount)
    ))
    .map(item => item)
    .reduce((total, current) => total + current, 0);
  const totalDebtsAmount = propertyDetails.debts
    .filter(item => item.isShow)
    .filter(item => isNumber(item.evaluationAmount))
    .map(item => Number(item.evaluationAmount))
    .map(item => calculatePropertyAmount(item))
    .reduce((total, current) => total + current, 0);
  return totalPropertiesAmount - totalDebtsAmount;
}

export const calculateInheritanceTotalAmount = createSelector(
  state => state.propertyDetails,
  propertyDetails => sumInheritanceAmount(propertyDetails)
);

const sumPropertyAmount = propertyDetails => {
  const totalPropertiesAmount = propertyDetails.properties
    .filter(item => item.isShow)
    .filter(item => isNumber(item.evaluationAmount))
    .map(item => Number(item.evaluationAmount))
    .map(item => calculatePropertyAmount(item))
    .reduce((total, current) => total + current, 0);
  return totalPropertiesAmount;
}

export const calculatePropertyTotalAmount = createSelector(
  state => state.propertyDetails,
  propertyDetails => sumPropertyAmount(propertyDetails)
);
