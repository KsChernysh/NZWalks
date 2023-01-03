using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public RegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDBContext.AddAsync(region);
            await nZWalksDBContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
           var region = await nZWalksDBContext.Region.FirstOrDefaultAsync(x => x.Id == id);
           if(region == null)
            {
              return null;
            }
            //delete region
            nZWalksDBContext.Region.Remove(region);
           await nZWalksDBContext.SaveChangesAsync();
            return region;

        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
         return await nZWalksDBContext.Region.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDBContext.Region.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id,Region region)
        {

         var exregion = await  nZWalksDBContext.Region.FirstOrDefaultAsync(x => x.Id == id);
            if(exregion == null)
            {
                return null;
            }
            exregion.Code = region.Code;
            exregion.Name = region.Name;
            exregion.Population = region.Population;
            exregion.Area = region.Area;
            exregion.Lat = region.Lat;
            exregion.Long = region.Long;
            await nZWalksDBContext.SaveChangesAsync();
            return exregion;


        }
    }
}
