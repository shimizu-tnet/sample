using JetBrains.Annotations;
using JuniorTennis.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace JuniorTennis.Infrastructure.DataBase
{
    public static class PropertyBuilderExtensions
    {
        /// <summary>
        /// スネークケースに変換したカラム名を設定します。
        /// </summary>
        /// <param name="value">PropertyBuilder。</param>
        /// <returns>PropertyBuilder。</returns>
        public static PropertyBuilder HasSnakeCaseColumnName([NotNull] this PropertyBuilder value) =>
            value.HasColumnName(ToSnakeCaseColumnName(value.GetColumnName()));

        /// <summary>
        /// スネークケースに変換したカラム名を設定します。
        /// </summary>
        /// <typeparam name="TProperty">プロパティの型。</typeparam>
        /// <param name="value">PropertyBuilder。</param>
        /// <returns>PropertyBuilder。</returns>
        public static PropertyBuilder<TProperty> HasSnakeCaseColumnName<TProperty>([NotNull] this PropertyBuilder<TProperty> value) =>
            value.HasColumnName(ToSnakeCaseColumnName(value.GetColumnName()));

        /// <summary>
        /// PropertyBuilder から DB に設定するカラム名を取得します。
        /// </summary>
        /// <param name="propertyBuilder">PropertyBuilder。</param>
        /// <returns>DB に設定するカラム名。</returns>
        public static string GetColumnName([NotNull] this PropertyBuilder propertyBuilder) =>
            propertyBuilder.Metadata.Name.ToLower() == "value"
                ? propertyBuilder.Metadata.DeclaringType.DisplayName()
                : propertyBuilder.Metadata.Name;

        /// <summary>
        /// DB保存時にId（数値）とEnumerationを変換します。
        /// </summary>
        /// <typeparam name="T">Enumeration継承クラス。</typeparam>
        /// <returns>PropertyBuilder。</returns>
        /// <returns>ValueConverter</returns>
        public static PropertyBuilder<T> HasEnumerationConversion<T>([NotNull] this PropertyBuilder<T> value) where T : Enumeration =>
            value.HasConversion(o => o.Id, o => Enumeration.FromValue<T>(o));

        /// <summary>
        /// カラム名をスネークケースに変換します。
        /// </summary>
        /// <param name="columnName">カラム名。</param>
        /// <returns>スネークケースに変換したカラム名。</returns>
        private static string ToSnakeCaseColumnName(string columnName) =>
            new Regex("[a-z][A-Z]")
                    .Replace(columnName, m => m.Groups[0].Value[0] + "_" + m.Groups[0].Value[1])
                    .ToLower();
    }
}
