using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Validations
{
    /// <summary>
    /// 対象プロパティのValueがTargetValuesと同じ場合必須検証を行います。
    /// 検証対象が単一な物のみに使用可能です。
    /// </summary>
    public class RequiredWhenAttribute : ValidationAttribute
    {
        /// <summary>
        /// 対象のプロパティ名を取得します。
        /// </summary>
        public string TargetPropertyName { get; }

        /// <summary>
        /// 対象の値を取得します。
        /// </summary>
        /// <remarks>カンマ区切りで複数指定可能。</remarks>
        public string TargetValues { get; }

        /// <summary>
        /// 新しい検証のインスタンスを生成します。
        /// </summary>
        /// <param name="targetPropertyName">対象のプロパティ名。</param>
        /// <param name="targetValues">対象の値。</param>
        public RequiredWhenAttribute(string targetPropertyName, string targetValues)
        {
            this.TargetPropertyName = targetPropertyName;
            this.TargetValues = targetValues;
        }

        /// <summary>
        /// 新しい検証のインスタンスを生成します。
        /// </summary>
        /// <param name="targetPropertyName">対象のプロパティ名。</param>
        /// <param name="targetValues">対象の値。</param>
        /// <param name="errorMessage">エラーメッセージ。</param>
        public RequiredWhenAttribute(string targetPropertyName, string targetValues, string errorMessage)
        {
            this.TargetPropertyName = targetPropertyName;
            this.TargetValues = targetValues;
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
            var targetProperty = validationContext.ObjectInstance.GetType().GetProperty(this.TargetPropertyName);
            if (targetProperty == null)
            {
                throw new InvalidOperationException(string.Format("指定したプロパティ名が見つかりません。Name:{0}", targetProperty));
            }

            var targetPropertyValue = targetProperty.GetValue(validationContext.ObjectInstance);
            if (targetPropertyValue == null)
            {
                // 比較できないため検証しない
                return ValidationResult.Success;
            }

            var targetPropertyValueString = targetPropertyValue.ToString();
            var targetValues = this.TargetValues.Split(',');
            if (targetValues.Contains(targetPropertyValueString))
            {
                if (value == null)
                {
                    return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
