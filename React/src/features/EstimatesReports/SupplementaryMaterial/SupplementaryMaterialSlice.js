import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
  additionalFeeDescription: '',
};

const update = (state, action) => {
  const { supplementaryMaterial } = action.payload;
  const { additionalFeeDescription } = supplementaryMaterial;
  state.additionalFeeDescription = additionalFeeDescription;
}

const supplementaryMaterialSlice = createSlice({
  name: 'supplementaryMaterial',
  initialState,
  reducers: {
    updateDescription: update
  },
  extraReducers: {
    [updatePdfParam]: update,
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  updateDescription,
} = supplementaryMaterialSlice.actions;

export default supplementaryMaterialSlice.reducer;