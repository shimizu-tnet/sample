using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.Tournaments
{
    /// <summary>
    /// 大会一覧ビューモデル。
    /// </summary>
    public class CreateViewModel
    {
        /// <summary>
        /// 大会一覧を取得します。
        /// </summary>
        [Display(Name = "大会一覧")]
        public readonly ObservableCollection<DisplayTournament> Tournaments;

        /// <summary>
        /// 大会一覧ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournaments">大会一覧。</param>
        public CreateViewModel(ObservableCollection<DisplayTournament> tournaments) => this.Tournaments = tournaments;
    }
}
