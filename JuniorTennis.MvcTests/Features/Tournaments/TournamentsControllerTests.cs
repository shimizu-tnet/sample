using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.Tournaments;
using JuniorTennis.Mvc.Features.Tournaments;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Tournaments
{
    public class TournamentsControllerTests
    {
        [Fact]
        public async void 大会一覧を表示()
        {
            var mockUseCase = new Mock<ITournamentUseCase>();
            var tournaments = new List<Tournament>() {
                new Tournament(new TournamentName("大会０"), TournamentType.OnlyPoints, new RegistrationYear(DateTime.Now), TypeOfYear.Evem, new AggregationMonth(DateTime.Now), new List<TennisEvent>(){ new TennisEvent(Category.Under11Or12, Gender.Boys,Format.Doubles)  }),
                new Tournament(new TournamentName("大会１"), TournamentType.OnlyPoints, new RegistrationYear(DateTime.Now), TypeOfYear.Evem, new AggregationMonth(DateTime.Now), new List<TennisEvent>(){ new TennisEvent(Category.Under11Or12, Gender.Boys,Format.Doubles)  }),
                new Tournament(new TournamentName("大会２"), TournamentType.OnlyPoints, new RegistrationYear(DateTime.Now), TypeOfYear.Evem, new AggregationMonth(DateTime.Now), new List<TennisEvent>(){ new TennisEvent(Category.Under11Or12, Gender.Boys,Format.Doubles)  }),
                new Tournament(new TournamentName("大会３"), TournamentType.OnlyPoints, new RegistrationYear(DateTime.Now), TypeOfYear.Evem, new AggregationMonth(DateTime.Now), new List<TennisEvent>(){ new TennisEvent(Category.Under11Or12, Gender.Boys,Format.Doubles)  }),
                new Tournament(new TournamentName("大会４"), TournamentType.OnlyPoints, new RegistrationYear(DateTime.Now), TypeOfYear.Evem, new AggregationMonth(DateTime.Now), new List<TennisEvent>(){ new TennisEvent(Category.Under11Or12, Gender.Boys,Format.Doubles)  }),
            };

            mockUseCase.Setup(m => m.GetTournaments())
                .ReturnsAsync(tournaments)
                .Verifiable();
            var controller = new TournamentsController(mockUseCase.Object);

            var result = await controller.Index();

            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Equal(5, model.Tournaments.Count);
        }

        [Fact]
        public void 大会登録画面を表示()
        {
            // Arrange
            var mailTemplate = new Dictionary<string, string>()
            {
                { "PrePayment", "メール内容" },
                { "PostPayment", "メール内容" },
                { "NotRecieve", "メール内容" },
                { "Other", "メール内容" }
            };
            var mockUseCase = new Mock<ITournamentUseCase>();
            mockUseCase.Setup(m => m.GetTournamentEntryReceptionMailBodies())
                .Returns(mailTemplate)
                .Verifiable();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = controller.Register();

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RegisterViewModel>(viewResult.ViewData.Model);
            Assert.Equal(mailTemplate, model.TournamentEntryReceptionMailBodies);
        }

        [Fact]
        public async void 大会を登録()
        {
            // Arrange
            var viewModel = new RegisterViewModel()
            {
                TournamentName = "ジュニア選手権",
                SelectedTournamentType = TournamentType.WithDraw.Id.ToString(),
                SelectedRegistrationYear = "2020/4/1",
                SelectedTypeOfYear = TypeOfYear.Odd.Id.ToString(),
                SelectedAggregationMonth = "2020/6/1",
                SelectedTennisEvents = new List<string>() { "1_1_1", "1_1_2" },
                HoldingStartDate = new DateTime(2020, 6, 10),
                HoldingEndDate = new DateTime(2020, 6, 20),
                SelectedHoldingDates = new List<string>() { "2020/6/12", "2020/6/13" },
                Venue = "日本テニスコート",
                EntryFee = 100,
                SelectedMethodOfPayments = MethodOfPayment.PrePayment.Id.ToString(),
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手　権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール本文",
            };

            var mockUseCase = new Mock<ITournamentUseCase>();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = await controller.Register(viewModel);

            // Assert
            mockUseCase.Verify(m => m.RegisterTournament(It.IsAny<RegisterTournamentDto>()), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public async void 大会登録時無効な値の場合再表示()
        {
            // Arrange
            var holdingPeriodStartDate = new DateTime(2020, 5, 1);
            var holdingPeriodEndDate = new DateTime(2020, 5, 31);
            var viewModel = new RegisterViewModel()
            {
                TournamentName = "ジュニア選手権",
                SelectedTournamentType = TournamentType.WithDraw.Name,
                SelectedRegistrationYear = "2020/4/1",
                SelectedTypeOfYear = TypeOfYear.Odd.Name,
                SelectedAggregationMonth = "2020/6/1",
                SelectedTennisEvents = new List<string>() { "1_1_1", "1_1_2" },
                HoldingStartDate = holdingPeriodStartDate,
                HoldingEndDate = holdingPeriodEndDate,
                SelectedHoldingDates = new List<string>() { "2020/6/12", "2020/6/13" },
                Venue = "日本テニスコート",
                EntryFee = 100,
                SelectedMethodOfPayments = MethodOfPayment.PrePayment.Name,
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手　権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール本文",
            };

            var holdingDates = new List<JsonHoldingDate>() {
                new JsonHoldingDate(new DateTime(2020, 5, 1), true),
                new JsonHoldingDate(new DateTime(2020, 5, 31), true),
            };
            var mockUseCase = new Mock<ITournamentUseCase>();
            mockUseCase.Setup(m => m.CreateHoldingDates(holdingPeriodStartDate, holdingPeriodEndDate))
                .Returns(holdingDates)
                .Verifiable();
            var controller = new TournamentsController(mockUseCase.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Register(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<RegisterViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.TournamentName, model.TournamentName);
            Assert.Equal(viewModel.SelectedTournamentType, model.SelectedTournamentType);
            Assert.Equal(viewModel.SelectedRegistrationYear, model.SelectedRegistrationYear);
            Assert.Equal(viewModel.SelectedTypeOfYear, model.SelectedTypeOfYear);
            Assert.Equal(viewModel.SelectedAggregationMonth, model.SelectedAggregationMonth);
            Assert.Equal(viewModel.SelectedTennisEvents, model.SelectedTennisEvents);
            Assert.Equal(viewModel.HoldingStartDate, model.HoldingStartDate);
            Assert.Equal(viewModel.HoldingEndDate, model.HoldingEndDate);
            Assert.Equal(viewModel.SelectedHoldingDates, model.SelectedHoldingDates);
            Assert.Equal(viewModel.Venue, model.Venue);
            Assert.Equal(viewModel.EntryFee, model.EntryFee);
            Assert.Equal(viewModel.SelectedMethodOfPayments, model.SelectedMethodOfPayments);
            Assert.Equal(viewModel.ApplicationStartDate, model.ApplicationStartDate);
            Assert.Equal(viewModel.ApplicationEndDate, model.ApplicationEndDate);
            Assert.Equal(viewModel.Outline, model.Outline);
            Assert.Equal(viewModel.TournamentEntryReceptionMailSubject, model.TournamentEntryReceptionMailSubject);
            Assert.Equal(viewModel.TournamentEntryReceptionMailBody, model.TournamentEntryReceptionMailBody);
        }

        [Fact]
        public async void 大会の詳細画面を表示()
        {
            // Arrange
            var id = 000001;
            var holdingPeriodStartDate = new DateTime(2020, 6, 10);
            var holdingPeriodEndDate = new DateTime(2020, 6, 20);
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(holdingPeriodStartDate, holdingPeriodEndDate),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                1);

            var holdingDates = new List<JsonHoldingDate>() { new JsonHoldingDate(new DateTime(2020, 03, 30), true) };
            var mockUseCase = new Mock<ITournamentUseCase>();
            mockUseCase.Setup(m => m.GetTournament(id))
                .ReturnsAsync(tournament)
                .Verifiable();
            mockUseCase.Setup(m => m.CreateHoldingDates(holdingPeriodStartDate, holdingPeriodEndDate))
                .Returns(holdingDates)
                .Verifiable();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = await controller.Details(id);

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async void 大会の編集画面を表示()
        {
            // Arrange
            var id = 000001;
            var holdingPeriodStartDate = new DateTime(2020, 6, 10);
            var holdingPeriodEndDate = new DateTime(2020, 6, 20);
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(holdingPeriodStartDate, holdingPeriodEndDate),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                1);

            var mailTemplate = new Dictionary<string, string>()
            {
                { "PrePayment", "メール内容" },
                { "PostPayment", "メール内容" },
                { "NotRecieve", "メール内容" },
                { "Other", "メール内容" }
            };

            var holdingDates = new List<JsonHoldingDate>() { new JsonHoldingDate(new DateTime(2020, 03, 30), true) };
            var mockUseCase = new Mock<ITournamentUseCase>();
            mockUseCase.Setup(m => m.GetTournament(id))
                .ReturnsAsync(tournament)
                .Verifiable();
            mockUseCase.Setup(m => m.CreateHoldingDates(holdingPeriodStartDate, holdingPeriodEndDate))
                .Returns(holdingDates)
                .Verifiable();
            mockUseCase.Setup(m => m.GetTournamentEntryReceptionMailBodies())
                .Returns(mailTemplate)
                .Verifiable();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = await controller.Edit(id);

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EditViewModel>(viewResult.ViewData.Model);
            Assert.Equal(mailTemplate, model.TournamentEntryReceptionMailBodies);
        }

        [Fact]
        public async void 大会を更新する()
        {
            // Arrange
            var viewModel = new EditViewModel()
            {
                TournamentId = 000001,
                TournamentName = "ジュニア選手権",
                SelectedTournamentType = TournamentType.WithDraw.Id.ToString(),
                SelectedRegistrationYear = "2020/4/1",
                SelectedTypeOfYear = TypeOfYear.Odd.Id.ToString(),
                SelectedAggregationMonth = "2020/6/1",
                SelectedTennisEvents = new List<string>() { "1_1_1", "1_1_2" },
                HoldingStartDate = new DateTime(2020, 6, 10),
                HoldingEndDate = new DateTime(2020, 6, 20),
                SelectedHoldingDates = new List<string>() { "2020/6/12", "2020/6/13" },
                Venue = "日本テニスコート",
                EntryFee = 100,
                SelectedMethodOfPayments = MethodOfPayment.PrePayment.Id.ToString(),
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手　権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール本文",
            };

            var mockUseCase = new Mock<ITournamentUseCase>();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = await controller.Edit(viewModel);

            // Assert
            mockUseCase.Verify(m => m.UpdateTournament(It.IsAny<UpdateTournamentDto>()), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Details), redirectToActionResult.ActionName);
        }

        [Fact]
        public async void 大会編集時無効な値の場合再表示()
        {
            // Arrange
            var viewModel = new EditViewModel()
            {
                TournamentId = 000001,
                TournamentName = "ジュニア選手権",
                SelectedTournamentType = TournamentType.WithDraw.Id.ToString(),
                SelectedRegistrationYear = "2020/4/1",
                SelectedTypeOfYear = TypeOfYear.Odd.Id.ToString(),
                SelectedAggregationMonth = "2020/6/1",
                SelectedTennisEvents = new List<string>() { "1_1_1", "1_1_2" },
                HoldingStartDate = new DateTime(2020, 6, 10),
                HoldingEndDate = new DateTime(2020, 6, 20),
                SelectedHoldingDates = new List<string>() { "2020/6/12", "2020/6/13" },
                Venue = "日本テニスコート",
                EntryFee = 100,
                SelectedMethodOfPayments = MethodOfPayment.PrePayment.Id.ToString(),
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手　権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール 本文",
            };

            var mockUseCase = new Mock<ITournamentUseCase>();
            var controller = new TournamentsController(mockUseCase.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Edit(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<EditViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.TournamentId, model.TournamentId);
            Assert.Equal(viewModel.TournamentName, model.TournamentName);
            Assert.Equal(viewModel.SelectedTournamentType, model.SelectedTournamentType);
            Assert.Equal(viewModel.SelectedRegistrationYear, model.SelectedRegistrationYear);
            Assert.Equal(viewModel.SelectedTypeOfYear, model.SelectedTypeOfYear);
            Assert.Equal(viewModel.SelectedAggregationMonth, model.SelectedAggregationMonth);
            Assert.Equal(viewModel.SelectedTennisEvents, model.SelectedTennisEvents);
            Assert.Equal(viewModel.HoldingStartDate, model.HoldingStartDate);
            Assert.Equal(viewModel.HoldingEndDate, model.HoldingEndDate);
            Assert.Equal(viewModel.SelectedHoldingDates, model.SelectedHoldingDates);
            Assert.Equal(viewModel.Venue, model.Venue);
            Assert.Equal(viewModel.EntryFee, model.EntryFee);
            Assert.Equal(viewModel.SelectedMethodOfPayments, model.SelectedMethodOfPayments);
            Assert.Equal(viewModel.ApplicationStartDate, model.ApplicationStartDate);
            Assert.Equal(viewModel.ApplicationEndDate, model.ApplicationEndDate);
            Assert.Equal(viewModel.Outline, model.Outline);
            Assert.Equal(viewModel.TournamentEntryReceptionMailSubject, model.TournamentEntryReceptionMailSubject);
            Assert.Equal(viewModel.TournamentEntryReceptionMailBody, model.TournamentEntryReceptionMailBody);
        }

        [Fact]
        public async void 大会が削除される()
        {
            // Arrange
            var id = 000001;
            var mockUseCase = new Mock<ITournamentUseCase>();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            mockUseCase.Verify(m => m.DeleteTournament(It.IsAny<int>()), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public void 登録年度から集計月度一覧をJSON形式で取得する()
        {
            // Arrange
            var registrationYear = "2020";
            var mockUseCase = new Mock<ITournamentUseCase>();
            var controller = new TournamentsController(mockUseCase.Object);
            var registrationYears = @$"
                [
                    {{
                        'value':'2020/04/01',
                        'text':'2020\u5E744\u6708'
                    }},
                    {{
                        'value':'2020/05/01',
                        'text':'2020\u5E745\u6708'
                    }},
                    {{
                        'value':'2020/06/01',
                        'text':'2020\u5E746\u6708'
                    }},
                    {{
                        'value':'2020/07/01',
                        'text':'2020\u5E747\u6708'
                    }},
                    {{
                        'value':'2020/08/01',
                        'text':'2020\u5E748\u6708'
                    }},
                    {{
                        'value':'2020/09/01',
                        'text':'2020\u5E749\u6708'
                    }},
                    {{
                        'value':'2020/10/01',
                        'text':'2020\u5E7410\u6708'
                    }},
                    {{
                        'value':'2020/11/01',
                        'text':'2020\u5E7411\u6708'
                    }},
                    {{
                        'value':'2020/12/01',
                        'text':'2020\u5E7412\u6708'
                    }},
                    {{
                        'value':'2021/01/01',
                        'text':'2021\u5E741\u6708'
                    }},
                    {{
                        'value':'2021/02/01',
                        'text':'2021\u5E742\u6708'
                    }},
                    {{
                        'value':'2021/03/01',
                        'text':'2021\u5E743\u6708'
                    }},
                    {{
                        'value':'2021/04/01',
                        'text':'2021\u5E744\u6708'
                    }},
                    {{
                        'value':'2021/05/01',
                        'text':'2021\u5E745\u6708'
                    }},
                    {{
                        'value':'2021/06/01',
                        'text':'2021\u5E746\u6708'
                    }},

                    {{
                        'value':'2021/07/01',
                        'text':'2021\u5E747\u6708'
                    }}
                ]
            ";

            registrationYears = registrationYears.Replace("'", "\"");
            registrationYears = registrationYears.Replace(" ", "");
            registrationYears = registrationYears.Replace("\r\n", "");

            //Act
            var result = controller.GetAggregationMonths(registrationYear);

            // Assert
            Assert.Equal(registrationYears, result);
        }

        [Fact]
        public void 登録年度が空文字の場合空文字を返却する()
        {
            // Arrange
            var registrationYear = "";
            var mockUseCase = new Mock<ITournamentUseCase>();
            var controller = new TournamentsController(mockUseCase.Object);

            //Act
            var result = controller.GetAggregationMonths(registrationYear);

            // Arrange
            Assert.Equal("", result);
        }

        [Fact]
        public void 開催期間から開催日の一覧をJSON形式で取得する()
        {
            var holdingStartDate = "2020/4/1";
            var holdingEndDate = "2020/4/8";
            var jsonHoldingDates = new List<JsonHoldingDate>()
            {
                new JsonHoldingDate(new DateTime(2020, 03, 30) , true),
                new JsonHoldingDate(new DateTime(2020, 03, 31) , true),
                new JsonHoldingDate(new DateTime(2020, 04, 01) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 02) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 03) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 04) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 05) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 06) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 07) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 08) , false),
                new JsonHoldingDate(new DateTime(2020, 04, 09) , true),
                new JsonHoldingDate(new DateTime(2020, 04, 10) , true),
                new JsonHoldingDate(new DateTime(2020, 04, 11) , true),
                new JsonHoldingDate(new DateTime(2020, 04, 12) , true),
            };

            var holdingDates = @$"
                [
                    {{
                        'value':'2020/03/30',
                        'text':'3/30', 
                        'disabled':true
                    }}, 
                    {{ 
                        'value':'2020/03/31', 
                        'text':'3/31', 
                        'disabled':true
                    }}, 
                    {{ 
                        'value':'2020/04/01', 
                        'text':'4/1', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/02', 
                        'text':'4/2', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/03', 
                        'text':'4/3', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/04', 
                        'text':'4/4', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/05', 
                        'text':'4/5', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/06', 
                        'text':'4/6', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/07', 
                        'text':'4/7', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/08', 
                        'text':'4/8', 
                        'disabled':false
                    }}, 
                    {{ 
                        'value':'2020/04/09', 
                        'text':'4/9', 
                        'disabled':true
                    }}, 
                    {{ 
                        'value':'2020/04/10', 
                        'text':'4/10', 
                        'disabled':true
                    }}, 
                    {{ 
                        'value':'2020/04/11', 
                        'text':'4/11', 
                        'disabled':true
                    }}, 
                    {{ 
                        'value':'2020/04/12', 
                        'text':'4/12', 
                        'disabled':true
                    }} 
                ]";

            holdingDates = holdingDates.Replace("'", "\"");
            holdingDates = holdingDates.Replace(" ", "");
            holdingDates = holdingDates.Replace("\r\n", "");
            var mockUseCase = new Mock<ITournamentUseCase>();
            mockUseCase.Setup(m => m.CreateHoldingDates(DateTime.Parse(holdingStartDate), DateTime.Parse(holdingEndDate)))
                .Returns(jsonHoldingDates)
                .Verifiable();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = controller.GetHoldingDates(holdingStartDate, holdingEndDate);

            // Assert
            mockUseCase.Verify();
            Assert.Equal(holdingDates, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("2020/4/1", null)]
        [InlineData(null, "2020/4/8")]
        public void 開催期間の開始日終了日どちらかがNullの場合から文字を返却する(string holdingStartDate, string holdingEndDate)
        {
            // Arrange
            var mockUseCase = new Mock<ITournamentUseCase>();
            var controller = new TournamentsController(mockUseCase.Object);

            // Act
            var result = controller.GetHoldingDates(holdingStartDate, holdingEndDate);

            // Assert
            Assert.Equal("", result);
        }
    }
}
