import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from '../../UpdatePdfParamAction';
import { toCurrencyValue, toJPDateString, getDisplayDeceasedName } from '../../../helpers/stringHelper';
import { toZeroIfEmpty, calculateTaxPaymentAmount } from '../../../helpers/numberHelper';
import { deceasedID } from '../InheritanceDiagram/InheritanceDiagramSlice';
import { savingsAmountPropertyID, insuranceAmountPropertyID } from '../PropertyDetails/PropertyDetailsSlice';
import { updateAccessToken } from '../../LoginSlice';
import { updateProposal } from '../../PlatformSystem/PlatformSystemSlice';

const createInitialState = (lastName, totalInheritanceTax, savingsAmount, insuranceAmount, finalTaxReturnDate) => {
    const lastNameText = getDisplayDeceasedName(lastName);
    const totalInheritanceTaxText = toCurrencyValue(totalInheritanceTax);
    const savingsAmountText = toCurrencyValue(savingsAmount);
    const insuranceAmountText = toCurrencyValue(insuranceAmount);
    const finalTaxReturnDateText = toJPDateString(finalTaxReturnDate);

    return ({
        taxPayments: [
            {
                index: 0,
                isShow: true,
                text: `相続税の納税方法の原則は金銭一時納付であり、金銭納付困難などの特段の事情がない限り、本来は申告期限（${finalTaxReturnDateText}）までに金銭で即納する必要があります。`,
            },
            {
                index: 1,
                isShow: true,
                text: `${lastNameText}様の場合､相続税額（${totalInheritanceTaxText}千円）を上回る預貯金（${savingsAmountText}千円）があることから､納税については問題ないと思われます｡`,
            },
            {
                index: 2,
                isShow: true,
                text: `しかしながら${lastNameText}様の場合、相続税額（${totalInheritanceTaxText}千円）を下回る金銭（${savingsAmountText}千円）しか残っておらず、申告期限までに土地の処分を行って現金を用意するか、土地そのものの所有権を国（財務省）に移転することにより納税に充てる物納を選択せざるを得ない状況にあります。`,
            },
            {
                index: 3,
                isShow: true,
                text: `${lastNameText}様の場合、相続税額（${totalInheritanceTaxText}千円）に対し､相続預貯金は（${savingsAmountText}千円）であることから､相続人様固有の財産等による納税が必要となります｡`,
            },
            {
                index: 4,
                isShow: true,
                text: `${lastNameText}様の場合､前項までの想定の下では､相続税の納税は不要と考えられますが､相続税の申告が必要となります｡（小規模宅地等の特例は､申告することにより認められるため､相続税がゼロの場合でも申告は必要です｡)\nなお､相続税の申告期限は､${finalTaxReturnDateText}であり､納税が必要な場合には､原則として同日までに金銭で即納する必要があります｡`,
            },
            {
                index: 5,
                isShow: true,
                text: `${lastNameText}様の場合､相続税額（${totalInheritanceTaxText}千円）を上回る預貯金（${savingsAmountText}千円）があること､また､小規模宅地等の特例の適用ができなかった場合の相続税額（${totalInheritanceTaxText}千円）を上回る生命保険金（${insuranceAmountText}千円）があることから納税については問題ないと思われます｡`,
            },
            {
                index: 6,
                isShow: true,
                text: "仮にどちらを選択することも可能な場合、売却と物納の損益分岐点は、次の算式により判断することができます。\n\n＜算式＞　A>B…売却が有利　　A<B…物納が有利\n　A. 売却価格－譲渡費用（仲介手数料・測量分筆費用等）－譲渡税\n　B. 収納価格－収納費用（物納税務代理報酬・測量分筆費用等）",
            },
        ]
    });
};

const initialState = createInitialState('', 0, 0, 0, '');

const TaxPaymentSlice = createSlice({
    name: 'taxPayment',
    initialState,
    reducers: {
        initializeTaxPayments(state, action) {
            const { lastName, totalInheritanceTax, savingsAmount, insuranceAmount, finalTaxReturnDate } = action.payload;
            const { taxPayments } = createInitialState(lastName, totalInheritanceTax, savingsAmount, insuranceAmount, finalTaxReturnDate);
            state.taxPayments = taxPayments;
        },
        updateTaxPayment(state, action) {
            const { taxPayment } = action.payload;
            state.taxPayments[taxPayment.index] = taxPayment;
        },
    },
    extraReducers: {
        [updatePdfParam]: (state, action) => {
            const { taxPayment } = action.payload;
            state.taxPayments = taxPayment.taxPayments;
        },
        [updateAccessToken]: (state, action) => initialState,
        [updateProposal]: (state, action) => initialState,
    }
});

export const {
    initializeTaxPayments,
    updateTaxPayment
} = TaxPaymentSlice.actions;

export default TaxPaymentSlice.reducer;

export const createTaxPaymentsParameter = state => {
    const {
        inheritanceDiagram,
        estimatedInheritanceTax,
        propertyDetails,
        schedules,
    } = state;
    const { relatives } = inheritanceDiagram;
    const { properties } = propertyDetails;
    const { lastName } = [...relatives].find(x => x.relativeID === deceasedID);
    const { totalInheritanceTax } = estimatedInheritanceTax;
    const { evaluationAmount: savingsAmount } = [...properties].find(x => x.propertyID === savingsAmountPropertyID);
    const { evaluationAmount: insuranceAmount } = [...properties].find(x => x.propertyID === insuranceAmountPropertyID);
    const { finalTaxReturnDate } = schedules;

    return {
        lastName,
        totalInheritanceTax: calculateTaxPaymentAmount(toZeroIfEmpty(totalInheritanceTax)),
        savingsAmount: calculateTaxPaymentAmount(toZeroIfEmpty(savingsAmount)),
        insuranceAmount: calculateTaxPaymentAmount(toZeroIfEmpty(insuranceAmount)),
        finalTaxReturnDate,
    };
}
