using JuniorTennis.Domain.DrawTables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class DrawTableRepository : IDrawTableRepository
    {
        private readonly JuniorTennisDbContext context;
        public DrawTableRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<List<DrawTable>> FindAllAsync()
        {
            return await this.context.DrawTables
                .Include(o => o.EntryDetails)
                .Include(o => o.QualifyingDrawSettings)
                .Include(o => o.MainDrawSettings)
                .Include(o => o.Blocks)
                    .ThenInclude(o => o.Games)
                        .ThenInclude(o => o.GameResult)
                .Include(o => o.Blocks)
                    .ThenInclude(o => o.Games)
                        .ThenInclude(o => o.Opponents)
                .ToListAsync();
        }

        public async Task<DrawTable> FindByDtoAsync(DrawTableRepositoryDto dto, bool asNoTracking = false)
        {
            // AsNoTracking については
            // https://docs.microsoft.com/ja-jp/ef/core/querying/tracking
            var drawTables = asNoTracking
                ? this.context.DrawTables.AsNoTracking()
                : this.context.DrawTables.AsTracking();

            drawTables = drawTables
                .Where(o => o.TournamentId == dto.TournamentId)
                .Where(o => o.TennisEventId == dto.TennisEventId)
                .Where(o => o.EditStatus == dto.EditStatus);

            if (dto.IncludeEntryDetails)
            {
                drawTables = drawTables.Include(o => o.EntryDetails);
            }

            if (dto.IncludeEntryPlayers)
            {
                drawTables = drawTables
                    .Include(o => o.EntryDetails)
                    .ThenInclude(o => o.EntryPlayers);
            }

            if (dto.IncludeQualifyingDrawSettings)
            {
                drawTables = drawTables.Include(o => o.QualifyingDrawSettings);
            }

            if (dto.IncludeMainDrawSettings)
            {
                drawTables = drawTables.Include(o => o.MainDrawSettings);
            }

            if (dto.IncludeGameResult)
            {
                drawTables = drawTables
                    .Include(o => o.Blocks)
                    .ThenInclude(o => o.Games)
                    .ThenInclude(o => o.GameResult);
            }

            if (dto.IncludeOpponents)
            {
                drawTables = drawTables
                    .Include(o => o.Blocks)
                    .ThenInclude(o => o.Games)
                    .ThenInclude(o => o.Opponents);
            }

            if (dto.IncludeGameResult || dto.IncludeOpponents)
            {
                return await drawTables.FirstOrDefaultAsync();
            }

            if (dto.IncludeGames)
            {
                drawTables = drawTables
                    .Include(o => o.Blocks)
                    .ThenInclude(o => o.Games);

                return await drawTables.FirstOrDefaultAsync();
            }

            if (dto.IncludeBlocks)
            {
                drawTables = drawTables.Include(o => o.Blocks);
            }

            return await drawTables.FirstOrDefaultAsync();
        }

        public async Task<DrawTable> AddAsync(DrawTable entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<DrawTable> UpdateAsync(DrawTable entity)
        {
            this.context.Update(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(DrawTable entity)
        {
            var entryDetails = this.context.EntryDetails.Where(o => o.DrawTableId == entity.Id);
            this.context.EntryDetails.RemoveRange(entryDetails);
            this.context.DrawTables.Remove(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByDtoAsync(DrawTableRepositoryDto dto)
        {
            return await this.context.DrawTables
                .Where(o => o.TournamentId == dto.TournamentId)
                .Where(o => o.TennisEventId == dto.TennisEventId)
                .Where(o => o.EditStatus == dto.EditStatus)
                .AnyAsync();
        }
    }
}
