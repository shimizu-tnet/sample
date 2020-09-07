namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ドロー枠初期設定情報 DTO。
    /// </summary>
    public class DrawNumberSettingsDto
    {
        /// <summary>
        /// ドロー数を取得または設定します。
        /// </summary>
        public int NumberOfDraws { get; set; }

        /// <summary>
        /// ドロー番号を取得または設定します。
        /// </summary>
        public int DrawNumber { get; set; }

        /// <summary>
        /// 選手区分 ID を取得または設定します。
        /// </summary>
        public int PlayerClassificationId { get; set; }

        /// <summary>
        /// シードレベルを取得または設定します。
        /// </summary>
        public int SeedLevel { get; set; }

        /// <summary>
        /// 割り当て順を取得または設定します。
        /// </summary>
        public int AssignOrder { get; set; }

        /// <summary>
        /// 対戦者インスタンスに変換します。
        /// </summary>
        /// <returns>対戦者。</returns>
        public Opponent ToOpponent()
        {
            return new Opponent(
                new DrawNumber(this.DrawNumber),
                new SeedLevel(this.SeedLevel),
                new AssignOrder(this.AssignOrder)
            );
        }
    }
}
