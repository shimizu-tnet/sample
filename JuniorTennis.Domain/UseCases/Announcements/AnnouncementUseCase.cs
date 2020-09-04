using JuniorTennis.Domain.Announcements;
using System.Threading.Tasks;
using System.Linq;
using JuniorTennis.Domain.Externals;
using System.IO;
using System;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.UseCases.Announcements
{
    public class AnnouncementUseCase : IAnnouncementUseCase
    {
        private readonly IAnnouncementRepository repository;

        private readonly IFileAccessor fileAccessor;

        public AnnouncementUseCase(
            IAnnouncementRepository repository,
            IFileAccessor fileAccessor)
        {
            this.repository = repository;
            this.fileAccessor = fileAccessor;
        }

        public async Task<Pagable<Announcement>> GetAnnouncements(int pageIndex, int displayCount)
        {
            var skipNumber = pageIndex * displayCount;
            var annoucements = await this.repository.Find();
            var totalCount = annoucements.Count();
            var displayAnouncements = annoucements
                .Where(o => o.DeletedDateTime == null)
                .OrderByDescending(o => o.RegisteredDate.Value)
                .Skip(skipNumber)
                .Take(displayCount)
                .ToList();

            return new Pagable<Announcement>(displayAnouncements, pageIndex, totalCount, displayCount);
        }

        public async Task<Announcement> GetAnnouncement(int id) => await this.repository.FindById(id);

        public async Task<Announcement> RegisterAnnouncement(string title, string body, int announcementGenre, DateTime? endDate, string fileName, Stream fileStream)
        {
            var announcement = new Announcement(
                new AnnouncementTitle(title),
                body,
                Enumeration.FromValue<AnnouncementGenre>(announcementGenre),
                new RegisteredDate(DateTime.Today),
                endDate.HasValue ? new EndDate(endDate.Value) : null,
                attachedFilePath: null);

            if (this.HasAttachment(fileName))
            {
                var filePath = await this.fileAccessor.UploadAsync(fileName, fileStream);
                announcement.ChangeAttachedFilePath(new AttachedFilePath(filePath));
            }

            return await this.repository.Add(announcement);
        }

        public async Task<Announcement> UpdateAnnouncement(int id, string title, string body, int announcementGenre, DateTime? endDate, string fileName, Stream fileStream)
        {
            var announcement = await repository.FindById(id);

            if (!announcement.HasAttachedFile && this.HasAttachment(fileName))
            {
                var newFilePath = await this.fileAccessor.UploadAsync(fileName, fileStream);
                announcement.ChangeAttachedFilePath(new AttachedFilePath(newFilePath));
            }

            announcement.Change(
                new AnnouncementTitle(title),
                body,
                Enumeration.FromValue<AnnouncementGenre>(announcementGenre),
                endDate.HasValue ? new EndDate(endDate.Value) : null
                );

            return await this.repository.Update(announcement);
        }

        public async Task DeleteAnnouncement(int id)
        {
            var announcement = await repository.FindById(id);
            announcement.Delete();
            await this.repository.Update(announcement);
        }

        public async Task<string> UploadFile(string fileName, Stream fileStream)
        {
            var filePath = await this.fileAccessor.UploadAsync(fileName, fileStream);
            return filePath;
        }

        public async Task DeleteAttachedFile(int id)
        {
            var announcement = await repository.FindById(id);
            if (!announcement.HasAttachedFile)
            {
                return;
            }

            await fileAccessor.DeleteAsync(announcement.AttachedFilePath.Value);
            announcement.DeleteAttachedFilePath();
            await this.repository.Update(announcement);
        }

        private bool HasAttachment(string fileName) => !string.IsNullOrWhiteSpace(fileName);
    }
}
