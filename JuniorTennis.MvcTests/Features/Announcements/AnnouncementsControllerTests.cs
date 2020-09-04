using JuniorTennis.Domain.Announcements;
using JuniorTennis.Domain.UseCases.Announcements;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Mvc.Features.Announcements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Announcements
{
    public class AnnouncementsControllerTests
    {
        [Fact]
        public async void お知らせ一覧を表示()
        {
            // Arrange
            var pageIndex = 0;
            var displayCount = 30;
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var annoucements = new List<Announcement>()
            {
                new Announcement(
                    new AnnouncementTitle("お知らせタイトル"),
                    "本文",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020,4,1)),
                    new EndDate(new DateTime(2020,5,1)),
                    attachedFilePath: null),

                new Announcement(
                    new AnnouncementTitle("お知らせタイトル2"),
                    "本文",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020,4,1)),
                    new EndDate(new DateTime(2020,5,1)),
                    attachedFilePath: null),

                new Announcement(
                    new AnnouncementTitle("お知らせタイトル3"),
                    "本文",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020,4,1)),
                    new EndDate(new DateTime(2020,5,1)),
                    attachedFilePath: null)
            };

            var pagable = new Pagable<Announcement>(annoucements, pageIndex, 3, displayCount);
            mockUseCase.Setup(m => m.GetAnnouncements(pageIndex, displayCount))
                .ReturnsAsync(pagable)
                .Verifiable();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Index(pageIndex);

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Equal(0, model.Annoucements.PageIndex);
            Assert.Equal(3, model.Annoucements.Count);
        }

        [Fact]
        public async void ページ数がnullの場合1ページ目を表示()
        {
            // Arrange
            var displayCount = 30;
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var annoucements = new List<Announcement>()
            {
                new Announcement(
                    new AnnouncementTitle("お知らせタイトル"),
                    "本文",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020,4,1)),
                    new EndDate(new DateTime(2020,5,1)),
                    attachedFilePath: null),

                new Announcement(
                    new AnnouncementTitle("お知らせタイトル2"),
                    "本文",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020,4,1)),
                    new EndDate(new DateTime(2020,5,1)),
                    attachedFilePath: null),

                new Announcement(
                    new AnnouncementTitle("お知らせタイトル3"),
                    "本文",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020,4,1)),
                    new EndDate(new DateTime(2020,5,1)),
                    attachedFilePath: null)
            };

            var pagable = new Pagable<Announcement>(annoucements, 1, 3, displayCount);
            mockUseCase.Setup(m => m.GetAnnouncements(0, displayCount))
                .ReturnsAsync(pagable)
                .Verifiable();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Index(null);

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Equal(0, model.Annoucements.PageIndex);
        }

        [Fact]
        public async void 存在しないページ数の場合空のお知らせとページングは最終ページを表示()
        {
            // Arrange
            var pageIndex = 100;
            var displayCount = 30;
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var annoucements = new List<Announcement>() { };
            var pagable = new Pagable<Announcement>(annoucements, pageIndex, 3, displayCount);
            mockUseCase.Setup(m => m.GetAnnouncements(pageIndex, displayCount))
                .ReturnsAsync(pagable)
                .Verifiable();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Index(pageIndex);

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Equal(0, model.Annoucements.PageIndex);
            Assert.Empty(model.Annoucements);
        }

        [Fact]
        public void お知らせ登録画面を表示()
        {
            // Arrange
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = controller.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<RegisterViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async void 添付ファイル無しのお知らせを登録()
        {
            // Arrange
            var viewModel = new RegisterViewModel()
            {
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                EndDate = new DateTime(2020, 5, 1),
                UploadFile = null
            };

            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("お知らせタイトル"),
                "本文",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 5, 1)),
                attachedFilePath: null);
            announcement.Id = id;
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            mockUseCase.Setup(m => m.RegisterAnnouncement("お知らせタイトル", "本文", AnnouncementGenre.News.Id, new DateTime(2020, 5, 1), null, null))
                .ReturnsAsync(announcement)
                .Verifiable();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Register(viewModel);

            // Assert
            mockUseCase.Verify();
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Edit), redirectToActionResult.ActionName);
        }

        [Fact]
        public async void 添付ファイル有りのお知らせを登録()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.Setup(f => f.OpenReadStream())
                .Returns(mockStream.Object)
                .Verifiable();
            mockIFormFile.Setup(f => f.FileName)
                .Returns("FileName")
                .Verifiable();
            var viewModel = new RegisterViewModel()
            {
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                EndDate = new DateTime(2020, 5, 1),
                UploadFile = mockIFormFile.Object
            };

            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("お知らせタイトル"),
                "本文",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 5, 1)),
                new AttachedFilePath("/attached/filePath"));

            announcement.Id = id;
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            mockUseCase.Setup(m => m.RegisterAnnouncement("お知らせタイトル", "本文", AnnouncementGenre.News.Id, new DateTime(2020, 5, 1), "FileName", mockStream.Object))
                .ReturnsAsync(announcement)
                .Verifiable();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Register(viewModel);

            // Assert
            mockIFormFile.Verify();
            mockUseCase.Verify();
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Edit), redirectToActionResult.ActionName);
        }

        [Fact]
        public async void お知らせ登録時無効な値の場合再表示()
        {
            // Arrange
            var viewModel = new RegisterViewModel()
            {
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                EndDate = null,
                UploadFile = null
            };

            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var controller = new AnnouncementsController(mockUseCase.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Register(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<RegisterViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.AnnouncementTitle, model.AnnouncementTitle);
            Assert.Equal(viewModel.Body, model.Body);
            Assert.Equal(viewModel.SelectedAnnouncementGenre, model.SelectedAnnouncementGenre);
            Assert.Equal(viewModel.EndDate, model.EndDate);
            Assert.Equal(viewModel.UploadFile, model.UploadFile);
        }

        [Fact]
        public async void お知らせ編集画面表示()
        {
            // Arrange
            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("お知らせタイトル"),
                "本文",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 5, 1)),
                attachedFilePath: null);
            announcement.Id = id;

            var mockUseCase = new Mock<IAnnouncementUseCase>();
            mockUseCase.Setup(m => m.GetAnnouncement(id))
                .ReturnsAsync(announcement)
                .Verifiable();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Edit(id);

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<EditViewModel>(viewResult.ViewData.Model);
            Assert.Equal(id, model.AnnouncementId);
            Assert.Equal("お知らせタイトル", model.AnnouncementTitle);
            Assert.Equal("本文", model.Body);
            Assert.Equal("1", model.SelectedAnnouncementGenre);
            Assert.Equal(new DateTime(2020, 4, 1), model.RegisteredDate);
            Assert.Equal(new DateTime(2020, 5, 1), model.EndDate);
            Assert.Null(model.AttachedFilePath);
        }

        [Fact]
        public async void 添付ファイル無しのお知らせを更新()
        {
            // Arrange
            var viewModel = new EditViewModel()
            {
                AnnouncementId = 100000,
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                EndDate = new DateTime?(new DateTime(2020, 5, 1)),
                UploadFile = null
            };

            var updateViewModel = new EditViewModel()
            {
                AnnouncementId = 100000,
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                RegisteredDate = new DateTime(2020, 4, 1),
                EndDate = new DateTime?(new DateTime(2020, 5, 1)),
                UploadFile = null
            };

            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("お知らせタイトル"),
                "本文",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 5, 1)),
                attachedFilePath: null);
            announcement.Id = id;

            var mockUseCase = new Mock<IAnnouncementUseCase>();
            mockUseCase.Setup(m => m.UpdateAnnouncement(100000, "お知らせタイトル", "本文", AnnouncementGenre.News.Id, new DateTime(2020, 5, 1), null, null))
                .ReturnsAsync(announcement)
                .Verifiable();

            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Edit(viewModel);

            // Assert
            mockUseCase.Verify();
            var viewResulut = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EditViewModel>(viewResulut.Model);
            Assert.Null(viewResulut.ViewName);
            Assert.Equal(updateViewModel.AnnouncementId, model.AnnouncementId);
            Assert.Equal(updateViewModel.AnnouncementTitle, model.AnnouncementTitle);
            Assert.Equal(updateViewModel.Body, model.Body);
            Assert.Equal("1", model.SelectedAnnouncementGenre);
            Assert.Equal(updateViewModel.RegisteredDate, model.RegisteredDate);
            Assert.Equal(updateViewModel.EndDate, model.EndDate);
            Assert.Equal(updateViewModel.UploadFile, model.UploadFile);
        }

        [Fact]
        public async void 添付ファイル有りのお知らせを更新()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.Setup(f => f.OpenReadStream())
                .Returns(mockStream.Object)
                .Verifiable();
            mockIFormFile.Setup(f => f.FileName)
                .Returns("FileName")
                .Verifiable();
            var viewModel = new EditViewModel()
            {
                AnnouncementId = 100000,
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                EndDate = new DateTime?(new DateTime(2020, 5, 1)),
                UploadFile = mockIFormFile.Object
            };

            var updateViewModel = new EditViewModel()
            {
                AnnouncementId = 100000,
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                RegisteredDate = new DateTime(2020, 4, 1),
                EndDate = new DateTime?(new DateTime(2020, 5, 1)),
                AttachedFilePath = "/attached/filePath"
            };

            var id = 100000;
            var announcement = new Announcement(
                new AnnouncementTitle("お知らせタイトル"),
                "本文",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 5, 1)),
                new AttachedFilePath("/attached/filePath"));
            announcement.Id = id;

            var mockUseCase = new Mock<IAnnouncementUseCase>();
            mockUseCase.Setup(m => m.UpdateAnnouncement(100000, "お知らせタイトル", "本文", AnnouncementGenre.News.Id, new DateTime(2020, 5, 1), "FileName", mockStream.Object))
                .ReturnsAsync(announcement)
                .Verifiable();

            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Edit(viewModel);

            // Assert
            mockIFormFile.Verify();
            mockUseCase.Verify();
            var viewResulut = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EditViewModel>(viewResulut.Model);
            Assert.Null(viewResulut.ViewName);
            Assert.Equal(updateViewModel.AnnouncementId, model.AnnouncementId);
            Assert.Equal(updateViewModel.AnnouncementTitle, model.AnnouncementTitle);
            Assert.Equal(updateViewModel.Body, model.Body);
            Assert.Equal("1", model.SelectedAnnouncementGenre);
            Assert.Equal(updateViewModel.RegisteredDate, model.RegisteredDate);
            Assert.Equal(updateViewModel.EndDate, model.EndDate);
            Assert.Equal(updateViewModel.AttachedFilePath, model.AttachedFilePath);
        }

        [Fact]
        public async void お知らせ更新時無効な値の場合再表示()
        {
            // Arrange
            var viewModel = new EditViewModel()
            {
                AnnouncementId = 100000,
                AnnouncementTitle = "お知らせタイトル",
                Body = "本文",
                SelectedAnnouncementGenre = "1",
                EndDate = new DateTime?(new DateTime(2020, 5, 1)),
                UploadFile = null
            };

            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var controller = new AnnouncementsController(mockUseCase.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Edit(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<EditViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.AnnouncementId, model.AnnouncementId);
            Assert.Equal(viewModel.AnnouncementTitle, model.AnnouncementTitle);
            Assert.Equal(viewModel.Body, model.Body);
            Assert.Equal(viewModel.SelectedAnnouncementGenre, model.SelectedAnnouncementGenre);
            Assert.Equal(viewModel.EndDate, model.EndDate);
            Assert.Equal(viewModel.UploadFile, model.UploadFile);
        }

        [Fact]
        public async void お知らせを削除()
        {
            // Arrange
            var id = 100000;
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            mockUseCase.Verify(m => m.DeleteAnnouncement(id), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public async void 添付ファイルを削除()
        {
            // Arrange
            var announcementId = "100000";
            var id = int.Parse(announcementId);
            var mockUseCase = new Mock<IAnnouncementUseCase>();
            var controller = new AnnouncementsController(mockUseCase.Object);

            // Act
            await controller.PostDeleteAttachedFile(announcementId);

            mockUseCase.Verify(m => m.DeleteAttachedFile(id), Times.Once());
        }
    }
}
