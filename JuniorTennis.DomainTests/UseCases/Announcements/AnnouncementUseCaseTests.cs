using JuniorTennis.Domain.Announcements;
using JuniorTennis.Domain.Externals;
using JuniorTennis.Domain.UseCases.Announcements;
using JuniorTennis.SeedWork;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases.Announcements
{
    public class AnnouncementUseCaseTests
    {
        [Fact]
        public async void お知らせの一覧を登録日の降順で取得()
        {
            // Arrange
            var pageIndex = 0;
            var displayCounts = 5;
            var announcement1 = new Announcement(
                    new AnnouncementTitle("大会のお知らせ1"),
                    "<h3>○○大会のおしらせ</h3>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 1)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                );

            var announcement2 = new Announcement(
                    new AnnouncementTitle("大会のお知らせ2"),
                    "<h3>○○大会のおしらせ</h3>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 2)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                );

            var announcement3 = new Announcement(
                    new AnnouncementTitle("大会のお知らせ3"),
                    "<h3>○○大会のおしらせ</h3>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 3)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                );

            var announcement4 = new Announcement(
                    new AnnouncementTitle("大会のお知らせ4"),
                    "<h3>○○大会のおしらせ</h3>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 4)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                );

            var announcement5 = new Announcement(
                    new AnnouncementTitle("大会のお知らせ5"),
                    "<h3>○○大会のおしらせ</h3>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 5)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                );

            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.Find())
                .ReturnsAsync(new List<Announcement>() { announcement1, announcement2, announcement3, announcement4, announcement5 })
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var act = await usecase.GetAnnouncements(pageIndex, displayCounts);

            // Assert
            mockRepository.Verify();
            Assert.Equal(5, act.List.Count());
            Assert.Equal(new DateTime(2020, 4, 5), act.List.First().RegisteredDate.Value);
            Assert.Equal(new DateTime(2020, 4, 4), act.List.Skip(1).First().RegisteredDate.Value);
            Assert.Equal(new DateTime(2020, 4, 3), act.List.Skip(2).First().RegisteredDate.Value);
            Assert.Equal(new DateTime(2020, 4, 2), act.List.Skip(3).First().RegisteredDate.Value);
            Assert.Equal(new DateTime(2020, 4, 1), act.List.Skip(4).First().RegisteredDate.Value);
            Assert.Equal(pageIndex, act.PageIndex);
            Assert.Equal(5, act.TotalCount);
            Assert.Equal(displayCounts, act.DisplayCount);
        }

        [Fact]
        public async void 最終ページを指定した場合表示できる件数のみ取得()
        {
            // Arrange
            var pageIndex = 3;
            var displayCounts = 30;
            var announcement = new Announcement(
                new AnnouncementTitle("大会のお知らせ"),
                "<h3>○○大会のおしらせ</h3>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                new AttachedFilePath("/attached/filePath")
                );

            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.Find())
                .ReturnsAsync(Enumerable.Range(0, 100).Select(o => announcement).ToList())
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var act = await usecase.GetAnnouncements(pageIndex, displayCounts);

            // Assert
            mockRepository.Verify();
            Assert.Equal(10, act.List.Count());
            Assert.Equal(pageIndex, act.PageIndex);
            Assert.Equal(100, act.TotalCount);
            Assert.Equal(displayCounts, act.DisplayCount);
        }

        [Fact]
        public async void 存在しないページ数を設定した場合0件取得()
        {
            // Arrange
            var pageIndex = 3;
            var displayCounts = 30;
            var announcement = new Announcement(
                new AnnouncementTitle("大会のお知らせ"),
                "<h3>○○大会のおしらせ</h3>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                 new AttachedFilePath("/attached/filePath")
                );

            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.Find())
                .ReturnsAsync(Enumerable.Range(0, 50).Select(o => announcement).ToList())
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            var act = await usecase.GetAnnouncements(pageIndex, displayCounts);

            // Assert
            Assert.Empty(act.List);
            mockRepository.Verify();
            mockRepository.VerifyNoOtherCalls();
            Assert.Equal(pageIndex, act.PageIndex);
            Assert.Equal(50, act.TotalCount);
            Assert.Equal(displayCounts, act.DisplayCount);
        }

        [Fact]
        public async void お知らせを取得()
        {
            var id = 100000;
            var announcement = new Announcement(
                    new AnnouncementTitle("大会のお知らせ1"),
                    "<h3>○○大会のおしらせ</h3>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 1)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                    );

            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var act = await usecase.GetAnnouncement(id);

            // Assert
            mockRepository.Verify();
            Assert.Equal(announcement.AnnounceTitle, act.AnnounceTitle);
            Assert.Equal(announcement.Body, act.Body);
            Assert.Equal(announcement.AnnouncementGenre, act.AnnouncementGenre);
            Assert.Equal(announcement.RegisteredDate, act.RegisteredDate);
            Assert.Equal(announcement.EndDate, act.EndDate);
            Assert.Equal(announcement.DeletedDateTime, act.DeletedDateTime);
            Assert.Equal(announcement.AttachedFilePath, act.AttachedFilePath);
        }

        [Fact]
        public async void Fileをアップロードしファイルパスを取得()
        {
            // Arrange
            var fileName = "UploadFile";
            var mockStream = new Mock<Stream>();
            var mockRepository = new Mock<IAnnouncementRepository>();
            var mockFileAccessor = new Mock<IFileAccessor>();
            mockFileAccessor.Setup(f => f.UploadAsync(fileName, mockStream.Object))
                .ReturnsAsync("/attached/filePath")
                .Verifiable();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var act = await usecase.UploadFile(fileName, mockStream.Object);

            // Assert
            Assert.Equal("/attached/filePath", act.ToString());
            mockRepository.Verify();
            mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Fileの削除を行いお知らせを更新()
        {
            // Arrange
            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("大会のお知らせ1"),
                "<h3>○○大会のおしらせ</h3>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                new AttachedFilePath("/attached/filePath")
                );
            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            await usecase.DeleteAttachedFile(id);

            // Assert
            mockRepository.Verify();
            mockRepository.Verify(u => u.Update(It.IsAny<Announcement>()), Times.Once());
            mockFileAccessor.Verify(u => u.DeleteAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async void 削除先のFilePathがnullの場合削除しない()
        {
            // Arrange
            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("大会のお知らせ1"),
                "<h3>○○大会のおしらせ</h3>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                null
                );
            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            await usecase.DeleteAttachedFile(id);

            // Assert
            mockRepository.Verify();
            mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void お知らせを登録する()
        {
            // Arrange
            var title = "大会のお知らせ";
            var body = "<h3>○○大会のおしらせ</h3>";
            var fileName = "UploadFile";
            var mockStream = new Mock<Stream>();
            var mockRepository = new Mock<IAnnouncementRepository>();

            var announcement = new Announcement(
                new AnnouncementTitle(title),
                body,
                AnnouncementGenre.News,
                new RegisteredDate(DateTime.Today),
                endDate: null,
                new AttachedFilePath("/attached/filePath"));

            mockRepository.Setup(u => u.Add(It.IsAny<Announcement>()))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            mockFileAccessor.Setup(f => f.UploadAsync(fileName, mockStream.Object))
                .ReturnsAsync("/attached/filePath")
                .Verifiable();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var result = await usecase.RegisterAnnouncement(title, body, AnnouncementGenre.News.Id, null, fileName, mockStream.Object);

            // Assert
            mockFileAccessor.Verify();
            mockRepository.Verify();
            Assert.Equal(announcement.AnnounceTitle, result.AnnounceTitle);
            Assert.Equal(announcement.Body, result.Body);
            Assert.Equal(announcement.AnnouncementGenre, result.AnnouncementGenre);
            Assert.Equal(announcement.RegisteredDate, result.RegisteredDate);
            Assert.Equal(announcement.EndDate, result.EndDate);
            Assert.Equal(announcement.DeletedDateTime, result.DeletedDateTime);
            Assert.Equal(announcement.AttachedFilePath, result.AttachedFilePath);
        }

        [Fact]
        public async void 添付ファイルの無いお知らせを登録する()
        {
            // Arrange
            var title = "大会のお知らせ";
            var body = "<h3>○○大会のおしらせ</h3>";
            var announcement = new Announcement(
                new AnnouncementTitle(title),
                body,
                AnnouncementGenre.News,
                new RegisteredDate(DateTime.Today),
                endDate: null,
                attachedFilePath: null);

            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(u => u.Add(It.IsAny<Announcement>()))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var result = await usecase.RegisterAnnouncement(title, body, AnnouncementGenre.News.Id, null, null, null);

            // Assert
            mockRepository.Verify();
            mockRepository.VerifyNoOtherCalls();
            Assert.Equal(announcement.AnnounceTitle, result.AnnounceTitle);
            Assert.Equal(announcement.Body, result.Body);
            Assert.Equal(announcement.RegisteredDate, result.RegisteredDate);
            Assert.Equal(announcement.EndDate, result.EndDate);
            Assert.Equal(announcement.DeletedDateTime, result.DeletedDateTime);
            Assert.Equal(announcement.AttachedFilePath, result.AttachedFilePath);
        }

        [Fact]
        public async void お知らせを更新する()
        {
            // Arrange
            var id = 100000;
            var title = "大会のお知らせ";
            var body = "<h3>○○大会のおしらせ</h3>";
            var endDate = new DateTime(2020, 4, 30);
            var fileName = "UploadFile";
            var announcement = new Announcement(
                new AnnouncementTitle(title),
                body,
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(endDate),
                null
                );
            announcement.Id = id;

            var mockStream = new Mock<Stream>();
            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(announcement)
                .Verifiable();
            mockRepository.Setup(r => r.Update(It.IsAny<Announcement>()))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            mockFileAccessor.Setup(f => f.UploadAsync(fileName, mockStream.Object))
                .ReturnsAsync("/attached/filePath")
                .Verifiable();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var result = await usecase.UpdateAnnouncement(id, title, body, AnnouncementGenre.News.Id, endDate, fileName, mockStream.Object);

            // Assert
            mockFileAccessor.Verify();
            mockRepository.Verify();
            Assert.Equal(announcement.Id, result.Id);
            Assert.Equal(announcement.AnnounceTitle, result.AnnounceTitle);
            Assert.Equal(announcement.Body, result.Body);
            Assert.Equal(announcement.AnnouncementGenre, result.AnnouncementGenre);
            Assert.Equal(announcement.RegisteredDate, result.RegisteredDate);
            Assert.Equal(announcement.EndDate, result.EndDate);
            Assert.Equal(announcement.DeletedDateTime, result.DeletedDateTime);
            Assert.Equal(announcement.AttachedFilePath, result.AttachedFilePath);
        }

        [Fact]
        public async void 添付ファイルの無いお知らせを更新する()
        {
            // Arrange
            var id = 100000;
            var title = "大会のお知らせ";
            var body = "<h3>○○大会のおしらせ</h3>";
            var endDate = new DateTime(2020, 4, 30);
            var announcement = new Announcement(
                new AnnouncementTitle(title),
                body,
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(endDate),
                attachedFilePath: null
                );
            announcement.Id = id;

            var mockStream = new Mock<Stream>();
            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(announcement)
                .Verifiable();
            mockRepository.Setup(r => r.Update(It.IsAny<Announcement>()))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            var result = await usecase.UpdateAnnouncement(id, title, body, AnnouncementGenre.News.Id, endDate, fileName: null, fileStream: null);

            // Assert
            mockFileAccessor.Verify();
            mockRepository.Verify();
            Assert.Equal(announcement.Id, result.Id);
            Assert.Equal(announcement.AnnounceTitle, result.AnnounceTitle);
            Assert.Equal(announcement.Body, result.Body);
            Assert.Equal(announcement.AnnouncementGenre, result.AnnouncementGenre);
            Assert.Equal(announcement.RegisteredDate, result.RegisteredDate);
            Assert.Equal(announcement.EndDate, result.EndDate);
            Assert.Equal(announcement.DeletedDateTime, result.DeletedDateTime);
            Assert.Equal(announcement.AttachedFilePath, result.AttachedFilePath);
        }

        [Fact]
        public async void お知らせを論理削除する()
        {
            // Arrange
            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("大会のお知らせ1"),
                "<h3>○○大会のおしらせ</h3>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                attachedFilePath: null
                );

            var mockRepository = new Mock<IAnnouncementRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(announcement)
                .Verifiable();
            var mockFileAccessor = new Mock<IFileAccessor>();
            var usecase = new AnnouncementUseCase(mockRepository.Object, mockFileAccessor.Object);

            // Act
            await usecase.DeleteAnnouncement(id);

            // Assert
            mockRepository.Verify();
            mockRepository.Verify(u => u.Update(It.IsAny<Announcement>()), Times.Once());
            Assert.NotNull(announcement.DeletedDateTime);
        }
    }
}
