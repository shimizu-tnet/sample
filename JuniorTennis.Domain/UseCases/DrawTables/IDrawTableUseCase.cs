using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.DrawTables
{
    /// <summary>
    /// ドロー表管理。
    /// </summary>
    public interface IDrawTableUseCase
    {
        /// <summary>
        /// ドロー表一覧を取得します。
        /// </summary>
        /// <returns>ドロー表一覧。</returns>
        Task<IEnumerable<DrawTable>> GetDrawTables();

        /// <summary>
        /// 選手情報一覧を含むドロー表を取得します。
        /// </summary>
        /// <param name="dto">ドロー表リポジトリ DTO。</param>
        /// <param name="asNoTracking">追跡なしフラグ。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> GetDrawTable(DrawTableRepositoryDto dto, bool asNoTracking = false);

        /// <summary>
        /// 関連データを含まないドロー表を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> GetDrawTableWithoutRelevantData(int tournamentId, string tennisEventId);

        /// <summary>
        /// ドロー表が既に作成済みかどうかを判定します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>ドロー表が既に作成済みの場合は true。それ以外の場合は false。</returns>
        Task<bool> ExistsDrawTable(int tournamentId, string tennisEventId);

        /// <summary>
        /// ドロー表を登録します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> RegisterDrawTable(DrawTable drawTable);

        /// <summary>
        /// ドロー表を更新します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> UpdateDrawTable(DrawTable drawTable);

        /// <summary>
        /// ドロー表を削除します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        Task DeleteDrawTable(DrawTable drawTable);

        /// <summary>
        /// 選手情報の一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>選手情報の一覧。</returns>
        Task<List<EntryDetail>> RetrievePlayers(int tournamentId, string tennisEventId);

        /// <summary>
        /// ドロー表の編集を開始します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="tournamentFromatId">大会形式 ID。</param>
        /// <returns>Task。</returns>
        Task StartEditingTheDrawTable(int tournamentId, string tennisEventId, int tournamentFromatId);

        /// <summary>
        /// 出場対象選手の種別を更新します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <param name="eligiblePlayersType">出場対象選手の種別。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> UpdateEligiblePlayersType(DrawTable drawTable, EligiblePlayersType eligiblePlayersType);

        /// <summary>
        /// JSON 文字列に変換された選手情報の一覧を取得します。
        /// </summary>
        /// <param name="tournament">大会。</param>
        /// <param name="drawTable">ドロー表。</param>
        /// <param name="eligiblePlayersType">出場対象選手の種別。</param>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>JSON 文字列に変換された選手情報の一覧。</returns>
        string GetEntryDetailsJson(
            Tournament tournament,
            DrawTable drawTable,
            EligiblePlayersType eligiblePlayersType,
            ParticipationClassification participationClassification);

        /// <summary>
        /// JSON 文字列に変換されたドロー設定を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>JSON 文字列に変換されたドロー設定。</returns>
        Task<string> GetDrawSettingsJson(int tournamentId, string tennisEventId, int participationClassificationId);

        /// <summary>
        /// JSON 文字列に変換されたドロー設定を取得します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>JSON 文字列に変換されたドロー設定。</returns>
        string GetDrawSettingsJson(DrawTable drawTable, ParticipationClassification participationClassification);

        /// <summary>
        /// JSON 文字列に変換された試合日一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>JSON 文字列に変換された試合日一覧。</returns>
        Task<string> GetGameDatesJson(int tournamentId, string tennisEventId);

        /// <summary>
        /// JSON 文字列に変換された試合日一覧を取得します。
        /// </summary>
        /// <param name="tournament">大会。</param>
        /// <param name="drawTable">ドロー表。</param>
        /// <returns>JSON 文字列に変換された試合日一覧。</returns>
        string GetGameDatesJson(Tournament tournament, DrawTable drawTable);

        /// <summary>
        /// JSON 文字列に変換された空きドロー一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>JSON 文字列に変換された空きドロー一覧。</returns>
        Task<string> GetBlankDrawsJson(int tournamentId, string tennisEventId);

        /// <summary>
        /// JSON 文字列に変換された同団体戦一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>JSON 文字列に変換された同団体戦一覧。</returns>
        Task<string> GetGameOfSameTeamsJson(int tournamentId, string tennisEventId);

        /// <summary>
        /// 試合日を更新します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="gameDates">試合日一覧。</param>
        /// <returns>Task。</returns>
        Task UpdateGameDates(int tournamentId, string tennisEventId, IEnumerable<(int blockNumber, DateTime gameDate)> gameDates);

        /// <summary>
        /// 試合日を更新します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <param name="gameDates">試合日一覧。</param>
        /// <returns>Task。</returns>
        Task UpdateGameDates(DrawTable drawTable, IEnumerable<(BlockNumber blockNumber, GameDate gameDate)> gameDates);

        /// <summary>
        /// 大会申込者の件数を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>大会申込者の件数。</returns>
        Task<int> GetNumberOfEntries(int tournamentId, string tennisEventId);

        /// <summary>
        /// 本戦のドロー表の抽選を行います。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>Task。</returns>
        Task ExecuteDrawingMain(int tournamentId, string tennisEventId);

        /// <summary>
        /// 予選のドロー表の抽選を行います。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>Task。</returns>
        Task ExecuteDrawingQualifying(int tournamentId, string tennisEventId);

        /// <summary>
        /// JSON 文字列に変換されたブロック一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <returns>JSON 文字列に変換されたブロック一覧。</returns>
        Task<string> GetBlocksJson(int tournamentId, string tennisEventId, int participationClassificationId, int? blockNumber = null);

        /// <summary>
        /// JSON 文字列に変換されたブロック一覧を取得します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <returns>JSON 文字列に変換されたブロック一覧。</returns>
        string GetBlocksJson(DrawTable drawTable, ParticipationClassification participationClassification, BlockNumber blockNumber);

        /// <summary>
        /// ドロー表を初期化します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>Task。</returns>
        Task InitializeDrawTable(int tournamentId, string tennisEventId, int participationClassificationId);

        /// <summary>
        /// シード位置の自動設定を行います。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>Task。</returns>
        Task ExecuteSeedFrameSetting(int tournamentId, string tennisEventId, int participationClassificationId);

        /// <summary>
        /// 割当済みのシード位置を全て削除します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>Task。</returns>
        Task ExecuteSeedFrameRemove(int tournamentId, string tennisEventId, int participationClassificationId);

        /// <summary>
        /// BYE 位置の自動設定を行います。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>Task。</returns>
        Task ExecuteByeFrameSetting(int tournamentId, string tennisEventId, int participationClassificationId);

        /// <summary>
        /// 割当済みの BYE 位置を全て削除します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>Task。</returns>
        Task ExecuteByeFrameRemove(int tournamentId, string tennisEventId, int participationClassificationId);

        /// <summary>
        /// 予選勝者を取り込みます。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>Task。</returns>
        Task IntakeQualifyingWinners(int tournamentId, string tennisEventId);

        /// <summary>
        /// 抽選結果を取り消します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>Task。</returns>
        Task CancelDrawing(int tournamentId, string tennisEventId, int participationClassificationId);

        /// <summary>
        /// 抽選結果を取り消します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>Task。</returns>
        void CancelDrawing(DrawTable drawTable, ParticipationClassification participationClassification);

        /// <summary>
        /// JSON 文字列に変換された試合結果一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <returns>JSON 文字列に変換されたブロック一覧。</returns>
        Task<string> GetGemeResultsJson(int tournamentId, string tennisEventId, int participationClassificationId, int blockNumber);

        /// <summary>
        /// 試合結果を更新します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <param name="gameNumber">試合番号。</param>
        /// <param name="gameResult">試合結果。</param>
        /// <returns>Task。</returns>
        Task UpdateGameStatus(int tournamentId, string tennisEventId, BlockNumber blockNumber, GameNumber gameNumber, GameResult gameResult);

        /// <summary>
        /// ドロー枠の選手区分を変更します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <param name="drawNumber">ドロー番号。</param>
        /// <param name="playerClassificationId">選手区分 ID。</param>
        /// <returns>Task。</returns>
        Task DrawFramePlayerClassificationChange(
            int tournamentId,
            string tennisEventId,
            int blockNumber,
            int drawNumber,
            int playerClassificationId);

        /// <summary>
        /// ドロー枠の選手割り当てを解除します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <param name="drawNumber">ドロー番号。</param>
        /// <returns>Task。</returns>
        Task UnassignPlayer(int tournamentId, string tennisEventId, int blockNumber, int drawNumber);

        /// <summary>
        /// ドローに対し、選手を個別に割り当てます。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="fromEntryNumber">割り当てる側の選手とエントリー番号。</param>
        /// <param name="toBlockNumber">割り当てられる側のブロック番号。</param>
        /// <param name="toDrawNumber">割り当てられる側のドロー番号。</param>
        /// <returns>Task。</returns>
        Task AssignPlayersToDraw(int tournamentId, string tennisEventId, int fromEntryNumber, int toBlockNumber, int toDrawNumber);

        /// <summary>
        /// ドロー表を登録します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>Task。</returns>
        Task UpdateToDraft(int tournamentId, string tennisEventId);

        /// <summary>
        /// ドロー表を公開します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>Task。</returns>
        Task UpdateToPublish(int tournamentId, string tennisEventId);
    }
}
