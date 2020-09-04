using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Externals;
using JuniorTennis.Domain.Operators;
using JuniorTennis.Domain.UseCases.Operators;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases.Operators
{
    public class OperatorUseCaseTests
    {
        [Fact]
        public async void 管理ユーザー一覧を取得()
        {
            // Arrange
            var operators = new List<Operator>
            {
                new Operator(
                    "管理太郎",
                    new EmailAddress("test@example.com"),
                    new LoginId("testloginid")
                    )
                    { Id = 1 },
                new Operator(
                    "管理太郎",
                    new EmailAddress("test@example.com"),
                    new LoginId("testloginid")
                    )
                    { Id = 2 },
                new Operator(
                    "管理太郎",
                    new EmailAddress("test@example.com"),
                    new LoginId("testloginid")
                    )
                    { Id = 3 },
                new Operator(
                    "管理太郎",
                    new EmailAddress("test@example.com"),
                    new LoginId("testloginid")
                    )
                    { Id = 4 },
            };
            var mockRepository = new Mock<IOperatorRepository>();
            mockRepository.Setup(r => r.FindAllAsync())
                .ReturnsAsync(operators)
                .Verifiable();
            var mockMailSender = new Mock<IMailSender>();
            var usecase = new OperatorUseCase(mockRepository.Object, mockMailSender.Object);

            // Act
            var act = await usecase.GetOperators();

            // Assert
            mockRepository.Verify();
            Assert.Equal(4, act.Count);
        }

        [Fact]
        public async void 管理ユーザーを登録する()
        {
            // Arrange
            var name = "管理太郎";
            var emailAddress = "test@example.com";
            var loginId = "testloginid";
            var registerOperator = new Operator(name, new EmailAddress(emailAddress), new LoginId(loginId));
            var mockRepository = new Mock<IOperatorRepository>();
            mockRepository.Setup(r => r.AddAsync(It.IsAny<Operator>()))
                .ReturnsAsync(registerOperator)
                .Verifiable();
            var mockMailSender = new Mock<IMailSender>();
            var usecase = new OperatorUseCase(mockRepository.Object, mockMailSender.Object);

            // Act
            var result = await usecase.RegisterOperator(name, emailAddress, loginId);

            // Assert
            mockRepository.Verify();
            Assert.Equal(registerOperator.Name, result.Name);
            Assert.Equal(registerOperator.EmailAddress.Value, result.EmailAddress.Value);
            Assert.Equal(registerOperator.LoginId.Value, result.LoginId.Value);
        }

        [Fact]
        public async void 管理ユーザーを取得する()
        {
            // Arrange
            var id = 100000;
            var getOperator = new Operator(
                "管理太郎",
                new EmailAddress("test@example.com"),
                new LoginId("testloginid")
                )
            { Id = id };

            var mockRepository = new Mock<IOperatorRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(id))
                .ReturnsAsync(getOperator)
                .Verifiable();
            var mockMailSender = new Mock<IMailSender>();
            var usecase = new OperatorUseCase(mockRepository.Object, mockMailSender.Object);

            // Act
            var result = await usecase.GetOperator(id);

            // Assert
            mockRepository.Verify();
            Assert.Equal(getOperator.Id, result.Id);
            Assert.Equal(getOperator.Name, result.Name);
            Assert.Equal(getOperator.EmailAddress, result.EmailAddress);
            Assert.Equal(getOperator.LoginId, result.LoginId);
        }

        [Fact]
        public async void 管理ユーザーを更新する()
        {
            // Arrange
            var id = 100000;
            var name = "管理太郎";
            var emailAddress = "test@example.com";
            var loginId = "testloginid";
            var updateOperator = new Operator(
                name,
                new EmailAddress(emailAddress),
                new LoginId(loginId)
                )
            { Id = id };

            var mockRepository = new Mock<IOperatorRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(id))
                .ReturnsAsync(updateOperator)
                .Verifiable();
            mockRepository.Setup(m => m.UpdateAsync(It.IsAny<Operator>()))
                .ReturnsAsync(updateOperator)
                .Verifiable();
            var mockMailSender = new Mock<IMailSender>();
            var usecase = new OperatorUseCase(mockRepository.Object, mockMailSender.Object);

            // Act
            var result = await usecase.UpdateOperator(id, name, emailAddress);

            // Assert
            mockRepository.Verify();
            Assert.Equal(updateOperator.Id, result.Id);
            Assert.Equal(updateOperator.Name, result.Name);
            Assert.Equal(updateOperator.EmailAddress, result.EmailAddress);
            Assert.Equal(updateOperator.LoginId, result.LoginId);
        }
    }
}
