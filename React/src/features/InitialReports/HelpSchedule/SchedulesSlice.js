import { createSlice } from '@reduxjs/toolkit';
import {
  calculateInheritanceSchedule,
  calculateProposalSchedule,
} from '../../../api/legacyDocument';
import { setApiFailure, setError, hasResponseError } from '../../Errors/ErrorSlice';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
  inheritanceStartDate: '',
  proposalDate: '',
  semiFinalTaxReturnDate: '',
  interimReportDate: '',
  paymentDate: '',
  finalTaxReturnDate: '',
};

const SchedulesSlice = createSlice({
  name: 'schedules',
  initialState,
  reducers: {
    updateSchedule(state, action) {
      const { schedules } = action.payload;
      state.inheritanceStartDate = changeSeparator(schedules.inheritanceStartDate, '-', '/');
      state.proposalDate = changeSeparator(schedules.proposalDate, '-', '/');
      state.semiFinalTaxReturnDate = changeSeparator(schedules.semiFinalTaxReturnDate, '-', '/');
      state.interimReportDate = changeSeparator(schedules.interimReportDate, '-', '/');
      state.paymentDate = changeSeparator(schedules.paymentDate, '-', '/');
      state.finalTaxReturnDate = changeSeparator(schedules.finalTaxReturnDate, '-', '/');
    }
  },
  extraReducers: {
    [updatePdfParam]: (state, action) => {
      const { schedules } = action.payload;
      state.inheritanceStartDate = schedules.inheritanceStartDate;
      state.proposalDate = schedules.proposalDate;
      state.semiFinalTaxReturnDate = schedules.semiFinalTaxReturnDate;
      state.interimReportDate = schedules.interimReportDate;
      state.paymentDate = schedules.paymentDate;
      state.finalTaxReturnDate = schedules.finalTaxReturnDate;
    },
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  updateSchedule,
} = SchedulesSlice.actions;

export default SchedulesSlice.reducer;

export const fetchInheritanceSchedule = (accessToken, schedules) => {
  return async dispatch => {
    try {
      const inheritanceSchedule = await calculateInheritanceSchedule(accessToken, schedules);
      dispatch(updateSchedule({
        schedules: {
          ...schedules,
          inheritanceStartDate: inheritanceSchedule.inheritanceStartDate,
          semiFinalTaxReturnDate: inheritanceSchedule.semiFinalTaxReturnDate,
          finalTaxReturnDate: inheritanceSchedule.finalTaxReturnDate,
        }
      }));
    } catch (err) {
      console.error(err);
      if (hasResponseError(err)) {
        dispatch(setApiFailure({ response: err.response }));
      } else {
        dispatch(setError({ errorMessages: ['処理でエラーが発生したため、再度実行してください。'] }));
      }
    }
  }
}

export const fetchProposalSchedule = (accessToken, schedules) => {
  return async dispatch => {
    try {
      const proposalSchedule = await calculateProposalSchedule(accessToken, schedules);
      dispatch(updateSchedule({
        schedules: {
          ...schedules,
          proposalDate: proposalSchedule.proposalDate,
          interimReportDate: proposalSchedule.interimReportDate,
          paymentDate: proposalSchedule.paymentDate,
        }
      }));
    } catch (err) {
      console.error(err);
      if (hasResponseError(err)) {
        dispatch(setApiFailure({ response: err.response }));
      } else {
        dispatch(setError({ errorMessages: ['処理でエラーが発生したため、再度実行してください。'] }));
      }
    }
  }
}

export const changeSeparator = (value, oldSeparator, newSeparator) => {
  return `${value}`.split(`${oldSeparator}`).join(`${newSeparator}`);
}