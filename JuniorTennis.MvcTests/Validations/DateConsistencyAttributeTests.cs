using JuniorTennis.Mvc.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace JuniorTennis.MvcTests.Validations
{
    public class DateConsistencyAttributeTests
    {
        [Fact]
        public void 指定した年のプロパティが存在しない場合例外()
        {
            // Arrange
            var day = 1;
            var model = new DateConsistencyAttributeTestModel()
            {
                Year = 2020,
                Month = 4
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateConsistencyAttribute("DifferentYear","Month");

            // Assert
            var result = Assert.Throws<InvalidOperationException>(() =>
            {
                attribute.GetValidationResult(day, context);
            });
            Assert.Equal("指定したプロパティ名が見つかりません。Name:DifferentYear", result.Message);
        }

        [Fact]
        public void 指定した月のプロパティが存在しない場合例外()
        {
            // Arrange
            var day = 1;
            var model = new DateConsistencyAttributeTestModel()
            {
                Year = 2020,
                Month = 4
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateConsistencyAttribute("Year", "DifferentMonth");

            // Assert
            var result = Assert.Throws<InvalidOperationException>(() =>
            {
                attribute.GetValidationResult(day, context);
            });
            Assert.Equal("指定したプロパティ名が見つかりません。Name:DifferentMonth", result.Message);
        }

        [Fact]
        public void 指定した年のプロパティの値がNullだった場合検証しない()
        {
            // Arrange
            var day = 1;
            var model = new DateConsistencyAttributeTestNullModel()
            {
                Year = null,
                Month = 4
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateConsistencyAttribute("Year", "Month");

            // Act
            var result = attribute.GetValidationResult(day, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void 指定した月のプロパティの値がNullだった場合検証しない()
        {
            // Arrange
            var day = 1;
            var model = new DateConsistencyAttributeTestNullModel()
            {
                Year = 2020,
                Month = null
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateConsistencyAttribute("Year", "Month");

            // Act
            var result = attribute.GetValidationResult(day, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void 検証対象の日がNullだった場合検証しない()
        {
            // Arrange
            var day = 1;
            var model = new DateConsistencyAttributeTestNullModel()
            {
                Year = 2020,
                Month = 4
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateConsistencyAttribute("Year", "Month");

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(2020, 4, 1)]
        [InlineData(2020, 8, 31)]
        [InlineData(2020, 2, 29)]
        [InlineData(1900, 2, 28)]
        public void 年月日が正しい場合検証成功(int year, int month, int day)
        {
            var model = new DateConsistencyAttributeTestModel()
            {
                Year = year,
                Month = month,
                Day = day
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateConsistencyAttribute("Year", "Month");

            // Act
            var result = attribute.GetValidationResult(day, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(2020, 4, 31)]
        [InlineData(1900, 2, 29)]
        [InlineData(2020, 2, 30)]
        public void 年月日が正しくない場合検証無効(int year, int month, int day)
        {
            var model = new DateConsistencyAttributeTestModel()
            {
                Year = year,
                Month = month,
                Day = day
            };
            var context = new ValidationContext(model, null, null);
            var attribute = new DateConsistencyAttribute("Year", "Month", "エラーテキスト");

            // Act
            var result = attribute.GetValidationResult(day, context);

            // Assert           
            Assert.Equal("エラーテキスト", result.ErrorMessage);
        }
    }
}
