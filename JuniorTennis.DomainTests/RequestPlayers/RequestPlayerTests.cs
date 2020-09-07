using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.SeedWork;
using System;
using Xunit;
using Moq;
using JuniorTennis.Domain.Tournaments;

namespace JuniorTennis.DomainTests.RequestPlayers
{
    public class RequestPlayerTests
    {
        [Fact]
        public void 登録選手情報を設定()
        {
            var now = DateTime.Now;
            var act = new RequestPlayer(
                playerId: 1,
                teamId: 1,
                seasonId : 1,
                reservationNumber: new ReservationNumber(),
                reservationBranchNumber: 1,
                category: Category.Under11Or12,
                requestType: RequestType.NewRegistration,
                approveState: ApproveState.Unapproved,
                now,
                new PlayerRegistrationFee(500),
                null
                );

            Assert.Equal(1, act.PlayerId);
            Assert.Equal(1, act.TeamId);
            Assert.Equal(1, act.SeasonId);
            Assert.Equal(new ReservationNumber(), act.ReservationNumber);
            Assert.Equal(Category.Under11Or12, act.Category);
            Assert.Equal(RequestType.NewRegistration, act.RequestType);
            Assert.Equal(ApproveState.Unapproved, act.ApproveState);
            Assert.Equal(now, act.RequestedDateTime);
            Assert.Equal(500, act.PlayerRegistrationFee.Value);
            Assert.Null(act.ApproveDateTime);
        }

        [Fact]
        public void 登録選手情報を受領にする()
        {
            var act = new RequestPlayer(
                playerId: 1,
                teamId: 1,
                seasonId: 1,
                reservationNumber: new ReservationNumber(),
                reservationBranchNumber: 1,
                category: Category.Under11Or12,
                requestType: RequestType.NewRegistration,
                approveState: ApproveState.Unapproved,
                DateTime.Now,
                new PlayerRegistrationFee(500),
                null
                );
            act.Approve();
            Assert.Equal(ApproveState.Approved, act.ApproveState);
        }

        [Fact]
        public void 登録選手情報を取消にする()
        {
            var act = new RequestPlayer(
                playerId: 1,
                teamId: 1,
                seasonId: 1,
                reservationNumber: new ReservationNumber(),
                reservationBranchNumber: 1,
                category: Category.Under11Or12,
                requestType: RequestType.NewRegistration,
                approveState: ApproveState.Approved,
                DateTime.Now,
                new PlayerRegistrationFee(500),
                null
                );
            act.Unapprove();
            Assert.Equal(ApproveState.Unapproved, act.ApproveState);
        }
    }
}
