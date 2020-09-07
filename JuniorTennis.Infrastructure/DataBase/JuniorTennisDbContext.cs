using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Announcements;
using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.Operators;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationNumber = JuniorTennis.Domain.ReservationNumbers.ReservationNumber;

namespace JuniorTennis.Infrastructure.DataBase
{
    public class JuniorTennisDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public JuniorTennisDbContext(DbContextOptions<JuniorTennisDbContext> options) : base(options) { }

        /// <summary>
        /// トーナメント。
        /// </summary>
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<HoldingDate> HoldingDates { get; set; }
        public DbSet<TennisEvent> TennisEvents { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TournamentEntry> TournamentEntries { get; set; }
        public DbSet<DrawTable> DrawTables { get; set; }
        public DbSet<DrawSettings> DrawSettings { get; set; }
        public DbSet<EntryDetail> EntryDetails { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AuthorizationLink> AuthorizationLinks { get; set; }
        public DbSet<ReservationNumber> ReservationNumbers { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<RequestTeam> RequestTeams { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<RequestPlayer> RequestPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        }

        public override int SaveChanges()
        {
            return this.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
