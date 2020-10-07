import { createSelector } from 'reselect';

export const pdfParamaToJson = createSelector(
  state => state,
  state => JSON.stringify({
    navigation: state.navigation,
    taxAccountant: state.taxAccountant,
    inheritanceDiagram: state.inheritanceDiagram,
    propertyDetails: state.propertyDetails,
    landEvaluations: state.landEvaluations,
    houseEvaluations: state.houseEvaluations,
    estimatedInheritanceTax: state.estimatedInheritanceTax,
    propertyEvaluation: state.propertyEvaluation,
    taxPayment: state.taxPayment,
    heritageDivisionDiscussion: state.heritageDivisionDiscussion,
    taxInvestigationMeasures: state.taxInvestigationMeasures,
    schedules: state.schedules,
    businessDescription: state.businessDescription,
    fee: state.fee,
    inheritanceRegistration: state.inheritanceRegistration,
    supplementaryMaterial: state.supplementaryMaterial,
  })
);

export const pdfParamaToObject = createSelector(
  state => state,
  state => ({
    navigation: state.navigation,
    taxAccountant: state.taxAccountant,
    inheritanceDiagram: state.inheritanceDiagram,
    propertyDetails: state.propertyDetails,
    landEvaluations: state.landEvaluations,
    houseEvaluations: state.houseEvaluations,
    estimatedInheritanceTax: state.estimatedInheritanceTax,
    propertyEvaluation: state.propertyEvaluation,
    taxPayment: state.taxPayment,
    heritageDivisionDiscussion: state.heritageDivisionDiscussion,
    taxInvestigationMeasures: state.taxInvestigationMeasures,
    schedules: state.schedules,
    businessDescription: state.businessDescription,
    fee: state.fee,
    inheritanceRegistration: state.inheritanceRegistration,
    supplementaryMaterial: state.supplementaryMaterial,
  })
);