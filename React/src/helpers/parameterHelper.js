const merger = (merged, diff) => {
  for (const key of Object.keys(diff)) {
    if (!(key in merged)) {
      // パラメータの追加
      merged[key] = diff[key];
      continue;
    }
    if (typeof diff[key] === 'object' && diff[key] !== null) {
      // オブジェクトの場合は再帰呼び出し
      merger(merged[key], diff[key]);
    } else {
      // パラメータの更新
      merged[key] = diff[key];
    }
  }
}

export const deepMerge = (base, difference) => {
  let merged = JSON.parse(JSON.stringify(base));
  merger(merged, JSON.parse(JSON.stringify(difference)));
  return merged;
}
