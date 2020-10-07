import BigNumber from "bignumber.js";

export const isNumber = value => /^[-]?(\d+)(\.\d+)?$/.test(`${value}`);

export const toZeroIfEmpty = value => value ? Number(value) : 0;

export const toOneIfEmpty = value => value ? Number(value) : 1;

export const toMinusOneIfEmpty = value => value ? Number(value) : -1;

export const isNegative = value => Number(value) < 0;

export const calculatePropertyAmount = amount => Math.round(amount / 1000) * 1000;

export const calculateTaxPaymentAmount = amount => Math.trunc(amount / 1000);

export const calculateEstimatedAmountWithTax = amount => {
  const estimatedAmount = new BigNumber(amount);
  const consumptionTax = new BigNumber(1.1);
  const oneMillion = new BigNumber(1000000);
  const overOneMillionEstimatedAmount = estimatedAmount.gte(oneMillion);
  const initialPaymentAmountWithTax = overOneMillionEstimatedAmount
    ? estimatedAmount.times(new BigNumber(0.4)).times(consumptionTax).integerValue(BigNumber.ROUND_DOWN)
    : estimatedAmount.times(new BigNumber(0.5)).times(consumptionTax).integerValue(BigNumber.ROUND_DOWN);
  const interimPaymentAmountWithTax = overOneMillionEstimatedAmount
    ? estimatedAmount.times(new BigNumber(0.3)).times(consumptionTax).integerValue(BigNumber.ROUND_DOWN)
    : null;
  const finalPaymentAmountWithTax = overOneMillionEstimatedAmount
    ? estimatedAmount.times(new BigNumber(0.3)).times(consumptionTax).integerValue(BigNumber.ROUND_DOWN)
    : estimatedAmount.times(new BigNumber(0.5)).times(consumptionTax).integerValue(BigNumber.ROUND_DOWN);
  const estimatedAmountWithTax
    = initialPaymentAmountWithTax.plus(interimPaymentAmountWithTax || new BigNumber(0)).plus(finalPaymentAmountWithTax);

  return ({
    initialPaymentAmountWithTax: initialPaymentAmountWithTax.toNumber(),
    interimPaymentAmountWithTax: interimPaymentAmountWithTax && interimPaymentAmountWithTax.toNumber(),
    finalPaymentAmountWithTax: finalPaymentAmountWithTax.toNumber(),
    estimatedAmountWithTax: estimatedAmountWithTax.toNumber()
  });
}
