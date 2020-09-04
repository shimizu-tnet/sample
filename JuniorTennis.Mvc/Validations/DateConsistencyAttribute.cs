using System;
using System.ComponentModel.DataAnnotations;


namespace JuniorTennis.Mvc.Validations
{
    /// <summary>
    /// 日付の整合性検証を行います。
    /// </summary>
    public class DateConsistencyAttribute : ValidationAttribute
    {
        /// <summary>
        /// 年のプロパティ名を取得します。
        /// </summary>
        public string YearPropatyName { get; }

        /// <summary>
        /// 月のプロパティ名を取得します。
        /// </summary>
        public string MonthPropatyName { get; }

        /// <summary>
        /// 新しい検証のインスタンスを生成します。
        /// </summary>
        /// <param name="yearPropatyName">年のプロパティ名。</param>
        /// <param name="monthPropatyName">月のプロパティ名。</param>
        public DateConsistencyAttribute(string yearPropatyName, string monthPropatyName)
        {
            this.YearPropatyName = yearPropatyName;
            this.MonthPropatyName = monthPropatyName;
        }

        /// <summary>
        /// 新しい検証のインスタンスを生成します。
        /// </summary>
        /// <param name="yearPropatyName">年のプロパティ名。</param>
        /// <param name="monthPropatyName">月のプロパティ名。</param>
        /// <param name="errorMessage">エラーメッセージ。</param>
        public DateConsistencyAttribute(string yearPropatyName, string monthPropatyName, string errorMessage)
        {
            this.YearPropatyName = yearPropatyName;
            this.MonthPropatyName = monthPropatyName;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 検証を行います。
        /// </summary>
        /// <param name="value">検証対象の値。</param>
        /// <param name="validationContext">検証用コンテキスト。</param>
        /// <returns>検証結果。</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var yearProperty = validationContext.ObjectInstance.GetType().GetProperty(this.YearPropatyName);
            if (yearProperty == null)
            {
                throw new InvalidOperationException($"指定したプロパティ名が見つかりません。Name:{this.YearPropatyName}");
            }

            var monthProperty = validationContext.ObjectInstance.GetType().GetProperty(this.MonthPropatyName);
            if (monthProperty == null)
            {
                throw new InvalidOperationException($"指定したプロパティ名が見つかりません。Name:{this.MonthPropatyName}");
            }

            var yearPropertyValue = yearProperty.GetValue(validationContext.ObjectInstance);
            if (yearPropertyValue == null)
            {
                // 値がないため検証しない
                return ValidationResult.Success;
            }

            var monthPropertyValue = monthProperty.GetValue(validationContext.ObjectInstance);
            if (monthPropertyValue == null)
            {
                // 値がないため検証しない
                return ValidationResult.Success;
            }

            if(value == null)
            {
                // 値がないため検証しない
                return ValidationResult.Success;
            }

            var year = yearPropertyValue.ToString();
            var month = monthPropertyValue.ToString();
            var day = value.ToString();
            if (!DateTime.TryParse($"{year}/{month}/{day}", out var dateTime))
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
