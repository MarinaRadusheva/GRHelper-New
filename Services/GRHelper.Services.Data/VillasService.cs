namespace GRHelper.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;

    public class VillasService : IVillasService
    {
        private readonly IDeletableEntityRepository<Villa> villas;

        public VillasService(IDeletableEntityRepository<Villa> villas)
        {
            this.villas = villas;
        }

        public IEnumerable<string> GetVillaNumbers()
        {
            return this.villas.AllAsNoTracking().Select(v => v.Number).ToList();
        }
    }
}
