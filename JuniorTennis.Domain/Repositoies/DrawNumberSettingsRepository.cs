using JuniorTennis.Domain.DrawTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace JuniorTennis.Domain.Repositoies
{
    /// <summary>
    /// ドロー枠初期設定情報リポジトリ。
    /// </summary>
    public class DrawNumberSettingsRepository
    {
        /// <summary>
        /// ドロー枠初期設定情報一覧。
        /// </summary>
        private static readonly IReadOnlyCollection<DrawNumberSettingsDto> drawNumberSettings;

        private static readonly int[] drawNumbers = { 4, 8, 16, 32, 64, 128, 256, 512, 1024 };

        static DrawNumberSettingsRepository()
        {
            var drawNumberSettings = drawNumbers.SelectMany(o =>
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @$"Data\drawtable\drawNumbers_{o:0000}.csv");
                var csv = File.ReadAllLines(path, Encoding.UTF8);
                var list = csv
                    .Select(p => p.Split(','))
                    .Select(p => new DrawNumberSettingsDto()
                    {
                        NumberOfDraws = o,
                        DrawNumber = int.Parse(p[0]),
                        PlayerClassificationId = int.Parse(p[1]),
                        SeedLevel = int.Parse(p[2]),
                        AssignOrder = int.Parse(p[3]),
                    });

                return list;
            }).ToList();

            DrawNumberSettingsRepository.drawNumberSettings = new Collection<DrawNumberSettingsDto>(drawNumberSettings);
        }

        /// <summary>
        /// 指定されたドロー数のドロー枠初期設定情報一覧を取得します。
        /// </summary>
        /// <returns>ドロー枠初期設定情報一覧。</returns>
        public static IList<DrawNumberSettingsDto> FindByNumberOfDraws(int numberOfDraws)
        {
            if (!drawNumbers.Any(o => o == numberOfDraws))
            {
                throw new ArgumentException("不正なドロー数です。");
            }

            return drawNumberSettings
                .Where(o => o.NumberOfDraws == numberOfDraws)
                .ToList();
        }
    }
}
