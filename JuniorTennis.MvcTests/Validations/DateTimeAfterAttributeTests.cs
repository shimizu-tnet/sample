using System;
using Xunit;
using JuniorTennis.Mvc.Validations;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.MvcTests.Validations
{
    public class DateTimeAfterAttributeTests
    {

        [Fact]
        public void 開始時刻の値が終了時刻の値より古い場合検証有効()
        {
            // Arrange
            var beginPropertyName = "BeginDateTime";
            var endDateTime = new DateTime(2020, 1, 2, 0, 0, 0);
            var model = new DateTimeAfterAttributeTestModel()
            {
                BeginDateTime = new DateTime(2020, 1, 1, 0, 0, 0)
        };
            var context = new ValidationContext(model,null,null);
            var attribute = new DateTimeAfterAttribute(beginPropertyName);

            // Act
            var result = attribute.GetValidationResult(endDateTime, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void 開始時刻のPropertyが存在しない場合ArgumentException表示()
        {
            // Arrange
            var endDateTime = new DateTime(2020, 1, 2, 0, 0, 0);
            var model = new DateTimeAfterAttributeTestModel();
            var context = new ValidationContext(model, null, null);
            var attribute = new DateTimeAfterAttribute("test");

            // Assert
            var result = Assert.Throws<ArgumentException>(() =>
                { 
                    attribute.GetValidationResult(endDateTime, context);
                });
        }

        [Fact]
        public void 開始時刻のプロパティ値が存在しない場合検証しない()
        {
            // Arrange
            var beginPropertyName = "BeginDateTime";
            var model = new DateTimeAfterAttributeTestNullModel();
            var context = new ValidationContext(model, null, null);
            var attribute = new DateTimeAfterAttribute(beginPropertyName);
            // Act
            var result = attribute.GetValidationResult(string.Empty, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void 終了時刻の値が開始時刻の値より古い場合無効()
        {
            // Arrange
            var beginPropertyName = "BeginDateTime";
            var endDateTime = new DateTime(2020, 1, 1, 0, 0, 0);
            var model = new DateTimeAfterAttributeTestModel()
            { 
                BeginDateTime = new DateTime(2020, 1, 2, 0, 0, 0),
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateTimeAfterAttribute(beginPropertyName, "エラーテキスト");

            // Act
            var result = attribute.GetValidationResult(endDateTime, context);

            // Assert
            Assert.Equal("エラーテキスト", result.ErrorMessage);
        }

        [Fact]
        public void 開始時刻と終了時刻の値が同じ場合無効()
        {
            // Arrange
            var beginPropertyName = "BeginDateTime";
            var sameDateTime = new DateTime(2020, 1, 1, 0, 0, 0);
            var testModel = new DateTimeAfterAttributeTestModel() 
            {
                BeginDateTime = sameDateTime
            };
            var context = new ValidationContext(testModel, null, null);
            var attribute = new DateTimeAfterAttribute(beginPropertyName, "エラーテキスト");

            // Act
            var result = attribute.GetValidationResult(sameDateTime, context);

            // Assert
            Assert.Equal("エラーテキスト", result.ErrorMessage);
        }

        [Fact]
        public void AllowEquivalentがtrueの時開始時刻と終了時刻の値が同じ場合検証有効()
        {
            // Arrange
            var beginPropertyName = "BeginDateTime";
            var sameDateTime = new DateTime(2020, 1, 1, 0, 0, 0);
            var testModel = new DateTimeAfterAttributeTestModel()
            {
                BeginDateTime = sameDateTime
            };
            var context = new ValidationContext(testModel, null, null);
            var attribute = new DateTimeAfterAttribute(beginPropertyName, true,  "エラーテキスト");

            // Act
            var result = attribute.GetValidationResult(sameDateTime, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }
    }
}
