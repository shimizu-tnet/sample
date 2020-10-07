import { createSlice } from '@reduxjs/toolkit';

let initialState = {
  accessToken: '',
};

const LoginSlice = createSlice({
  name: 'login',
  initialState,
  reducers: {
    updateAccessToken(state, action) {
      state.accessToken = action.payload.accessToken;
    },
    deleteAccessToken(state) {
      state.accessToken = '';
    },
  }
});

export const {
  updateAccessToken,
  deleteAccessToken,
} = LoginSlice.actions;

export default LoginSlice.reducer;
