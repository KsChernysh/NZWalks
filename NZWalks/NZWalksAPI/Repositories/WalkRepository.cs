using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDbContext;

        public WalkRepository(NZWalksDBContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            //assign a new id
            walk.Id = Guid.NewGuid();

            //add walk to DB
            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var exwalk = await nZWalksDbContext.Walk.FindAsync(id);  
            if(exwalk == null)
            {
                return null;
            }
             nZWalksDbContext.Walk.Remove(exwalk);
            await nZWalksDbContext.SaveChangesAsync();
            return exwalk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                nZWalksDbContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
                
        }

        public Task<Walk> GetAsync(Guid Id)
        {
          return  nZWalksDbContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == Id);
                

        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var exwalk = await nZWalksDbContext.Walk.FindAsync(id);
            if(exwalk != null)
            {
                exwalk.WalkDifficultyId = walk.WalkDifficultyId;
                exwalk.RegionId = walk.RegionId;
                exwalk.Name = walk.Name;
                exwalk.Length = walk.Length;
              await  nZWalksDbContext.SaveChangesAsync();
                return exwalk;
            }
            return null;


        }
    }
}
