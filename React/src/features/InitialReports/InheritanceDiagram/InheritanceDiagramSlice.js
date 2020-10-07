import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

export const deceasedID = 0;

let initialState = {
    heirPattern: 1,
    relatives: [
        {
            relativeID: deceasedID,
            relationship: 1,
            supplement: '被相続人',
            firstName: '',
            lastName: '',
            firstNameKana: '',
            lastNameKana: '',
            age: '',
            isHeir: false,
            abandonedInheritance: false,
            targetRelativeID: null
        },
    ]
};

const InheritanceDiagramSlice = createSlice({
    name: 'inheritanceDiagram',
    initialState,
    reducers: {
        changeHeirPattern(state, action) {
            const { heirPattern } = action.payload;
            state.heirPattern = heirPattern;
            state.relatives = state.relatives.filter(relative => relative.relativeID === deceasedID);
        },
        addRelative(state) {
            const IDs = state.relatives.map(relative => relative.relativeID);
            const newID = Math.max(...IDs) + 1;
            state.relatives.push({
                relativeID: newID,
                relationship: 0,
                supplement: '',
                firstName: '',
                lastName: '',
                firstNameKana: '',
                lastNameKana: '',
                age: '',
                isHeir: true,
                abandonedInheritance: false,
                targetRelativeID: null,
            });
        },
        updateReative(state, action) {
            const { relative } = action.payload;
            const index = state.relatives
                .findIndex(item => item.relativeID === relative.relativeID);

            // 関係が更新されたら血縁者を初期化する
            if (state.relatives[index].relationship !== relative.relationship) {
                relative.targetRelativeID = null;
            }

            // 相続放棄したら相続権を失う
            if (relative.abandonedInheritance) {
                relative.isHeir = false;
            }

            state.relatives[index] = relative;
        },
        deleteRelative(state, action) {
            const { relativeID } = action.payload;
            const index = state.relatives
                .findIndex(item => item.relativeID === relativeID);
            state.relatives.splice(index, 1);
        },
        setRelatives(state, action) {
            state.relatives = action.payload.replacedRelatives;
        },
    },
    extraReducers: {
        [updatePdfParam]: (state, action) => {
            const { inheritanceDiagram } = action.payload;
            state.heirPattern = inheritanceDiagram.heirPattern;
            state.relatives = inheritanceDiagram.relatives;
        },
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    changeHeirPattern,
    addRelative,
    updateReative,
    deleteRelative,
    setRelatives,
} = InheritanceDiagramSlice.actions;

export default InheritanceDiagramSlice.reducer;