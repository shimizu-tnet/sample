import React from 'react';
import CurrencyParagraph from '../../../components/CurrencyParagraph';
import NumberParagraph from '../../../components/NumberParagraph';

const LandEvaluationDetailRow = ({ detail, className }) => {
  /** 計算方法を取得 */
  const calculationMethodName = () => {
    switch (detail.calculationMethod) {
      case "0":
        return "倍率";
      case "1":
        return "その他";
      default:
        return "";
    }
  }
  /** 持分を取得 */
  const ownedRatio = () => {
    if (!(detail.molecule && detail.denominator)) {
      return "";
    }
    return `${detail.molecule}/${detail.denominator}`;
  }

  return (
    <>
      <div className={className}>
        {/* 土地No. */}
        <div className="land-17">
          <p>{detail.symbol}</p>
        </div>
        {/* 所在地番 */}
        <div className="land-18">
          <pre>{detail.address}</pre>
        </div>
        {/* 計算方法 */}
        <div className="land-19">
          <p>{calculationMethodName()}</p>
        </div>
        {/* 地区区分 */}
        <div className="land-20">
          <p>{detail.districtCategory}</p>
        </div>
        {/* 正面路線価 */}
        <div className="land-21">
          <CurrencyParagraph value={detail.frontRouteEvaluation} />
        </div>
        {/* 側方路線価 */}
        <div className="land-22">
          <CurrencyParagraph value={detail.sideRouteEvaluation1} />
        </div>
        {/* 側方路線価 */}
        <div className="land-23">
          <CurrencyParagraph value={detail.sideRouteEvaluation2} />
        </div>
        {/* 裏面路線価 */}
        <div className="land-24">
          <CurrencyParagraph value={detail.backRouteEvaluation} />
        </div>
        {/* 画地補正率 */}
        <div className="land-25">
          <NumberParagraph value={detail.landCorrectionRate} />
        </div>
        {/* 権利割合 */}
        <div className="land-26">
          <NumberParagraph value={detail.rightsRatio} />
        </div>
        {/* 正面補正率 */}
        <div className="land-27">
          <NumberParagraph value={detail.frontRouteCorrectionRate} />
        </div>
        {/* 側方補正率 */}
        <div className="land-28">
          <NumberParagraph value={detail.sideRouteCorrectionRate1} />
        </div>
        {/* 側方補正率 */}
        <div className="land-29">
          <NumberParagraph value={detail.sideRouteCorrectionRate2} />
        </div>
        {/* 裏面補正率 */}
        <div className="land-30">
          <NumberParagraph value={detail.backRouteCorrectionRate} />
        </div>
        {/* ㎡当り評価額 */}
        <div className="land-31">
          <CurrencyParagraph value={detail.squareEvaluation} />
        </div>
        {/* 持分 */}
        <div className="land-32">
          <p>{ownedRatio()}</p>
        </div>
        {/* 使用者 */}
        <div className="land-33">
          <p>{detail.user}</p>
        </div>
        {/* 利用区分 */}
        <div className="land-34">
          <p>{detail.usageCategory}</p>
        </div>
        {/* 地目 */}
        <div className="land-35">
          <p>{detail.landCategory}</p>
        </div>
        {/* 地積 */}
        <div className="land-36">
          <NumberParagraph value={detail.area} />
        </div>
        {/* 正面加算率 */}
        <div className="land-37">
          <p>-</p>
        </div>
        {/* 側方加算率 */}
        <div className="land-38">
          <NumberParagraph value={detail.sideRouteAdditionRate1} />
        </div>
        {/* 側方加算率 */}
        <div className="land-39">
          <NumberParagraph value={detail.sideRouteAdditionRate2} />
        </div>
        {/* 裏面加算率 */}
        <div className="land-40">
          <NumberParagraph value={detail.backRouteAdditionRate} />
        </div>
        {/* 自用地評価額 */}
        <div className="land-41">
          <CurrencyParagraph value={detail.ownLandEvaluation} />
        </div>
        {/* 評価額 */}
        <div className="land-42">
          <CurrencyParagraph value={detail.evaluation} />
        </div>
      </div>
    </>
  )
}

export default LandEvaluationDetailRow;