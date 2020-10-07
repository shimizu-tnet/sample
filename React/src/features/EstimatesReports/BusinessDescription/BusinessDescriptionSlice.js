import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
  taxAccountantWorks: [
    {
      index: 0,
      text: "遺産・債務に関する通常必要な調査及び評価額計算、財産目録作成、中間報告"
    },
    {
      index: 1,
      text: "被相続人に係る相続税の税務代理、相続税申告書及び添付書類の作成、提出"
    },
    {
      index: 2,
      text: "3 回までの遺産分割協議用資料作成、同席アドバイス、 税額シミュレーション"
    },
    {
      index: 3,
      text: ""
    },
  ],
  administrativeScrivenerWorks: [
    {
      index: 0,
      text: "被相続人にかかる遺産分割協議書の作成"
    },
    {
      index: 1,
      text: "土地及び建物の相続登記（名義変更）の書類の整備、司法書士への取次"
    },
  ],
  showOptionWork: true,
};

const BusinessDescriptionSlice = createSlice({
  name: 'businessDescription',
  initialState,
  reducers: {
    initializeTaxAccountantWorks(state) {
      state.taxAccountantWorks = initialState.taxAccountantWorks;
    },
    updateTaxAccountantWorks(state, action) {
      const { taxAccountantWork } = action.payload;
      state.taxAccountantWorks[taxAccountantWork.index] = taxAccountantWork;
    },
    initializeAdministrativeScrivenerWorks(state) {
      state.administrativeScrivenerWorks = initialState.administrativeScrivenerWorks;
    },
    updatetaxAdministrativeScrivenerWorks(state, action) {
      const { administrativeScrivenerWork } = action.payload;
      state.administrativeScrivenerWorks[administrativeScrivenerWork.index] = administrativeScrivenerWork;
    },
    updateBusinessDescription(state, action) {
      const { businessDescription } = action.payload;
      state.showOptionWork = businessDescription.showOptionWork;
    }
  },
  extraReducers: {
    [updatePdfParam]: (state, action) => {
      const { businessDescription } = action.payload;
      state.taxAccountantWorks = businessDescription.taxAccountantWorks;
      state.administrativeScrivenerWorks = businessDescription.administrativeScrivenerWorks;
      state.showOptionWork = businessDescription.showOptionWork;
    },
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  initializeTaxAccountantWorks,
  updateTaxAccountantWorks,
  initializeAdministrativeScrivenerWorks,
  updatetaxAdministrativeScrivenerWorks,
  updateBusinessDescription
} = BusinessDescriptionSlice.actions;

export default BusinessDescriptionSlice.reducer;