using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JuniorTennis.Domain.Teams
{
    /// <summary>
    /// 団体名略称。
    /// </summary>
    public class TeamAbbreviatedName : ValueObject
    {
        /// <summary>
        /// 団体名略称の最大文字数。
        /// </summary>
        public static int MaxLength => 13;

        /// <summary>
        /// 団体名略称を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 団体名略称の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">団体名。</param>
        public TeamAbbreviatedName(string value) =>
            this.Value
                = value == null ? throw new ArgumentNullException("団体名略称")
                : string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("未入力です。", "団体名略称")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "団体名略称")
                : this.ContainsWideAlphanumericChar(value) ? throw new ArgumentException("全角英数字が入力されています。", "団体名略称")
                : this.ContainsHalfKanaChar(value) ? throw new ArgumentException("半角カナが入力されています。", "団体名略称")
                : value;

        /// <summary>
        /// 団体名略称をvalueObjectの仕様に沿う形に変換します(全角英数字→半角、半角カナ→全角)
        /// </summary>
        /// <param name="value">変換元文字列。</param>
        /// <returns>変換後文字列。。</returns>
        public static TeamAbbreviatedName Parse(string sourceString)
        {
            var alphanumericPattern = "[０-９Ａ-Ｚａ-ｚ]";
            var replaced = Regex.Replace(sourceString, alphanumericPattern, p => ((char)(p.Value[0] - 'Ａ' + 'A')).ToString());
            replaced = ToKatakanaFromKatakanaHalf(replaced);

            return new TeamAbbreviatedName(replaced);
        }

        /// <summary>
        /// 入力された団体名略称が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された団体名略称。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        /// <summary>
        /// 入力された団体名略称に全角英数字が入力されているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された団体名略称。</param>
        /// <returns>全角英数字が入力されている場合は true。それ以外の場合は false。</returns>
        private bool ContainsWideAlphanumericChar(string value)
        {
            return Regex.IsMatch(value, @"[０-９Ａ-Ｚａ-ｚ]");
        }

        /// <summary>
        /// 入力された団体名略称に半角カナが入力されているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された団体名略称。</param>
        /// <returns>半角カナが入力されている場合は true。それ以外の場合は false。</returns>
        private bool ContainsHalfKanaChar(string value)
        {
            return Regex.IsMatch(value, @"[ｦ-ﾟ]");
        }

        /// <summary>
        /// カタカナのテーブル
        /// </summary>
        private static readonly string[] KatakanaTable = new string[]
        {
             "ガ", "ｶﾞ", "ギ", "ｷﾞ", "グ", "ｸﾞ", "ゲ", "ｹﾞ", "ゴ", "ｺﾞ",
             "ザ", "ｻﾞ", "ジ", "ｼﾞ", "ズ", "ｽﾞ", "ゼ", "ｾﾞ", "ゾ", "ｿﾞ",
             "ダ", "ﾀﾞ", "ヂ", "ﾁﾞ", "ヅ", "ﾂﾞ", "デ", "ﾃﾞ", "ド", "ﾄﾞ",
             "バ", "ﾊﾞ", "ビ", "ﾋﾞ", "ブ", "ﾌﾞ", "ベ", "ﾍﾞ", "ボ", "ﾎﾞ",
             "ヴ", "ｳﾞ", "ヷ", "ﾜﾞ", "ヸ", "ｲﾞ", "ヹ", "ｴﾞ", "ヺ", "ｦﾞ",
             "パ", "ﾊﾟ", "ピ", "ﾋﾟ", "プ", "ﾌﾟ", "ペ", "ﾍﾟ", "ポ", "ﾎﾟ",
             "ア", "ｱ", "イ", "ｲ", "ウ", "ｳ", "エ", "ｴ", "オ", "ｵ",
             "カ", "ｶ", "キ", "ｷ", "ク", "ｸ", "ケ", "ｹ", "コ", "ｺ",
             "サ", "ｻ", "シ", "ｼ", "ス", "ｽ", "セ", "ｾ", "ソ", "ｿ",
             "タ", "ﾀ", "チ", "ﾁ", "ツ", "ﾂ", "テ", "ﾃ", "ト", "ﾄ",
             "ナ", "ﾅ", "ニ", "ﾆ", "ヌ", "ﾇ", "ネ", "ﾈ", "ノ", "ﾉ",
             "ハ", "ﾊ", "ヒ", "ﾋ", "フ", "ﾌ", "ヘ", "ﾍ", "ホ", "ﾎ",
             "マ", "ﾏ", "ミ", "ﾐ", "ム", "ﾑ", "メ", "ﾒ", "モ", "ﾓ",
             "ヤ", "ﾔ",            "ユ", "ﾕ",            "ヨ", "ﾖ",
             "ラ", "ﾗ", "リ", "ﾘ", "ル", "ﾙ", "レ", "ﾚ", "ロ", "ﾛ",
             "ワ", "ﾜ",            "ヲ", "ｦ",            "ン", "ﾝ",
             "ヱ", "ｴ",
             "ァ", "ｧ", "ィ", "ｨ", "ゥ", "ｩ", "ェ", "ｪ", "ォ", "ｫ",
             "ャ", "ｬ",            "ュ", "ｭ",            "ョ", "ｮ",
             "ッ", "ｯ",
             "ー", "ｰ", "、", "､", "。", "｡", "　", " "
        };

        /// <summary>
        /// 半角カタカナを全角カタカナに変換します。
        /// </summary>
        /// <param name="srcString">変換する文字列</param>
        /// <returns>半角カタカナが全角カタカナに変換された <paramref name="srcString"/></returns>
        private static string ToKatakanaFromKatakanaHalf(string srcString)
        {
            if (string.IsNullOrEmpty(srcString))
            {
                return string.Empty;
            }

            for (var i = 0; i < KatakanaTable.Length; i += 2)
            {
                srcString = srcString.Replace(KatakanaTable[i + 1], KatakanaTable[i]);
            }

            return srcString;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
