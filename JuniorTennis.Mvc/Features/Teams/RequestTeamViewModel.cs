using JuniorTennis.Domain.Teams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JuniorTennis.Domain.UseCases.Teams;
using System.Text.RegularExpressions;

namespace JuniorTennis.Mvc.Features.Teams
{
    public class RequestTeamViewModel
    {
        [Required(ErrorMessage = "団体種別を選択してください。")]
        [Display(Name = "団体種別")]
        public string TeamType { get; set; }

        [Required(ErrorMessage = "団体名を入力してください。")]
        [Display(Name = "団体名")]
        public string TeamName { get; set; }

        [Required(ErrorMessage = "団体名略称を入力してください。")]
        [Display(Name = "団体名略称")]
        [MaxLength(13)]
        public string TeamAbbreviatedName { get; set; }

        [Required(ErrorMessage = "代表者を入力してください。")]
        [Display(Name = "代表者")]
        public string RepresentativeName { get; set; }

        [Display(Name = "代表者 メールアドレス")]
        public string RepresentativeEmailAddress { get; set; }

        [Phone(ErrorMessage = "電話番号の形式で入力してください。")]
        [Display(Name = "電話番号")]
        public string TelephoneNumber { get; set; }

        [Display(Name = "住所")]
        public string Address { get; set; }

        [Display(Name = "顧問/コーチ")]
        public string CoachName { get; set; }

        [EmailAddress(ErrorMessage = "メールアドレスの形式で入力してください。")]
        [Display(Name = "顧問/コーチ メールアドレス")]
        public string CoachEmailAddress { get; set; }

        public bool IsDuplicated { get; set; }

        /// <summary>
        /// 団体登録用 DTO に変換します。
        /// </summary>
        /// <returns>団体登録用 DTO。</returns>
        public RequestTeamNewRegistrationDto ToDto()
        {
            return new RequestTeamNewRegistrationDto()
            {
                TeamType = int.Parse(this.TeamType),
                TeamName = this.TeamName,
                TeamAbbreviatedName = ConvertToString(this.TeamAbbreviatedName),
                RepresentativeName = this.RepresentativeName,
                RepresentativeEmailAddress = this.RepresentativeEmailAddress,
                TelephoneNumber = this.TelephoneNumber,
                Address = this.Address,
                CoachName = this.CoachName,
                CoachEmailAddress = this.CoachEmailAddress
            };
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
        public static string ToKatakanaFromKatakanaHalf(string srcString)
        {
            if (string.IsNullOrEmpty(srcString))
            {
                return string.Empty;
            }

            for (int i = 0; i < KatakanaTable.Length; i++)
            {
                srcString = srcString.Replace(KatakanaTable[i + 1], KatakanaTable[i]);
                i++;
            }

            return srcString;
        }

        /// <summary>
        /// 文字列を英数字は半角、カナは全角に変換します
        /// </summary>
        /// <param name="sourceString">変換元文字列</param>
        /// <returns>結果文字列</returns>
        public string ConvertToString(string sourceString)
        {
            var alphanumericPattern = "[０-９Ａ-Ｚａ-ｚ]";
            var replaced = Regex.Replace(sourceString, alphanumericPattern, p => ((char)(p.Value[0] - 'Ａ' + 'A')).ToString());
            replaced = ToKatakanaFromKatakanaHalf(replaced);

            return replaced;
        }
    }
}
