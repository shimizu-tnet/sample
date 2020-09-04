using JuniorTennis.Domain.UseCases.Announcements;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JuniorTennis.Mvc.Features.Announcements
{
    /// <summary>
    /// お知らせ一覧のコントローラ。
    /// </summary>
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementUseCase useCase;

        public AnnouncementsController(
            IAnnouncementUseCase useCase)
        {
            this.useCase = useCase;
        }

        /// <summary>
        /// お知らせ一覧を表示します。
        /// </summary>
        /// <param name="page">ページ番号。</param>
        /// <returns>お知らせ一覧画面。</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var displayCount = 30;
            var pageIndex = page ?? 0;
            var annoucements = await this.useCase.GetAnnouncements(pageIndex, displayCount);
            return this.View(new IndexViewModel(annoucements));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Register([Bind(
            "AnnouncementTitle",
            "Body",
            "SelectedAnnouncementGenre",
            "EndDate",
            "UploadFile")]
            RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var announcemet = await this.useCase.RegisterAnnouncement(
                model.AnnouncementTitle,
                model.Body,
                int.Parse(model.SelectedAnnouncementGenre),
                model.EndDate,
                model.AttachedFileName,
                model.AttachedFileStream);

            return this.RedirectToAction(nameof(this.Edit), new { id = announcemet.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var announcement = await this.useCase.GetAnnouncement(id);
            var viewModel = new EditViewModel(
                announcement.Id,
                announcement.AnnounceTitle.Value,
                announcement.Body,
                announcement.AnnouncementGenre.Id,
                announcement.RegisteredDate.Value,
                announcement.EndDate?.Value,
                announcement.AttachedFilePath?.Value
                );
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind(
            "AnnouncementId",
            "AnnouncementTitle",
            "Body",
            "SelectedAnnouncementGenre",
            "EndDate",
            "UploadFile"
            )]
            EditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var announcement = await this.useCase.UpdateAnnouncement(
                model.AnnouncementId,
                model.AnnouncementTitle,
                model.Body,
                int.Parse(model.SelectedAnnouncementGenre),
                model.EndDate,
                model.AttachedFileName,
                model.AttachedFileStream);

            var updateModel = new EditViewModel(
                announcement.Id,
                announcement.AnnounceTitle.Value,
                announcement.Body,
                announcement.AnnouncementGenre.Id,
                announcement.RegisteredDate.Value,
                announcement.EndDate?.Value,
                announcement.AttachedFilePath?.Value
                );

            return this.View(updateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.useCase.DeleteAnnouncement(id);
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [Route("Announcements/{announcementId}/delete_attached_filepath")]
        public async Task PostDeleteAttachedFile(string announcementId)
        {
            await this.useCase.DeleteAttachedFile(int.Parse(announcementId));
        }
    }
}
