using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;
using Xunit;
using Moq;

namespace JuniorTennis.DomainTests.Players
{
    public class PlayersTests
    {
        [Fact]
        public void 選手を設定()
        {
            var date = new DateTime(2000, 8, 27);
            var act = new Player(
                teamId: 1,
                playerCode: new PlayerCode("2123456"),
                playerFamilyName: new PlayerFamilyName("大成"),
                playerFirstName: new PlayerFirstName("太郎"),
                playerFamilyNameKana: new PlayerFamilyNameKana("タイセイ"),
                playerFirstNameKana: new PlayerFirstNameKana("タロウ"),
                playerJpin: "jpin123",
                category: Category.Under11Or12,
                gender: Gender.Boys,
                birthDate: new BirthDate(date),
                telephoneNumber: "01-2345-6789"
                ); ;

            Assert.Equal(1, act.TeamId);
            Assert.Equal("2123456", act.PlayerCode.Value);
            Assert.Equal("大成", act.PlayerFamilyName.Value);
            Assert.Equal("太郎", act.PlayerFirstName.Value);
            Assert.Equal("タイセイ", act.PlayerFamilyNameKana.Value);
            Assert.Equal("タロウ", act.PlayerFirstNameKana.Value);
            Assert.Equal("jpin123", act.PlayerJpin);
            Assert.Equal(Category.Under11Or12, act.Category);
            Assert.Equal(Gender.Boys, act.Gender);
            Assert.Equal(date, act.BirthDate.Value);
            Assert.Equal("01-2345-6789", act.TelephoneNumber);
            Assert.Equal("大成 太郎", act.PlayerName.Value);
            Assert.Equal("タイセイ タロウ", act.PlayerNameKana.Value);
        }
    }
}
