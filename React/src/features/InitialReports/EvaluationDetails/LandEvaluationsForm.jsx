import React, { useState, useEffect } from 'react';
import CurrencyParagraph from '../../../components/CurrencyParagraph';
import { toZeroIfEmpty, toOneIfEmpty, toMinusOneIfEmpty } from '../../../helpers/numberHelper';
import BigNumber from "bignumber.js";

/** 倍率 */
const magnification = "0";

/** その他 */
const other = "1";

/**
 * 評価額を算出する
 * ・倍率
 *    ㎡当り評価額 ＝正面路線価×正面補正率÷倍率（地積）
 *    自用地評価額 ＝正面路線価×正面補正率
 *    評価額       ＝自用地評価額×権利割合×持分
 */
const calculateEvaluationMagnification = landDetail => {
  const frontRouteEvaluation = new BigNumber(toZeroIfEmpty(landDetail.frontRouteEvaluation));
  const frontRouteCorrectionRate = new BigNumber(toZeroIfEmpty(landDetail.frontRouteCorrectionRate));
  const area = new BigNumber(toOneIfEmpty(landDetail.area));
  const rightsRatio = new BigNumber(toOneIfEmpty(landDetail.rightsRatio));
  const molecule = new BigNumber(toOneIfEmpty(landDetail.molecule));
  const denominator = new BigNumber(toOneIfEmpty(landDetail.denominator));
  const ownedRatio = molecule.div(denominator);
  const devaluationRightsRatio = new BigNumber(toMinusOneIfEmpty(landDetail.devaluationRightsRatio));
  const devaluationMolecule = new BigNumber(toOneIfEmpty(landDetail.devaluationMolecule));
  const devaluationDenominator = new BigNumber(toOneIfEmpty(landDetail.devaluationDenominator));
  const devaluationOwnedRatio = devaluationMolecule.div(devaluationDenominator);

  /** ㎡当り評価額を算出する */
  const squareEvaluation = frontRouteEvaluation
    .times(frontRouteCorrectionRate)
    .div(area)
    .integerValue(BigNumber.ROUND_DOWN);

  /** 自用地評価額を算出する */
  const ownLandEvaluation = frontRouteEvaluation
    .times(frontRouteCorrectionRate)
    .integerValue(BigNumber.ROUND_DOWN);

  /** 評価額 */
  const evaluation = ownLandEvaluation
    .times(rightsRatio)
    .times(ownedRatio)
    .integerValue(BigNumber.ROUND_DOWN);

  /** 自用地評価額（減額） */
  const devaluationOwnLandEvaluation = frontRouteEvaluation
    .times(frontRouteCorrectionRate)
    .integerValue(BigNumber.ROUND_DOWN);

  /** 評価額（減額） */
  const devaluation = devaluationOwnLandEvaluation
    .times(devaluationRightsRatio)
    .times(devaluationOwnedRatio)
    .integerValue(BigNumber.ROUND_DOWN);

  return {
    squareEvaluation: parseFloat(squareEvaluation),
    ownLandEvaluation: parseFloat(ownLandEvaluation),
    evaluation: parseFloat(evaluation),
    devaluationOwnLandEvaluation: parseFloat(devaluationOwnLandEvaluation),
    devaluation: parseFloat(devaluation),
  };
}

/**
 * 評価額を算出する
 * ・倍率以外
 *    ㎡当り評価額 ＝（（正面路線価×正面補正率）
 *                 ＋（側方路線価①×側方補正率①×側方加算率①）
 *                 ＋（側方路線価②×側方補正率②×側方加算率②）
 *                 ＋（裏面路線価×裏面補正率×裏面加算率））
 *                 ×画地補正率
 *    自用地評価額 ＝㎡当り評価額×地積（㎡）
 *    評価額       ＝自用地評価額×権利割合×持分
 */
const calculateEvaluationOther = landDetail => {
  const frontRouteEvaluation = new BigNumber(toZeroIfEmpty(landDetail.frontRouteEvaluation));
  const frontRouteCorrectionRate = new BigNumber(toZeroIfEmpty(landDetail.frontRouteCorrectionRate));
  const sideRouteEvaluation1 = new BigNumber(toZeroIfEmpty(landDetail.sideRouteEvaluation1));
  const sideRouteCorrectionRate1 = new BigNumber(toZeroIfEmpty(landDetail.sideRouteCorrectionRate1));
  const sideRouteAdditionRate1 = new BigNumber(toZeroIfEmpty(landDetail.sideRouteAdditionRate1));
  const sideRouteEvaluation2 = new BigNumber(toZeroIfEmpty(landDetail.sideRouteEvaluation2));
  const sideRouteCorrectionRate2 = new BigNumber(toZeroIfEmpty(landDetail.sideRouteCorrectionRate2));
  const sideRouteAdditionRate2 = new BigNumber(toZeroIfEmpty(landDetail.sideRouteAdditionRate2));
  const backRouteEvaluation = new BigNumber(toZeroIfEmpty(landDetail.backRouteEvaluation));
  const backRouteCorrectionRate = new BigNumber(toZeroIfEmpty(landDetail.backRouteCorrectionRate));
  const backRouteAdditionRate = new BigNumber(toZeroIfEmpty(landDetail.backRouteAdditionRate));
  const landCorrectionRate = new BigNumber(toOneIfEmpty(landDetail.landCorrectionRate));
  const area = new BigNumber(toZeroIfEmpty(landDetail.area));
  const rightsRatio = new BigNumber(toOneIfEmpty(landDetail.rightsRatio));
  const molecule = new BigNumber(toOneIfEmpty(landDetail.molecule));
  const denominator = new BigNumber(toOneIfEmpty(landDetail.denominator));
  const ownedRatio = molecule.div(denominator);
  const devaluationArea = new BigNumber(toZeroIfEmpty(landDetail.devaluationArea));
  const devaluationRightsRatio = new BigNumber(toMinusOneIfEmpty(landDetail.devaluationRightsRatio));
  const devaluationMolecule = new BigNumber(toOneIfEmpty(landDetail.devaluationMolecule));
  const devaluationDenominator = new BigNumber(toOneIfEmpty(landDetail.devaluationDenominator));
  const devaluationOwnedRatio = devaluationMolecule.div(devaluationDenominator);

  /** ㎡当り評価額 */
  const squareEvaluation
    = frontRouteEvaluation.times(frontRouteCorrectionRate)
      .plus(sideRouteEvaluation1.times(sideRouteCorrectionRate1).times(sideRouteAdditionRate1))
      .plus(sideRouteEvaluation2.times(sideRouteCorrectionRate2).times(sideRouteAdditionRate2))
      .plus(backRouteEvaluation.times(backRouteCorrectionRate).times(backRouteAdditionRate))
      .times(landCorrectionRate)
      .integerValue(BigNumber.ROUND_DOWN);

  /** 自用地評価額 */
  const ownLandEvaluation = squareEvaluation
    .times(area)
    .integerValue(BigNumber.ROUND_DOWN);

  /** 評価額 */
  const evaluation = ownLandEvaluation
    .times(rightsRatio)
    .times(ownedRatio)
    .integerValue(BigNumber.ROUND_DOWN);

  /** 自用地評価額（減額） */
  const devaluationOwnLandEvaluation = squareEvaluation
    .times(devaluationArea)
    .integerValue(BigNumber.ROUND_DOWN);

  /** 評価額（減額） */
  const devaluation = devaluationOwnLandEvaluation
    .times(devaluationRightsRatio)
    .times(devaluationOwnedRatio)
    .integerValue(BigNumber.ROUND_DOWN);

  return {
    squareEvaluation: parseFloat(squareEvaluation),
    ownLandEvaluation: parseFloat(ownLandEvaluation),
    evaluation: parseFloat(evaluation),
    devaluationOwnLandEvaluation: parseFloat(devaluationOwnLandEvaluation),
    devaluation: parseFloat(devaluation),
  };
}

/** 評価額を算出する */
const calculateEvaluation = landDetail => {
  switch (landDetail.calculationMethod) {
    case magnification:
      return calculateEvaluationMagnification(landDetail);

    case other:
      return calculateEvaluationOther(landDetail);

    default:
      return {
        squareEvaluation: 0,
        ownLandEvaluation: 0,
        evaluation: 0,
        devaluationOwnLandEvaluation: 0,
        devaluation: 0,
      };
  }
}

/** 土地・借地権の評価明細一覧表 入力フォーム */
const LandEvaluationsForm = ({ controlID, detail, handleSubmitClicked }) => {
  // 状態管理
  const [landDetail, setLandDetail] = useState(detail);

  useEffect(() => {
    setLandDetail(detail);
  }, [detail]);

  // 入力値の更新
  const inputValueChanged = key => e => {
    let target = { ...landDetail };
    target[key] = e.target.value;

    if (key === 'symbol') {
      target.devaluationSymbol = e.target.value;
    }

    if (key === 'address') {
      target.devaluationAddress = e.target.value;
    }

    if (key === 'districtCategory') {
      target.devaluationDistrictCategory = e.target.value;
    }

    target = { ...target, ...calculateEvaluation(target) };
    setLandDetail(target);
  }

  // 入力値の更新
  const inputCheckedChanged = key => e => {
    const target = { ...landDetail };
    target[key] = e.target.checked;
    setLandDetail(target);
  }

  /** 閉じるボタン押下 */
  const handleCloseClicked = () => {
    setLandDetail(detail);
    document.getElementById(controlID).close();
  }

  const disableEnterKey = e => {
    if (`${e.target.tagName}`.toLowerCase() === "textarea") {
      return;
    }

    if (window.event.keyCode === 13) {
      e.preventDefault();
    }
  }

  const disableRatioInput = (landDetail.calculationMethod === magnification);

  return (
    <dialog id={controlID} style={{ cursor: 'auto' }} onKeyPress={disableEnterKey}>
      <section id="modal-area" className="modal-area">
        <div className="modal-wrapper">
          <div className="modal-contents">
            <div className="modal-land-table">
              <div className="modal-land-data">
                {/* 評価額 */}
                <div className="modal-land-table-header">
                  <input type="checkbox" name="" id={`${controlID} check1`} disabled='disabled' checked='checked' />
                  <label htmlFor="check1">評価額</label>
                </div>
                <div className="modal-land-table-data">
                  {/* 地図上の表記 */}
                  <div className="modal-land-1">
                    <p>地図上の表記</p>
                  </div>
                  <div className="modal-land-2">
                    <input type="text" size="35" maxLength="5" value={landDetail.symbol} onChange={inputValueChanged('symbol')} />
                  </div>
                  {/* 所在地番 */}
                  <div className="modal-land-3">
                    <p>所在地番</p>
                  </div>
                  <div className="modal-land-4">
                    <textarea value={landDetail.address} onChange={inputValueChanged('address')}></textarea>
                  </div>
                  {/* 使用者 */}
                  <div className="modal-land-5">
                    <p>使用者</p>
                  </div>
                  <div className="modal-land-6">
                    <input type="text" size="35" value={landDetail.user} onChange={inputValueChanged('user')} />
                  </div>
                  {/* 利用区分 */}
                  <div className="modal-land-7">
                    <p>利用区分</p>
                  </div>
                  <div className="modal-land-8">
                    <input type="text" size="35" value={landDetail.usageCategory} onChange={inputValueChanged('usageCategory')} />
                  </div>
                  {/* 地目 */}
                  <div className="modal-land-9">
                    <p>地目</p>
                  </div>
                  <div className="modal-land-10">
                    <input type="text" size="35" value={landDetail.landCategory} onChange={inputValueChanged('landCategory')} />
                  </div>
                  {/* 計算方法 */}
                  <div className="modal-land-11">
                    <p>計算方法</p>
                  </div>
                  <div className="modal-land-12">
                    <input type="radio" name={`${controlID}calcMethod`} id={`${controlID}calcRate`} className="radio-input"
                      value={magnification}
                      checked={landDetail.calculationMethod === magnification}
                      onChange={inputValueChanged('calculationMethod')} />
                    <label htmlFor={`${controlID}calcRate`} className="font-wightR">倍率</label>
                    <input type="radio" name={`${controlID}calcMethod`} id={`${controlID}calcOther`} className="radio-input"
                      value={other}
                      checked={landDetail.calculationMethod === other}
                      onChange={inputValueChanged('calculationMethod')} />
                    <label htmlFor={`${controlID}calcOther`} className="font-wightR">その他</label>
                  </div>
                  {/* 地区区分 */}
                  <div className="modal-land-13">
                    <p>地区区分</p>
                  </div>
                  <div className="modal-land-14">
                    <input type="text" size="35" value={landDetail.districtCategory} onChange={inputValueChanged('districtCategory')} />
                  </div>
                  {/* 地積（㎡） */}
                  <div className="modal-land-15">
                    <p>地積（㎡）</p>
                  </div>
                  <div className="modal-land-16">
                    <input type="number" min={0.00} step={0.01}
                      value={landDetail.area} onChange={inputValueChanged('area')} />
                  </div>
                  {/* ブランク */}
                  <div className="modal-land-17">
                    <p></p>
                  </div>
                  {/* 正面 */}
                  <div className="modal-land-18">
                    <p>正面</p>
                  </div>
                  {/* 側方 */}
                  <div className="modal-land-19">
                    <p>側方</p>
                  </div>
                  {/* 側方 */}
                  <div className="modal-land-20">
                    <p>側方</p>
                  </div>
                  {/* 裏面 */}
                  <div className="modal-land-21">
                    <p>裏面</p>
                  </div>
                  {/* 路線価 */}
                  <div className="modal-land-22">
                    <p>路線価</p>
                  </div>
                  {/* 路線価　正面 */}
                  <div className="modal-land-23">
                    <input type="number" min={0} max={9999999999} step={1}
                      value={landDetail.frontRouteEvaluation} onChange={inputValueChanged('frontRouteEvaluation')} />
                  </div>
                  {/* 路線価　側方 */}
                  <div className="modal-land-24">
                    <input type="number" min={0} max={9999999999} step={1} disabled={disableRatioInput}
                      value={landDetail.sideRouteEvaluation1} onChange={inputValueChanged('sideRouteEvaluation1')} />
                  </div>
                  {/* 路線価　側方 */}
                  <div className="modal-land-25">
                    <input type="number" min={0} max={9999999999} step={1} disabled={disableRatioInput}
                      value={landDetail.sideRouteEvaluation2} onChange={inputValueChanged('sideRouteEvaluation2')} />
                  </div>
                  {/* 路線価　裏面 */}
                  <div className="modal-land-26">
                    <input type="number" min={0} max={9999999999} step={1} disabled={disableRatioInput}
                      value={landDetail.backRouteEvaluation} onChange={inputValueChanged('backRouteEvaluation')} />
                  </div>
                  {/* 補正率 */}
                  <div className="modal-land-27">
                    <p>補正率</p>
                  </div>
                  {/* 補正率　正面 */}
                  <div className="modal-land-28">
                    <input type="number" min={0.00} max={999.99} step={0.01}
                      value={landDetail.frontRouteCorrectionRate} onChange={inputValueChanged('frontRouteCorrectionRate')} />
                  </div>
                  {/* 補正率　側方 */}
                  <div className="modal-land-29">
                    <input type="number" min={0.00} max={999.99} step={0.01} disabled={disableRatioInput}
                      value={landDetail.sideRouteCorrectionRate1} onChange={inputValueChanged('sideRouteCorrectionRate1')} />
                  </div>
                  {/* 補正率　側方 */}
                  <div className="modal-land-30">
                    <input type="number" min={0.00} max={999.99} step={0.01} disabled={disableRatioInput}
                      value={landDetail.sideRouteCorrectionRate2} onChange={inputValueChanged('sideRouteCorrectionRate2')} />
                  </div>
                  {/* 補正率　裏面 */}
                  <div className="modal-land-31">
                    <input type="number" min={0.00} max={999.99} step={0.01} disabled={disableRatioInput}
                      value={landDetail.backRouteCorrectionRate} onChange={inputValueChanged('backRouteCorrectionRate')} />
                  </div>
                  {/* 加算率 */}
                  <div className="modal-land-32">
                    <p>加算率</p>
                  </div>
                  {/* 加算率　正面 */}
                  <div className="modal-land-33">
                    {/* ブランク */}
                  </div>
                  {/* 加算率　側方 */}
                  <div className="modal-land-34">
                    <input type="number" min={0.00} max={999.99} step={0.01} disabled={disableRatioInput}
                      value={landDetail.sideRouteAdditionRate1} onChange={inputValueChanged('sideRouteAdditionRate1')} />
                  </div>
                  {/* 加算率　側方 */}
                  <div className="modal-land-35">
                    <input type="number" min={0.00} max={999.99} step={0.01} disabled={disableRatioInput}
                      value={landDetail.sideRouteAdditionRate2} onChange={inputValueChanged('sideRouteAdditionRate2')} />
                  </div>
                  {/* 加算率　裏面 */}
                  <div className="modal-land-36">
                    <input type="number" min={0.00} max={999.99} step={0.01} disabled={disableRatioInput}
                      value={landDetail.backRouteAdditionRate} onChange={inputValueChanged('backRouteAdditionRate')} />
                  </div>
                  {/* 画地補正率 */}
                  <div className="modal-land-37">
                    <p>画地補正率</p>
                  </div>
                  <div className="modal-land-38">
                    <input type="number" min={0.00} max={999.99} step={0.01}
                      value={landDetail.landCorrectionRate} onChange={inputValueChanged('landCorrectionRate')} />
                  </div>
                  {/* ㎡当り評価額 */}
                  <div className="modal-land-39">
                    <p>㎡当り評価額</p>
                  </div>
                  <div className="modal-land-40">
                    <CurrencyParagraph value={landDetail.squareEvaluation} showUnit={true} />
                  </div>
                  {/* 自用地評価額 */}
                  <div className="modal-land-41">
                    <p>自用地評価額</p>
                  </div>
                  <div className="modal-land-42">
                    <CurrencyParagraph value={landDetail.ownLandEvaluation} showUnit={true} />
                  </div>
                  {/* 権利割合 */}
                  <div className="modal-land-13">
                    <p>権利割合</p>
                  </div>
                  <div className="modal-land-14">
                    <input type="number" min={0.00} max={999.99} step={0.01}
                      value={landDetail.rightsRatio} onChange={inputValueChanged('rightsRatio')} />
                  </div>
                  {/* 持分 */}
                  <div className="modal-land-13">
                    <p>持分</p>
                  </div>
                  <div className="modal-land-43">
                    <input type="number" min={1} max={99} step={1}
                      value={landDetail.molecule} onChange={inputValueChanged('molecule')} />
                    <span>/</span>
                    <input type="number" min={1} max={99} step={1}
                      value={landDetail.denominator} onChange={inputValueChanged('denominator')} />
                  </div>
                </div>
                {/* 評価額 */}
                <div className="modal-total">
                  <div className="modal-total-title">
                    <p>評価額</p>
                  </div>
                  <div className="modal-total-amount">
                    <CurrencyParagraph value={landDetail.evaluation} showUnit={true} />
                  </div>
                </div>
              </div>
              <div className="modal-land-reduction-data">
                {/* 評価減額 */}
                <div className="modal-land-table-header">
                  <input type="checkbox" id={`${controlID}showDevaluation`} defaultChecked={landDetail.showDevaluation} onChange={inputCheckedChanged('showDevaluation')} />
                  <label htmlFor={`${controlID}showDevaluation`}>評価減額</label>
                </div>
                {landDetail.showDevaluation && (
                  <>
                    <div className="modal-land-table-data">
                      {/* 地図上の表記 */}
                      <div className="modal-land-1">
                        <p>地図上の表記</p>
                      </div>
                      <div className="modal-land-2">
                        <input type="text" size="35" maxLength="5" disabled={!landDetail.showDevaluation} value={landDetail.devaluationSymbol} onChange={inputValueChanged('devaluationSymbol')} />
                      </div>
                      {/* 所在地番 */}
                      <div className="modal-land-3">
                        <p>所在地番</p>
                      </div>
                      <div className="modal-land-4">
                        <textarea disabled={!landDetail.showDevaluation} value={landDetail.devaluationAddress} onChange={inputValueChanged('devaluationAddress')}></textarea>
                      </div>
                      {/* 地区区分 */}
                      <div className="modal-land-13">
                        <p>地区区分</p>
                      </div>
                      <div className="modal-land-14">
                        <input type="text" size="35" disabled={!landDetail.showDevaluation} value={landDetail.devaluationDistrictCategory} onChange={inputValueChanged('devaluationDistrictCategory')} />
                      </div>
                      {/* 地積（㎡） */}
                      <div className="modal-land-13">
                        <p>地積（㎡）</p>
                      </div>
                      <div className="modal-land-14">
                        <input type="number" min={0.00} step={0.01} disabled={!landDetail.showDevaluation}
                          value={landDetail.devaluationArea} onChange={inputValueChanged('devaluationArea')} />
                      </div>
                      {/* 自用地評価額 */}
                      <div className="modal-land-13">
                        <p>自用地評価額</p>
                      </div>
                      <div className="modal-land-14">
                        <CurrencyParagraph value={landDetail.devaluationOwnLandEvaluation} showUnit={true} />
                      </div>
                      {/* 権利割合 */}
                      <div className="modal-land-13">
                        <p>権利割合</p>
                      </div>
                      <div className="modal-land-14">
                        <input type="number" min={-999.99} max={0.00} step={0.01} disabled={!landDetail.showDevaluation}
                          value={landDetail.devaluationRightsRatio} onChange={inputValueChanged('devaluationRightsRatio')} />
                      </div>
                      {/* 持分 */}
                      <div className="modal-land-13">
                        <p>持分</p>
                      </div>
                      <div className="modal-land-43">
                        <input type="number" min={1} max={99} step={1} disabled={!landDetail.showDevaluation}
                          value={landDetail.devaluationMolecule} onChange={inputValueChanged('devaluationMolecule')} />
                        <span>/</span>
                        <input type="number" min={1} max={99} step={1} disabled={!landDetail.showDevaluation}
                          value={landDetail.devaluationDenominator} onChange={inputValueChanged('devaluationDenominator')} />
                      </div>
                    </div>
                    {/* 評価減額 */}
                    <div className="modal-total">
                      <div className="modal-total-title">
                        <p>評価減額</p>
                      </div>
                      <div className="modal-total-amount">
                        <CurrencyParagraph value={landDetail.devaluation} showUnit={true} />
                      </div>
                    </div>
                  </>
                )}
              </div>
              <div className="addition">
                <button type="button" className="btn edit-btn" onClick={handleSubmitClicked(landDetail)}>確定</button>
                <button type="button" className="btn edit-btn cancel" onClick={handleCloseClicked}>キャンセル</button>
              </div>
            </div>
          </div>
        </div>
      </section>
    </dialog >
  );
}

export default LandEvaluationsForm;