using System;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Validations
{
    /// <summary>
    /// 日付の整合性チェック。
    /// 対象とBeginPropertyNameのプロパティの値を比較して時系列が正しい(BeginProperty < 対象)かどうか検証します。
    /// </summary>
    public class DateTimeAfterAttribute : ValidationAttribute
    {
        public string BeginPropertyName { get; }

        public bool AllowEquivalent { get; }

        public DateTimeAfterAttribute(string beginPropertyName)
        {
            this.BeginPropertyName = beginPropertyName;
            this.AllowEquivalent = false;
        }
        public DateTimeAfterAttribute(string beginPropertyName, string errorMessage)
        {
            this.BeginPropertyName = beginPropertyName;
            this.AllowEquivalent = false;
            this.ErrorMessage = errorMessage;
        }

        public DateTimeAfterAttribute(string beginPropertyName, bool allowEquivalent)
        {
            this.BeginPropertyName = beginPropertyName;
            this.AllowEquivalent = allowEquivalent;
        }

        public DateTimeAfterAttribute(string beginPropertyName, bool allowEquivalent, string errorMessage)
        {
            this.BeginPropertyName = beginPropertyName;
            this.AllowEquivalent = allowEquivalent;
            this.ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var beginProperty = validationContext.ObjectInstance.GetType().GetProperty(this.BeginPropertyName);
            if (beginProperty == null)
            {
                throw new ArgumentException(string.Format("指定したプロパティ名が見つかりません。Name:{0}", beginProperty));
            }

            var beginDateTimeValue = beginProperty.GetValue(validationContext.ObjectInstance);
            if (beginDateTimeValue == null)
            {
                // 比較できないため検証しない
                return ValidationResult.Success;
            }

            if(value == null)
            {
                // 比較できないため検証しない
                return ValidationResult.Success;
            }

            var beginDateTime = (DateTime)beginDateTimeValue;
            var endDateTime = (DateTime)value;
            if (this.AllowEquivalent)
            {
                if (endDateTime == beginDateTime)
                {
                    return ValidationResult.Success;
                }
            }

            if (beginDateTime < endDateTime)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
