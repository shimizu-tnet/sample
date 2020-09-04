using JuniorTennis.Mvc.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace JuniorTennis.MvcTests.Validations
{
    public class RequiredWhenAttributeTests
    {
        [Fact]
        public void 対象プロパティの値が対象の値と違う場合検証しない()
        {
            // Arrange
            var targetPropertyName = "Target";
            var targetValues = "apple,orange,peach";
            var model = new RequiredWhenAttributeTestModel()
            {
                Target = "cherry",
            };

            var context = new ValidationContext(model, null, null);
            var attribute = new RequiredWhenAttribute(targetPropertyName, targetValues);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void 対象プロパティの値がNullの場合検証しない()
        {
            // Arrange
            var targetPropertyName = "Target";
            var targetValues = "apple,orange,peach";
            var model = new RequiredWhenAttributeTestModel()
            {
                Target = null,
            };

            var context = new ValidationContext(model, null, null);
            var attribute = new RequiredWhenAttribute(targetPropertyName, targetValues);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void 対象プロパティの値が対象の値と同じで検証対象がNullの場合検証無効()
        {
            // Arrange
            var targetPropertyName = "Target";
            var targetValues = "apple,orange,peach";
            var model = new RequiredWhenAttributeTestModel()
            {
                Target = "apple",
            };

            var context = new ValidationContext(model, null, null);
            var attribute = new RequiredWhenAttribute(targetPropertyName, targetValues, "エラーテキスト");

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal("エラーテキスト", result.ErrorMessage);
        }

        [Fact]
        public void 対象プロパティの値が対象の値と同じで検証対象が存在する場合検証有効()
        {
            // Arrange
            var targetPropertyName = "Target";
            var targetValues = "apple,orange,peach";
            var model = new RequiredWhenAttributeTestModel()
            {
                Target = "apple",
            };

            var context = new ValidationContext(model, null, null);
            var attribute = new RequiredWhenAttribute(targetPropertyName, targetValues);

            // Act
            var result = attribute.GetValidationResult("test", context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void 対象プロパティ値が存在しない場合例外()
        {
            // Arrange
            var targetPropertyName = "BeginDateTime";
            var targetValues = "apple,orange,peach";
            var model = new RequiredWhenAttributeTestModel();
            var context = new ValidationContext(model, null, null);
            var attribute = new RequiredWhenAttribute(targetPropertyName, targetValues);

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                attribute.GetValidationResult("test", context);
            });
        }
    }
}
