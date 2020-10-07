import { createSlice } from '@reduxjs/toolkit';
import { calculateInheritanceTax } from '../../../api/legacyDocument';
import { updateBasicFee, updateHeirAdditionalFee } from '../../EstimatesReports/Fee/FeeSlice';
import { initializeTaxPayments, createTaxPaymentsParameter } from '../TaxPayment/TaxPaymentSlice';
import { setApiFailure, setError, hasResponseError } from '../../Errors/ErrorSlice';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
    inheritanceTotalAmount: null,
    propertyTotalAmount: null,
    basicDeduction: 0,
    taxableAmount: 0,
    totalInheritanceTax: 0,
    spousalTaxReduction: 0,
    paidTaxAmount: 0,
    // 相続税の試算額の基礎控除の人数で使用
    heirCount: 1,
    hasSpouse: false,
    descriptions: [
        { index: 0, isShow: false, text: "" },
        { index: 1, isShow: false, text: "" },
        { index: 2, isShow: false, text: "" },
        { index: 3, isShow: false, text: "" },
        { index: 4, isShow: false, text: "" },
        { index: 5, isShow: false, text: "" },
        { index: 6, isShow: false, text: "" },
    ],
    // お見積書の相続人加算で使用
    estimatedHeirsCount: 1,
};

const update = (state, action) => {
    const { estimatedInheritanceTax } = action.payload;
    state.inheritanceTotalAmount = estimatedInheritanceTax.inheritanceTotalAmount;
    state.propertyTotalAmount = estimatedInheritanceTax.propertyTotalAmount;
    state.basicDeduction = estimatedInheritanceTax.basicDeduction;
    state.taxableAmount = estimatedInheritanceTax.taxableAmount;
    state.totalInheritanceTax = estimatedInheritanceTax.totalInheritanceTax;
    state.spousalTaxReduction = estimatedInheritanceTax.spousalTaxReduction;
    state.paidTaxAmount = estimatedInheritanceTax.paidTaxAmount;
    state.heirCount = estimatedInheritanceTax.heirCount;
    state.hasSpouse = estimatedInheritanceTax.hasSpouse;
    state.estimatedHeirsCount = estimatedInheritanceTax.estimatedHeirsCount;
    state.descriptions.forEach(current => {
        const description = estimatedInheritanceTax.descriptions
            .find(item => item.index === current.index);
        if (!description) {
            current.isShow = false;
            current.text = '';
            return;
        }

        current.text = description.text;
        current.isShow = description.isShow;
    });
}

const EstimatedInheritanceTaxSlice = createSlice({
    name: 'estimatedInheritanceTax',
    initialState,
    reducers: {
        updateEstimatedInheritanceTax: update,
        updateDescription(state, action) {
            const { description } = action.payload;
            state.descriptions[description.index] = description;
        },
        updateEstimatedHeirsCount(state, action) {
            state.estimatedHeirsCount = action.payload.estimatedHeirsCount;
        },
    },
    extraReducers: {
        [updatePdfParam]: update,
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    updateEstimatedInheritanceTax,
    updateDescription,
    updateEstimatedHeirsCount,
} = EstimatedInheritanceTaxSlice.actions;

export default EstimatedInheritanceTaxSlice.reducer;

const applyIsShowOfDscriptions = (origins, destinations) => {
    if (!destinations) {
        return;
    }

    origins.forEach(origin => {
        const destination = destinations
            .find(item => item.index === origin.index);
        if (destination) {
            destination.isShow = origin.isShow;
        }
    });
}

export const fetchInheritanceTax = (inheritanceTaxParameters) => {
    return async (dispatch, getState) => {
        try {
            const estimatedInheritanceTax = await calculateInheritanceTax(inheritanceTaxParameters);
            const descriptions = getState().estimatedInheritanceTax.descriptions;
            // 説明の表示の現在の選択状態を適用
            applyIsShowOfDscriptions(descriptions, estimatedInheritanceTax.estimatedInheritanceTax.descriptions);
            dispatch(updateEstimatedInheritanceTax(estimatedInheritanceTax));

            const fee = estimatedInheritanceTax.fee;
            dispatch(updateBasicFee({ fee }));
            dispatch(updateHeirAdditionalFee({ fee }));

            const state = getState();
            const taxPaymentsParameter = createTaxPaymentsParameter(state);
            dispatch(initializeTaxPayments(taxPaymentsParameter));
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