import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { toZeroIfEmpty } from '../../../helpers/numberHelper';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
    details: [],
    comments: [
        '※小規模宅地等の特例は、配偶者様がご自宅を取得されることを前提として適用しています。',
        '※今年度の路線価は、7 月発表のため、前年度の路線価・倍率を仮に使用しています。',
    ],
    totalEvaluation: '0',
    totalDevaluation: '0',
    taxableAmount: '0',
};

// 新規に土地の行を作成する
const newDetail = landID => {
    return {
        landID,
        isEvaluation: true,
        symbol: "",
        address: "",
        user: "",
        usageCategory: "",
        landCategory: "",
        calculationMethod: "1",
        districtCategory: "",
        area: "",
        frontRouteEvaluation: "",
        frontRouteCorrectionRate: "",
        sideRouteEvaluation1: "",
        sideRouteCorrectionRate1: "",
        sideRouteAdditionRate1: "",
        sideRouteEvaluation2: "",
        sideRouteCorrectionRate2: "",
        sideRouteAdditionRate2: "",
        backRouteEvaluation: "",
        backRouteCorrectionRate: "",
        backRouteAdditionRate: "",
        landCorrectionRate: "",
        squareEvaluation: "",
        ownLandEvaluation: "",
        rightsRatio: "",
        denominator: "",
        molecule: "",
        evaluation: "",
        showDevaluation: false,
        devaluationSymbol: "",
        devaluationAddress: "",
        devaluationDistrictCategory: "",
        devaluationArea: "",
        devaluationRightsRatio: "",
        devaluationDenominator: "",
        devaluationMolecule: "",
        devaluationOwnLandEvaluation: "",
        devaluation: "",
    };
};

export const sumEvaluations = details => {
    if (!details.length) {
        return 0;
    }

    return details
        .map(v => toZeroIfEmpty(v.evaluation))
        .reduce((accumulator, currentValue) => accumulator + currentValue);
}

export const sumDevaluations = details => {
    const devaluations = details.filter(v => v.showDevaluation);
    if (!devaluations.length) {
        return 0;
    }

    return devaluations
        .map(v => toZeroIfEmpty(v.devaluation))
        .reduce((accumulator, currentValue) => accumulator + currentValue);
}

const landEvaluationsSlice = createSlice({
    name: 'landEvaluations',
    initialState,
    reducers: {
        // 土地の行を追加
        addDetail(state, action) {
            const IDs = state.details.map(v => v.landID);
            const newID = Math.max(0, ...IDs) + 1;
            state.details.push(newDetail(newID));
        },
        // 土地の行を削除
        deleteDetail(state, action) {
            state.details = action.payload.details;
            state.totalEvaluation = action.payload.totalEvaluation;
            state.totalDevaluation = action.payload.totalDevaluation;
            state.taxableAmount = action.payload.taxableAmount;
        },
        // 土地の行を更新
        updateDetail(state, action) {
            const { detail, totalEvaluation, totalDevaluation, taxableAmount } = action.payload;
            const index = state.details
                .findIndex(item => item.landID === detail.landID);
            const details = [...state.details];
            details[index] = detail;
            state.details = details;
            state.totalEvaluation = totalEvaluation;
            state.totalDevaluation = totalDevaluation;
            state.taxableAmount = taxableAmount;
        },
        setDetails(state, action) {
            state.details = action.payload.replacedDetails;
        },
        // コメントの行を追加
        addComment(state) {
            state.comments.push('');
        },
        // コメントの行を削除
        deleteComment(state, action) {
            const { index } = action.payload;
            const filtered = state.comments.filter((v, i) => i !== index);
            state.comments = filtered;
        },
        // コメントを更新
        updateComment(state, action) {
            const { index, value } = action.payload;
            state.comments[index] = value;
        },
    },
    extraReducers: {
        [updatePdfParam]: (state, action) => {
            const { landEvaluations } = action.payload;
            state.details = landEvaluations.details;
            state.comments = landEvaluations.comments;
            state.totalEvaluation = landEvaluations.totalEvaluation;
            state.totalDevaluation = landEvaluations.totalDevaluation;
            state.taxableAmount = landEvaluations.taxableAmount;
        },
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    addDetail,
    deleteDetail,
    updateDetail,
    setDetails,
    addComment,
    deleteComment,
    updateComment,
} = landEvaluationsSlice.actions;

export default landEvaluationsSlice.reducer;