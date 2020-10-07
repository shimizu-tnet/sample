import { combineReducers } from '@reduxjs/toolkit';
import loginReducer from '../features/LoginSlice';
import errorReducer from '../features/Errors/ErrorSlice';
import navigationReducer from '../features/NavigationSlice';
import taxAccountantReducer from '../features/TaxAccountantSlice';
import schedulesReducer from '../features/InitialReports/HelpSchedule/SchedulesSlice';
import inheritanceDiagramReducer from '../features/InitialReports/InheritanceDiagram/InheritanceDiagramSlice';
import PropertyDetailsReducer from '../features/InitialReports/PropertyDetails/PropertyDetailsSlice';
import landEvaluationsReducer from '../features/InitialReports/EvaluationDetails/LandEvaluationsSlice';
import houseEvaluationsReducer from '../features/InitialReports/EvaluationDetails/HouseEvaluationsSlice';
import estimatedInheritanceTaxReducer from '../features/InitialReports/EstimatedInheritanceTax/EstimatedInheritanceTaxSlice';
import propertyEvaluationReducer from '../features/InitialReports/PropertyEvaluation/PropertyEvaluationSlice';
import taxPaymentReducer from '../features/InitialReports/TaxPayment/TaxPaymentSlice';
import heritageDivisionDiscussionReducer from '../features/InitialReports/HeritageDivisionDiscussion/HeritageDivisionDiscussionSlice';
import taxInvestigationMeasuresReducer from '../features/InitialReports/TaxInvestigationMeasures/TaxInvestigationMeasuresSlice';
import feeReducer from '../features/EstimatesReports/Fee/FeeSlice';
import businessDescriptionReducer from '../features/EstimatesReports/BusinessDescription/BusinessDescriptionSlice';
import supplementaryMaterialReducer from '../features/EstimatesReports/SupplementaryMaterial/SupplementaryMaterialSlice';
import InheritanceRegistrationReducer from '../features/EstimatesReports/InheritanceRegistration/InheritanceRegistrationSlice';
import PlatformSystemReducer from '../features/PlatformSystem/PlatformSystemSlice';
import AnkenListReducer from '../features/PlatformSystem/AnkenListSlice';

const rootReducer = combineReducers({
    login: loginReducer,
    error: errorReducer,
    navigation: navigationReducer,
    taxAccountant: taxAccountantReducer,
    schedules: schedulesReducer,
    inheritanceDiagram: inheritanceDiagramReducer,
    propertyDetails: PropertyDetailsReducer,
    landEvaluations: landEvaluationsReducer,
    houseEvaluations: houseEvaluationsReducer,
    estimatedInheritanceTax: estimatedInheritanceTaxReducer,
    propertyEvaluation: propertyEvaluationReducer,
    taxPayment: taxPaymentReducer,
    heritageDivisionDiscussion: heritageDivisionDiscussionReducer,
    taxInvestigationMeasures: taxInvestigationMeasuresReducer,
    fee: feeReducer,
    businessDescription: businessDescriptionReducer,
    supplementaryMaterial: supplementaryMaterialReducer,
    inheritanceRegistration: InheritanceRegistrationReducer,
    platformSystem: PlatformSystemReducer,
    ankenList: AnkenListReducer,
});

export default rootReducer;
