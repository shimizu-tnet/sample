import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { toZeroIfEmpty, toOneIfEmpty } from '../../../helpers/numberHelper';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';
import BigNumber from "bignumber.js";

/** 新規に土地の行を作成する */
const newDetail = houseID => {
    return {
        /** 家屋のユニークな ID を取得または設定します。 */
        houseID,
        /** 地図上の表記を取得または設定します。 */
        symbol: "",
        /** 所在地番を取得または設定します。 */
        address: "",
        /** 物件名（使用者）を取得または設定します。 */
        user: "",
        /** 利用区分を取得または設定します。 */
        usageCategory: forOwn,
        /** 固定資産税評価額を取得または設定します。 */
        propertyEvaluation: "",
        /** 倍率を取得または設定します。 */
        magnification: forOwnRate,
        /** 持分の分母を取得または設定します。 */
        denominator: "",
        /** 持分の分子を取得または設定します。 */
        molecule: "",
        /** 持分を取得します。 */
        ownedRatio: "",
        /** 評価額を取得または設定します。 */
        evaluation: "",
    };
};

/** 自用 */
const forOwn = "0";
const forOwnRate = "1.00";

/** 貸家 */
const forRent = "1";
const forRentRate = "0.70";

/** 倍率を取得する */
export const getMagnification = usageCategory => {
    switch (usageCategory) {
        case forOwn:
            return forOwnRate;
        case forRent:
            return forRentRate;
        default:
            return "1.00";
    }
}

/** 評価額を算出する */
export const calculateEvaluation = detail => {
    const propertyEvaluation = new BigNumber(toZeroIfEmpty(detail.propertyEvaluation));
    const magnification = new BigNumber(toOneIfEmpty(detail.magnification));
    const molecule = new BigNumber(toOneIfEmpty(detail.molecule));
    const denominator = new BigNumber(toOneIfEmpty(detail.denominator));
    const ownedRatio = molecule.div(denominator);
    const evaluation = propertyEvaluation
        .times(magnification)
        .times(ownedRatio)
        .integerValue(BigNumber.ROUND_DOWN);

    return parseFloat(evaluation);
}

/** 評価額の合計を算出する */
export const calculateTotalEvaluation = details => {
    const totalEvaluation = [...details]
        .map(detail => toZeroIfEmpty(detail.evaluation))
        .reduce((accumulator, currentValue) => accumulator + currentValue, 0);

    return totalEvaluation;
}

/** 初期値。 */
let initialState = {
    details: [],
    comments: [],
    totalEvaluation: '0',
};

const houseEvaluationsSlice = createSlice({
    name: 'houseEvaluations',
    initialState,
    reducers: {
        // 家屋の行を追加
        addDetail(state, action) {
            const IDs = state.details.map(v => v.houseID);
            const newID = Math.max(0, ...IDs) + 1;
            state.details.push(newDetail(newID));
        },
        // 家屋の行を削除
        deleteDetail(state, action) {
            state.details = action.payload.details;
            state.totalEvaluation = action.payload.totalEvaluation;
        },
        // 家屋の行を更新
        updateDetail(state, action) {
            const { index, detail, totalEvaluation } = action.payload;
            state.details[index] = detail;
            state.totalEvaluation = totalEvaluation;
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
            const { houseEvaluations } = action.payload;
            state.details = houseEvaluations.details;
            state.comments = houseEvaluations.comments;
            state.totalEvaluation = houseEvaluations.totalEvaluation;
        },
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    addDetail,
    deleteDetail,
    updateDetail,
    updateMagnification,
    setDetails,
    addComment,
    deleteComment,
    updateComment,
} = houseEvaluationsSlice.actions;

export default houseEvaluationsSlice.reducer;