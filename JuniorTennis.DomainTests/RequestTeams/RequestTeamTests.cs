using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.SeedWork;
using System;
using Xunit;
using Moq;

namespace JuniorTennis.DomainTests.RequestTeams
{
    public class RequestTeamTests
    {
        [Fact]
        public void 登録団体情報を設定()
        {
            var now = DateTime.Now;
            var act = new RequestTeam(
                1,
                1,
                new ReservationNumber(),
                ApproveState.Unapproved,
                now,
                new RequestedFee(5000),
                null,
                MailState.Unsent
                );

            Assert.Equal(1, act.TeamId);
            Assert.Equal(1, act.SeasonId);
            Assert.Equal(new ReservationNumber(), act.ReservationNumber);
            Assert.Equal(ApproveState.Unapproved, act.ApproveState);
            Assert.Equal(now, act.RequestedDateTime);
            Assert.Equal(5000, act.RequestedFee.Value);
            Assert.Null(act.ApproveDateTime);
            Assert.Equal(MailState.Unsent, act.MailState);
        }
    }
}
