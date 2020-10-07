import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from './UpdatePdfParamAction';
import { updateAccessToken } from './LoginSlice';
import { updateProposal } from './PlatformSystem/PlatformSystemSlice';

let initialState = {
  showInitialReportCover: false,
  showInheritanceDiagram: false,
  showPropertyDetails: false,
  showLandEvaluation: false,
  showHouseEvaluation: false,
  showEstimatedInheritanceTax: false,
  showPropertyEvaluation: false,
  showTaxPayment: false,
  showHeritageDivisionDiscussion: false,
  showTaxInvestigationMeasures: false,
  showInheritanceRealEstateConsulting: false,
  showHelpSchedule: false,
  showAppnedix: false,
  showEstimatesReportCover: false,
  showBusinessDescription: false,
  showFee: false,
  showInheritanceRegistration: false,
  showSupplementaryMaterial: false,
  showPageIndex: false,
};

const update = (state, action) => {
  const { navigation } = action.payload;
  for (const key of Object.keys(state))
    state[key] = navigation[key];
}

const NavigationSlice = createSlice({
  name: 'navigation',
  initialState,
  reducers: {
    updateNavigation: update,
    selectAllInitialReport(state) {
      state.showInitialReportCover = true;
      state.showInheritanceDiagram = true;
      state.showPropertyDetails = true;
      state.showLandEvaluation = true;
      state.showHouseEvaluation = true;
      state.showEstimatedInheritanceTax = true;
      state.showPropertyEvaluation = true;
      state.showTaxPayment = true;
      state.showHeritageDivisionDiscussion = true;
      state.showTaxInvestigationMeasures = true;
      state.showInheritanceRealEstateConsulting = true;
      state.showHelpSchedule = true;
      state.showAppnedix = true;
    },
    selectAllEstimatesReport(state) {
      state.showEstimatesReportCover = true;
      state.showBusinessDescription = true;
      state.showFee = true;
      state.showInheritanceRegistration = true;
      state.showSupplementaryMaterial = true;
    },
  },
  extraReducers: {
    [updatePdfParam]: update,
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  updateNavigation,
  selectAllInitialReport,
  selectAllEstimatesReport,
} = NavigationSlice.actions;

export default NavigationSlice.reducer;

export const isSelectedAny = (type, navigation) => {
  if (type === 'initialReport') {
    return navigation.showInitialReportCover
      || navigation.showInheritanceDiagram
      || navigation.showPropertyDetails
      || navigation.showLandEvaluation
      || navigation.showHouseEvaluation
      || navigation.showEstimatedInheritanceTax
      || navigation.showPropertyEvaluation
      || navigation.showTaxPayment
      || navigation.showHeritageDivisionDiscussion
      || navigation.showTaxInvestigationMeasures
      || navigation.showInheritanceRealEstateConsulting
      || navigation.showHelpSchedule
      || navigation.showAppnedix;
  } else {
    return navigation.showEstimatesReportCover
      || navigation.showBusinessDescription
      || navigation.showFee
      || navigation.showInheritanceRegistration
      || navigation.showSupplementaryMaterial;
  }
}
