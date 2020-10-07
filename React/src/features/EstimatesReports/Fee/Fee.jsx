import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHandleInput } from '../../../helpers/useHandleInput';
import {
  calculateTotalOptionFee,
  calculateTotalFee,
  calculateEstimatedAmount,
  updateFee,
  updateLandAdditionalFee
} from './FeeSlice';
import { separateComma } from '../../../helpers/stringHelper';
import { calculateEstimatedAmountWithTax } from '../../../helpers/numberHelper';
import Option from './Option';

// デバッグ用　税込報酬額の内訳を表示したいときに true を設定する。
const debug = false;

const Fee = () => {
  const fee = useSelector(state => state.fee);
  const totalOptionFee = useSelector(calculateTotalOptionFee);
  const totalFee = useSelector(calculateTotalFee);
  const estimatedAmount = useSelector(calculateEstimatedAmount);
  const {
    initialPaymentAmountWithTax,
    interimPaymentAmountWithTax,
    finalPaymentAmountWithTax,
    estimatedAmountWithTax
  } = calculateEstimatedAmountWithTax(estimatedAmount);
  const options = fee.options.map(item => <Option key={item.index} option={item} />);
  const dispatch = useDispatch();
  const handleLandAddtionalFee = e => dispatch(updateLandAdditionalFee({ number: e.target.value }));
  const { inputValueChanged } = useHandleInput(updateFee, 'fee', fee);
  const dislayMoneyAmount = moneyAmount => {
    if (Number.isNaN(+moneyAmount)) {
      return '数字以外が入力されています。';
    }

    return separateComma(moneyAmount);
  }

  return (
    <form>
      <div className="input-table">
        <div className="input-table-row fee-row">
          <label>基本報酬額</label>
          <div></div>
          <div className="fee-amount">
            <input type="text" value={fee.basicFee || ''} onChange={inputValueChanged('basicFee')} />
          </div>
          <div>円</div>
        </div>
        <div className="input-table-row fee-row">
          <label>相続人加算</label>
          <div></div>
          <div className="fee-amount">
            <input type="text" value={fee.heirAdditionalFee || ''} onChange={inputValueChanged('heirAdditionalFee')} />
          </div>
          <div>円</div>
        </div>
        <div className="input-table-row fee-row">
          <label>土地加算</label>
          <div>
            <input type="number" min="1" max="999"
              value={fee.landAdditionalUnit} onChange={handleLandAddtionalFee} />
            <span>　利用単位</span>
          </div>
          <div className="fee-amount">{separateComma(fee.landAdditionalFee)}</div>
          <div>円</div>
        </div>
        <div className="input-table-row fee-row">
          <label>その他の加算</label>
          <div></div>
          <div className="fee-amount">{dislayMoneyAmount(totalOptionFee)}</div>
          <div>円</div>
        </div>
        {options}
        <div className="input-table-row fee-row">
          <label>合計報酬額</label>
          <div></div>
          <div className="fee-amount">{dislayMoneyAmount(totalFee)}</div>
          <div>円</div>
        </div>
        <div className="input-table-row fee-row">
          <label>報酬調整額</label>
          <div>
            <input type="text" value={fee.adjustmentAmountComment} onChange={inputValueChanged('adjustmentAmountComment')} />
          </div>
          <div>
            <input type="text" value={fee.adjustmentAmount || ''} onChange={inputValueChanged('adjustmentAmount')} />
          </div>
          <div>円</div>
        </div>
        <div className="input-table-row fee-row">
          <label>見積報酬額（税抜）</label>
          <div></div>
          <div className="fee-amount">{dislayMoneyAmount(estimatedAmount)}</div>
          <div>円</div>
        </div>
        <div className="input-table-row fee-row">
          <label>見積報酬額（税込）</label>
          <div></div>
          <div className="fee-amount">{dislayMoneyAmount(estimatedAmountWithTax)}</div>
          <div>円</div>
        </div>
        {debug &&
          (<>
            <div className="input-table-row fee-row">
              <label className="detail">- 初期報告時</label>
              <div></div>
              <div className="fee-amount detail">{dislayMoneyAmount(initialPaymentAmountWithTax)}</div>
              <div className="detail">円</div>
            </div>
            {interimPaymentAmountWithTax &&
              (<div className="input-table-row fee-row">
                <label className="detail">- 中間報告時</label>
                <div></div>
                <div className="fee-amount detail">{dislayMoneyAmount(interimPaymentAmountWithTax)}</div>
                <div className="detail">円</div>
              </div>)}
            <div className="input-table-row fee-row">
              <label className="detail">- 申告押印時</label>
              <div></div>
              <div className="fee-amount detail">{dislayMoneyAmount(finalPaymentAmountWithTax)}</div>
              <div className="detail">円</div>
            </div>
          </>)}
      </div >
    </form>
  );
}

export default Fee;
