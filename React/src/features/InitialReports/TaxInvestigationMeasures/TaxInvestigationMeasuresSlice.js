import { createSlice } from '@reduxjs/toolkit';
import { TaxInvestigationMeasuresType } from './TaxInvestigationMeasuresType';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
    taxInvestigationMeasuresType: TaxInvestigationMeasuresType.Details
};

const TaxInvestigationMeasuresSlice = createSlice({
    name: 'taxInvestigationMeasures',
    initialState,
    reducers: {
        changeTaxInvestigationMeasures(state, action) {
            const { taxInvestigationMeasuresType } = action.payload;
            state.taxInvestigationMeasuresType = taxInvestigationMeasuresType;
        },
    },
    extraReducers: {
        [updatePdfParam]: (state, action) => {
            const { taxInvestigationMeasures } = action.payload;
            state.taxInvestigationMeasuresType = taxInvestigationMeasures.taxInvestigationMeasuresType;
        },
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    changeTaxInvestigationMeasures,
} = TaxInvestigationMeasuresSlice.actions;

export default TaxInvestigationMeasuresSlice.reducer;