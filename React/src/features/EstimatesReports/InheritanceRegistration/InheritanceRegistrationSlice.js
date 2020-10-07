import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
  inheritanceRegistrations: [
    {
      index: 0,
      text: '不動産の相続登記費用の概算額は、司法書士報酬料・登録免許税等を含め40,000円となります。'
    },
    {
      index: 1,
      text: '本業務については、ご要望に応じて、オプション業務として、行政書士法人レガシィと提携司法書士 の共同でお手伝いをさせていただきます。'
    },
    {
      index: 2,
      text: 'また、不動産、金融資産の名義変更に必要な登記簿全部事項証明書、被相続人・相続人の戸籍の 全部事項証明書、住民票等の申請についても、弊社で代行してお取り寄せすることが可能です。'
    },
  ],
};

const InheritanceRegistrationSlice = createSlice({
  name: 'inheritanceRegistration',
  initialState,
  reducers: {
    initializeInheritanceRegistrations(state) {
      state.inheritanceRegistrations = initialState.inheritanceRegistrations;
    },
    updateInheritanceRegistration(state, action) {
      const { inheritanceRegistration } = action.payload;
      state.inheritanceRegistrations[inheritanceRegistration.index] = inheritanceRegistration;
    },
  },
  extraReducers: {
    [updatePdfParam]: (state, action) => {
      const { inheritanceRegistration } = action.payload;
      state.inheritanceRegistrations = inheritanceRegistration.inheritanceRegistrations;
    },
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  initializeInheritanceRegistrations,
  updateInheritanceRegistration,
} = InheritanceRegistrationSlice.actions;

export default InheritanceRegistrationSlice.reducer;