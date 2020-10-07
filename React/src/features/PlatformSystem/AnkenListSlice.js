import { createSlice } from '@reduxjs/toolkit';
import { updateAnken } from './PlatformSystemSlice';

const initialState = {
  ankenName: '',
  loginAdminName: '',
  pages: null,
  pageNo: 0,
  existsPreviousPage: false,
  existsNextPage: false,
};

const AnkenListSlice = createSlice({
  name: 'ankenList',
  initialState,
  reducers: {
    updatePageNo(state, action) {
      state.pageNo = action.payload.pageNo;
      state.existsPreviousPage = state.pageNo > 0;
      state.existsNextPage = state.pageNo < state.pages.length - 1;
    },
  },
  extraReducers: {
    [updateAnken](state, action) {
      state.ankenName = action.payload.ankenName;
      state.loginAdminName = action.payload.loginAdminName;
      state.pages = action.payload.pages;

      if (action.payload.doInit) {
        state.pageNo = initialState.pageNo;
      } else {
        const list = state.pages.flatMap((page, pageNo) => page.map(list => ({ pageNo, ...list })));
        const index = list.findIndex(proposal => `${proposal.id}` === `${action.payload.proposalId}`);
        if (index === -1) {
          state.pageNo = 0;
        } else {
          state.pageNo = list[index].pageNo;
        }
      }

      state.existsPreviousPage = state.pageNo > 0;
      state.existsNextPage = state.pageNo < state.pages.length - 1;
    }
  }
});

export const {
  updateAnkenList,
  updatePageNo,
} = AnkenListSlice.actions;

export default AnkenListSlice.reducer;
