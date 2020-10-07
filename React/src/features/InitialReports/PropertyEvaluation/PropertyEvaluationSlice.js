import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
    propertyEvaluations: [
        {
            index: 0,
            heading: "土地について",
            text: "前頁の土地の評価については、机上での概算評価です。実際のお手伝いの中では現地を確認し、必要に応じ役所調査等を実施し、土地の利用制限、公法上の制限等の有無を確認し、実務上是認されている最大限の評価減を加味し、最も有利な申告を行って行きます。",
        },
        {
            index: 1,
            heading: "自社株について（原則評価の場合）",
            text: "前頁の自社株の評価については、貸借対照表上の純資産額を基に算定しております。実際のお手伝いの中では、会社が所有している財産を個別に詳細評価し、実務上是認される最も有利な評価額を算定します。",
        },
        {
            index: 2,
            heading: "自社株について（少数株主の場合）",
            text: "前頁の自社株の評価については、保有されている株式の額面金額を基に計上しております。実際のお手伝いの中では、株式の発行会社から評価に必要な資料（決算書、株主名簿など）を入手し、実務上是認される最も有利な評価額を算定します。",
        },
        {
            index: 3,
            heading: "その他の財産について",
            text: "その他、後述の税務調査対策の所でも考えて行くところですが、相続財産としてどこまで預貯金等の金融資産を計上するか、という点に関して、銀行等の取引記録を基に、合法かつ最も有利な方法を模索しながら、数値を確定していきます。",
        },
        { index: 4, heading: "", text: "", },
        { index: 5, heading: "", text: "", },
        { index: 6, heading: "", text: "", },
        { index: 7, heading: "", text: "", },
        { index: 8, heading: "", text: "", },
        { index: 9, heading: "", text: "", },
    ]
};

const PropertyEvaluationSlice = createSlice({
    name: 'propertyEvaluation',
    initialState,
    reducers: {
        initializePropertyEvaluations(state) {
            state.propertyEvaluations = initialState.propertyEvaluations;
        },
        updatePropertyEvaluation(state, action) {
            const { propertyEvaluation } = action.payload;
            state.propertyEvaluations[propertyEvaluation.index] = propertyEvaluation;
        },
    },
    extraReducers: {
        [updatePdfParam]: (state, action) => {
            const { propertyEvaluation } = action.payload;
            state.propertyEvaluations = propertyEvaluation.propertyEvaluations;
        },
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    initializePropertyEvaluations,
    updatePropertyEvaluation
} = PropertyEvaluationSlice.actions;

export default PropertyEvaluationSlice.reducer;