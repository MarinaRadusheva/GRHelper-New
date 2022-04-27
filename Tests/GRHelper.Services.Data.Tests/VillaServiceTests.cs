using GRHelper.Data.Common.Repositories;
using GRHelper.Data.Models;
using GRHelper.Data.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GRHelper.Services.Data.Tests
{
    public class VillaServiceTests
    {
        public List<Villa> TestData => new List<Villa>()
        {
            new Villa
            {
                Id = 1,
                Number = "R1",
            },
            new Villa
            {
                Id = 2,
                Number = "R2",
            },
            new Villa
            {
                Id = 3,
                Number = "R3",
            },
        };

        [Fact]
        public void GetAllVillasMustReturnAllVIllasCorrectly()
        {
            var repo = new Mock<IDeletableEntityRepository<Villa>>();
            repo.Setup(m => m.AllAsNoTracking()).Returns(this.TestData.AsQueryable());
            IVillasService villasService = new VillasService(repo.Object);
            var villas = villasService.GetVillaNumbers().ToList();
            var expectedResult = this.TestData.Select(x => x.Number).ToList();

            Assert.Equal(expectedResult, villas);
        }
    }
}
