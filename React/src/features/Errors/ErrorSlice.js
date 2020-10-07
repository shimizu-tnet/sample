import { createSlice } from '@reduxjs/toolkit';
import { updateAccessToken } from '../../features/LoginSlice';
import { updateProposal } from '../../features/PlatformSystem/PlatformSystemSlice';

const initialState = {
  errorMessages: null
}

const getApiErrorMessage = response => {
  if (hasResponseStatus(response)) {
    if (response.status === 400 && hasErrorMessage(response)) {
      // プラットフォームシステム側API呼び出しエラー
      return [response.data.message];
    }

    if (response.status === 401) {
      // 提案書作成システム側の認証切れ
      return ['セッション切れのため、再度ログインしてください。'];
    }

    if (response.status === 403) {
      // プラットフォームシステム側の認証切れ
      return ['セッション切れのため、再度ログインしてください。'];
    }
  }

  return ['処理でエラーが発生したため、再度実行してください。'];
}

export const hasResponseError = err => {
  return typeof err !== 'undefined'
    && typeof err.response !== 'undefined';
}

export const hasResponseStatus = response => {
  return typeof response !== 'undefined'
    && typeof response.status !== 'undefined';
}

export const hasErrorMessage = response => {
  return typeof response.data !== 'undefined'
    && typeof response.data.message !== 'undefined';
}

const ErrorSlice = createSlice({
  name: 'error',
  initialState,
  reducers: {
    initializeError(state) {
      state.errorMessages = initialState.errorMessages;
    },
    setApiFailure(state, action) {
      const { response } = action.payload;
      state.errorMessages = getApiErrorMessage(response);
    },
    setError(state, action) {
      state.errorMessages = action.payload.errorMessages;
    }
  },
  extraReducers: {
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  initializeError,
  setApiFailure,
  setError,
} = ErrorSlice.actions;

export default ErrorSlice.reducer;