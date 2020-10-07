import React from 'react';

const HouseEvaluationHeader = () => {
  return (
    <div className="house-table-header">
      {/* 土地No. */}
      <div className="house-1">
        <p></p>
      </div>
      {/* 所在地番 */}
      <div className="house-2">
        <p>所在地番</p>
      </div>
      {/* 利用区分 */}
      <div className="house-3">
        <p>利用区分</p>
      </div>
      {/* 固定資産評価額 */}
      <div className="house-4">
        <p>固定資産評価額</p>
      </div>
      {/* 倍率 */}
      <div className="house-5">
        <p>倍率</p>
      </div>
      {/* 持分 */}
      <div className="house-6">
        <p>持分</p>
      </div>
      {/* 相続税評価額 */}
      <div className="house-7">
        <p>相続税評価額</p>
      </div>
      {/* 編集 */}
      <div className="house-8">
        <p>編集</p>
      </div>
      {/* 物件名（使用者） */}
      <div className="house-9">
        <p>使物件名（使用者）</p>
      </div>
    </div>
  );
};

export default HouseEvaluationHeader;