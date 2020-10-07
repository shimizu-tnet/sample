import { createSlice } from '@reduxjs/toolkit';
import { updatePdfParam } from './UpdatePdfParamAction';
import { updateAccessToken } from './LoginSlice';
import { updateProposal } from './PlatformSystem/PlatformSystemSlice';

const ChiefTaxAccountantType = {
  PartnerTaxAccountant: 0,
  Partner: 1,
  PrincipalTaxAccountant: 2,
  Principal: 3,
  NetworkTaxAccountant: 4,
}

export const chiefTaxAccountantTypeNameOptions = ['パートナー税理士', 'パートナー', 'プリンスパル税理士', 'プリンスパル', ''];
export const chiefTaxAccountantNameOptions = [
  '',
  '田川　嘉朗',
  '大山　広見',
  '岡崎　孝行',
  '陽田　賢一',
  '木下　裕行',
  '小野　修',
  '武田　利之',
  '髙櫻　寛',
  '大久保　智',
  'くのり会計事務所',
  '柴田健次税理士事務所',
  'せいれん会計事務所',
  '廣田勝彦税理士事務所',
  '斉藤大税理士事務所',
  '荒木達也税理士事務所',
  '渡邉優税理士事務所',
  '神戸直紀税理士事務所',
  '渋谷大輔税理士事務所',
  '税理士法人フロイデ',
  '税理士五十嵐邦雄事務所',
  '税理士法人永川会計事務所',
  '木村亮太税理士事務所',
  '二見達彦税理士事務所',
  '馬場明彦税理士事務所',
  'サン共同税理士法人日本橋オフィス',
  '冨永正見税理士事務所',
  '奥抜利之税理士事務所',
  '長津裕樹税理士事務所',
  '吉田健二公認会計士税理士事務所',
  '天尾信之税理士事務所',
  '植崎紳矢税理士・不動産鑑定士事務所',
  '平光税理士事務所',
  '藤井琢夫税理士事務所',
  'いわみ会計事務所',
  '税理士法人心',
  '髙山智巳税理士事務所',
  '土屋悠税理士事務所',
];
export const taxAccountantNameOptions = [
  '',
  '大山　広見', '小川　幸雄', '松本　透', '寿原　大介', '林　昇平', '松澤　崇彦', '大友　智', '川口　貴司', '志賀　康彦',
  '岡崎　孝行', '佐藤　秀治', '松永　卓朗', '渡邉　大希', '長井　秀司', '坂本　茂人', '山﨑　成幸', '大山　竜矢', '堀井　悠平',
  '陽田　賢一', '大倉　孝雄', '峯島　健太郎', '中園　直希', '辻　佑希', '髙城　雅史', '阪上　剛彦', '今村　満也', '髙橋　和雅',
  '木下　裕行', '大塚　国子', '松本　明久', '古田　圭佑', '市橋　晃', '小林　礼一郎', '奥　美穂', '小川　陽平', '嘉陽　哲久', '中里　哲也', '松下　祐貴',
  '小野　修', '靏岡　直希', '木梨　和也', '岩渕　郷', '髙原　直樹', '木村　陽介', '大日向　直輝',
  '武田　利之', '宗形　泉', '佐藤　隆史', '佐々木　進吾', '田端　順子', '並河　陽平',
  '髙櫻　寛', '下山　悟', '八杉　努', '柿田　慎一', '原田　光太郎', '白坂　恵太',
  '大久保　智', '來嶋　洋', '市川　園美', '田中　涼太', '高木　智成', '八木　駿彦',
  '石出　陽一',
  '久徳　徹', '柴田　健次', '村上　正倫', '廣田　勝彦', '斉藤　大', '荒木　達也', '渡邉　優', '神戸　直紀', '渋谷　大輔', '羽田　哲也', '五十嵐　邦雄', '永川　輝行', '木村　亮太', '二見　達彦',
  '馬場　明彦', '坪池　剛', '冨永　正見', '奥抜　利之', '長津　裕樹', '吉田　健二', '天尾　信之', '植崎　紳矢', '平光　史明', '藤井　琢夫', '岩見　文吾', '岩崎　友哉', '髙山　智巳', '土屋　悠',
  '土田　光',
];

let initialState = {
  chiefTaxAccountantType: ChiefTaxAccountantType.PartnerTaxAccountant,
  chiefTaxAccountantName: '',
  taxAccountantNames: [
    '',
    '',
    '',
  ],
};

const TaxAccountantSlice = createSlice({
  name: 'taxAccountant',
  initialState,
  reducers: {
    updateChiefTaxAccountantType(state, action) {
      state.chiefTaxAccountantType = action.payload.chiefTaxAccountantType;
    },
    updateChiefTaxAccountantName(state, action) {
      state.chiefTaxAccountantName = action.payload.chiefTaxAccountantName;
    },
    updateTaxAccountant(state, action) {
      const { index, name } = action.payload;
      state.taxAccountantNames[index] = name;
    },
  },
  extraReducers: {
    [updatePdfParam]: (state, action) => {
      const { taxAccountant } = action.payload;
      state.chiefTaxAccountantType = taxAccountant.chiefTaxAccountantType;
      state.chiefTaxAccountantName = taxAccountant.chiefTaxAccountantName;
      state.taxAccountantNames.forEach((value, index) => {
        state.taxAccountantNames[index] = taxAccountant.taxAccountantNames[index];
      });
    },
    [updateAccessToken]: (state, action) => initialState,
    [updateProposal]: (state, action) => initialState,
  }
});

export const {
  updateChiefTaxAccountantType,
  updateChiefTaxAccountantName,
  updateTaxAccountant,
} = TaxAccountantSlice.actions;

export default TaxAccountantSlice.reducer;