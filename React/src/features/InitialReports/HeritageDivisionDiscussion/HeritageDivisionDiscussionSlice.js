import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

let initialState = {
    heritageDivisionDiscussions: [
        {
            index: 0,
            isShow: true,
            text: "遺産の分割は、民法で定められた相続分を一つの目安として、個々の財産の利用価値、収益性、換金価値、税法の特例の適用関係なども考慮の上、現実的かつ経済的合理性を持った方法を相続人間の話し合いにより決めていくものです。",
        },
        {
            index: 1,
            isShow: true,
            text: "もちろん、その中で個々の家の価値観や財産に対する愛着など、理屈では割り切れない事情を考慮しなくてはならないこともあると思います。",
        },
        {
            index: 2,
            isShow: true,
            text: "私共は、分割に関するそうしたお客様の様々なご要望を考慮しつつ、中立公正な立場から経済的に最も有利な遺産分割案を提示し、お客様の意思決定の援助をさせて頂きます。",
        },
        {
            index: 3,
            isShow: true,
            text: "遺産の分割は、話し合いをスムーズに進めるために、被相続人が遺した遺産・債務のすべてを洗い出した財産目録を基に行うのが通例です。",
        },
        {
            index: 4,
            isShow: true,
            text: "",
        }
    ]
};

const HeritageDivisionDiscussionSlice = createSlice({
    name: 'heritageDivisionDiscussion',
    initialState,
    reducers: {
        initializeHeritageDivisionDiscussions(state) {
            state.heritageDivisionDiscussions = initialState.heritageDivisionDiscussions;
        },
        updateHeritageDivisionDiscussion(state, action) {
            const { heritageDivisionDiscussion } = action.payload;
            state.heritageDivisionDiscussions[heritageDivisionDiscussion.index] = heritageDivisionDiscussion;
        },
    },
    extraReducers: {
        [updatePdfParam]: (state, action) => {
            const { heritageDivisionDiscussion } = action.payload;
            state.heritageDivisionDiscussions = heritageDivisionDiscussion.heritageDivisionDiscussions;
        },
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    initializeHeritageDivisionDiscussions,
    updateHeritageDivisionDiscussion
} = HeritageDivisionDiscussionSlice.actions;

export default HeritageDivisionDiscussionSlice.reducer;