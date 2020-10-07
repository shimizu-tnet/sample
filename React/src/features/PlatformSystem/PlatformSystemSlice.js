import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  // プラットフォームシステム側から連携される値。
  platformToken: '',
  ankenId: '',
  ankenName: '',
  loginAdminName: '',
  subject: '',
  proposalId: null,
};

const PlatformSystemSlice = createSlice({
  name: 'platformSystem',
  initialState,
  reducers: {
    updatePlatformParameters(state, action) {
      state.platformToken = action.payload.platformToken;
      state.ankenId = action.payload.ankenId;
      state.ankenName = initialState.ankenName;
      state.loginAdminName = initialState.loginAdminName;
      state.subject = initialState.subject;
      state.proposalId = initialState.proposalId;
    },
    updateAnken(state, action) {
      state.ankenId = action.payload.ankenId;
      state.ankenName = action.payload.ankenName;
      state.loginAdminName = action.payload.loginAdminName;
      if (action.payload.doInit) {
        state.subject = initialState.subject;
        state.proposalId = initialState.proposalId;
      }
    },
    updateProposal(state, action) {
      state.subject = action.payload.subject;
      state.proposalId = action.payload.proposalId;
    },
  },
});

export const {
  updatePlatformParameters,
  updateAnken,
  updateProposal,
} = PlatformSystemSlice.actions;

export default PlatformSystemSlice.reducer;
