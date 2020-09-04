using JuniorTennis.SeedWork;
using System;
using System.Text.Json;

namespace JuniorTennis.Domain.Utils
{
    /// <summary>
    /// JsonElement の値を変換する。
    /// </summary>
    public static class JsonConverter
    {
        /// <summary>
        /// JsonElement の値を整数に変換します。
        /// </summary>
        /// <param name="prop">JsonElement。</param>
        /// <returns>整数に変換された JsonElement。</returns>
        public static int ToInt32(JsonElement prop)
        {
            if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt32(out var value))
            {
                return value;
            }

            return 0;
        }

        /// <summary>
        /// JsonElement の値を null 許容型の整数に変換します。
        /// </summary>
        /// <param name="prop">JsonElement。</param>
        /// <returns>null 許容型の整数に変換された JsonElement。</returns>
        public static int? ToNullableInt32(JsonElement prop)
        {
            if (prop.ValueKind == JsonValueKind.Null)
            {
                return null;
            }

            if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt32(out var value))
            {
                return value;
            }

            return 0;
        }

        /// <summary>
        /// JsonElement の値を文字列に変換します。
        /// </summary>
        /// <param name="prop">JsonElement。</param>
        /// <returns>文字列に変換された JsonElement。</returns>
        public static string ToString(JsonElement prop)
        {
            if (prop.ValueKind == JsonValueKind.String)
            {
                return prop.GetString();
            }

            return string.Empty;
        }

        /// <summary>
        /// JsonElement の値を日付型に変換します。
        /// </summary>
        /// <param name="prop">JsonElement。</param>
        /// <returns>日付型に変換された JsonElement。</returns>
        public static DateTime ToDateTime(JsonElement prop)
        {
            if (prop.ValueKind == JsonValueKind.String && prop.TryGetDateTime(out var value))
            {
                return value;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// JsonElement の値を真偽値に変換します。
        /// </summary>
        /// <param name="prop">JsonElement。</param>
        /// <returns>真偽値に変換された JsonElement。</returns>
        public static bool ToBoolean(JsonElement prop)
        {
            if (prop.ValueKind == JsonValueKind.True || prop.ValueKind == JsonValueKind.False)
            {
                return prop.GetBoolean();
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// JsonElement の値を列挙型クラスに変換します。
        /// </summary>
        /// <typeparam name="T">列挙型クラスの Type。</typeparam>
        /// <param name="prop">JsonElement。</param>
        /// <returns>列挙型クラスに変換された JsonElement。</returns>
        public static T ToEnumeration<T>(JsonElement prop) where T : Enumeration
        {
            if (prop.ValueKind == JsonValueKind.Null)
            {
                return null;
            }

            return Enumeration.FromValue<T>(ToInt32(prop));
        }

        /// <summary>
        /// JsonElement内のメール本文を文字列に変換します。
        /// <param name="prop">JsonElement。</param>
        /// <returns>文字列に変換された JsonElement。</returns>
        /// </summary>
        public static string ToMailBodyString(JsonElement prop)
        {
            var body = JsonConverter.ToString(prop);
            return body.Replace("<br>", "\r\n");
        }
    }
}
