using JuniorTennis.Domain.Announcements;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly JuniorTennisDbContext context;

        public AnnouncementRepository(JuniorTennisDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Announcement>> Find() =>
            await this.context.Announcements.ToListAsync();

        public async Task<Announcement> FindById(int id)
        {
            var announcements = await this.Find();
            return announcements.FirstOrDefault(o => o.Id == id);
        }

        public async Task<Announcement> Update(Announcement announcement)
        {
            this.context.Update(announcement);
            await this.context.SaveChangesAsync();
            return announcement;
        }

        public async Task<Announcement> Add(Announcement announcement)
        {
            await this.context.AddAsync(announcement);
            await this.context.SaveChangesAsync();
            return announcement;
        }
    }
}
