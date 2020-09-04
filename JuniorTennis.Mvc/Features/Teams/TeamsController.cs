using System.Linq;
using System.Threading.Tasks;
using JuniorTennis.Domain.UseCases.Teams;
using JuniorTennis.SeedWork.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using JuniorTennis.Infrastructure.Identity;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace JuniorTennis.Mvc.Features.Teams
{
    public class TeamsController : Controller
    {
        private readonly ITeamUseCase useCase;
        private readonly UserManager<ApplicationUser> userManager;

        public TeamsController(ITeamUseCase useCase,
            UserManager<ApplicationUser> userManager)
        {
            this.useCase = useCase;
            this.userManager = userManager;
        }

        /// <summary>
        /// 団体新規登録申請画面を表示します。
        /// </summary>
        /// <param name="authorizationCode">認証コード。</param>
        /// <returns>団体新規登録申請画面</returns>
        [HttpGet]
        public async Task<ActionResult> RequestTeam([FromQuery] string authorizationCode)
        {
            var authorizationLink = await this.useCase.GetAuthorizationLinkByCode(authorizationCode);
            var deserializedMailAddress = authorizationLink.UniqueKey;
            var model = new RequestTeamViewModel();
            model.RepresentativeEmailAddress = deserializedMailAddress;
            return this.View(model);
        }

        /// <summary>
        /// 団体を申請します。
        /// </summary>
        /// <param name="model">RequestViewModel。</param>
        /// <returns>トップ画面。</returns>
        [HttpPost]
        public async Task<ActionResult> RequestTeam([Bind(
            "TeamType",
            "TeamName",
            "TeamAbbreviatedName",
            "RepresentativeName",
            "RepresentativeEmailAddress",
            "TelephoneNumber",
            "Address",
            "CoachName",
            "CoachEmailAddress")]
            RequestTeamViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var normalizedMailAddress = userManager.NormalizeEmail(model.RepresentativeEmailAddress);
            var duplicatedMailAddress = await userManager.FindByEmailAsync(normalizedMailAddress);
            if (duplicatedMailAddress != null)
            {
                model.IsDuplicated = true;
                return this.View(model);
            }

            // AspNetUserへの登録
            var applicationUser = new ApplicationUser();
            applicationUser.UserName = model.RepresentativeEmailAddress;
            applicationUser.Email = model.RepresentativeEmailAddress;
            await this.userManager.CreateAsync(applicationUser);

            // teamsへの登録
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var dto = model.ToDto();
            await this.useCase.RequestTeamNewRegistration(dto);

            // 登録完了通知メールの送信
            await this.useCase.SendRequestTeamNewRegistrationMail(model.RepresentativeEmailAddress);

            return Redirect("/");
        }

        /// <summary>
        /// 団体編集画面を表示します。
        /// </summary>
        /// <param name="teamCode">団体コード。</param>
        /// <returns>団体編集画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(string teamCode)
        {
            try
            {
                var team = await this.useCase.GetTeam(teamCode);
                return this.View(EditViewModel.FromEntity(team));
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 団体を編集します。
        /// </summary>
        /// <param name="viewModel">団体編集viewModel。</param>
        /// <returns>団体編集画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(
            "TeamCode",
            "TeamName",
            "TeamAbbreviatedName",
            "RepresentativeName",
            "RepresentativeEmailAddress",
            "TelephoneNumber",
            "Address",
            "CoachName",
            "CoachEmailAddress",
            "OriginalMailAddress")]
            EditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // メールアドレスが変更されている場合、他のユーザーとの重複がないか確認
            if (model.IsMailAddressChanged)
            {
                var normalizedMailAddress = userManager.NormalizeEmail(model.RepresentativeEmailAddress);
                var duplicatedMailAddress = await userManager.FindByEmailAsync(normalizedMailAddress);
                if (duplicatedMailAddress != null)
                {
                    model.IsDuplicated = true;
                    return this.View(model);
                }
            }

            try
            {
                // teamsの更新
                var dto = model.ToDto();
                await this.useCase.UpdateTeam(dto);
                if (model.IsMailAddressChanged)
                {
                    // メールアドレスに変更がある場合、userの更新
                    var user = userManager.FindByEmailAsync(userManager.NormalizeEmail(model.OriginalMailAddress)).Result;
                    user.Email = model.RepresentativeEmailAddress;
                    await userManager.UpdateAsync(user);
                }

                return this.RedirectToAction(nameof(Edit), new { teamCode = model.TeamCode });
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 団体継続画面を表示します。
        /// </summary>
        /// <param name="teamId">団体Id。</param>
        /// <returns>団体継続画面。</returns>
        [HttpGet]
        public async Task<ActionResult> RequestContinuedTeam(int teamId)
        {
            try
            {
                var requestTeamState = await this.useCase.GetRequestTeamState(teamId);
                return this.View(RequestContinuedTeamViewModel.FromDto(requestTeamState));
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 団体継続を申請します。
        /// </summary>
        /// <param name="viewModel">団体継続viewModel。</param>
        /// <returns>団体継続画面。</returns>
        [HttpPost]
        public async Task<ActionResult> RequestContinuedTeam([Bind(
            "TeamId",
            "TeamType",
            "RequestedFee",
            "SeasonId")]
            RequestContinuedTeamViewModel model)
        {
            try
            {
                // RequestTeamの発行
                var dto = model.ToDto();
                await this.useCase.AddRequestTeam(dto);
                return this.RedirectToAction(nameof(RequestContinuedTeam), new { teamId = model.TeamId });
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
