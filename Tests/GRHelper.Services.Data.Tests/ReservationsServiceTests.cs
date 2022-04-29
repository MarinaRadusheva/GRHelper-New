namespace GRHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using GRHelper.Data;
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Data.Repositories;
    using GRHelper.Services.Mapping;
    using GRHelper.Services.Messaging;
    using GRHelper.Web.ViewModels.Administration.Reservations;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ReservationsServiceTests
    {
        private ApplicationDbContext dbContext;
        //private EfDeletableEntityRepository<Reservation> resRepo;
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        //private IReservationsService resService;

        public ReservationsServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.dbContext = new ApplicationDbContext(this.options.Options);
            //this.resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            //this.resService = new ReservationsService(this.resRepo, null, null);

            AutoMapperConfig.RegisterMappings(typeof(ReservationListViewModel).Assembly, typeof(Reservation).Assembly);
        }

        public List<Villa> VillasTestData()
        {
            var villas = new List<Villa>();
            for (int i = 1; i <= 3; i++)
            {
                var villa = new Villa
                {
                    Id = i,
                    Number = "R" + i,
                };
                villas.Add(villa);
            }

            return villas;
        }

        public List<Reservation> ReservationsTestData()
        {
            var reservations = new List<Reservation>();
            for (int i = -4; i <= 2; i++)
            {
                var guestId = "guest" + i;
                var email = $"Vasko{i + 5}@abv.bg";
                if (i == 0 || i == 2)
                {
                    guestId = "guest9";
                    email = "goshko@abv.bg";
                }

                var res = new Reservation
                {
                    Id = i + 5,
                    Number = i + 5,
                    VillaId = i + 5,
                    From = DateTime.Now.Date.AddDays(i),
                    To = DateTime.Now.Date.AddDays(i + 2),
                    AdultsCount = 2,
                    ChildrenCount = 1,
                    Name = $"Vasko{i + 5}",
                    Email = email,
                    Requests = new List<Request>(),
                    Villa = new Villa { Id = i + 5, Number = "R" + i + 5, },
                    Password = "123456",
                    GuestId = guestId,
                };
                reservations.Add(res);
            }

            return reservations;
        }

        [Theory]
        [InlineData(true, 5)]
        [InlineData(false, 2)]
        public void All_GetsCorrectlyActiveOrArchivedReservations(bool active, int result)
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var actual = resService.All<ReservationListViewModel>(active, 1).ToList().Count;
            var expectedActive = result;

            // Assert
            Assert.Equal(expectedActive, actual);
        }

        [Fact]
        public void GetBySearchTerms_ShouldReturnCorrectRes_WithExistingNumber()
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var resNumber = resService.GetBySearchTerms<ReservationListViewModel>(1).FirstOrDefault().Number;

            // Assert
            Assert.Equal(1, resNumber);
        }

        [Fact]
        public void GetBySearchTerms_ShouldReturnEmptyList_WithNonExistingNumber()
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var resCount = resService.GetBySearchTerms<ReservationListViewModel>(10).ToList().Count();

            // Assert
            Assert.Equal(0, resCount);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddReservation_WithCorrectData_Successfully()
        {
            // Arrange
            var resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            var villaRepo = new Mock<IDeletableEntityRepository<Villa>>();
            villaRepo.Setup(r => r.AllAsNoTracking()).Returns(this.VillasTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo, villaRepo.Object, null);

            // Act
            await this.CreateOneTestReservation(resService);

            // Assert
            Assert.Single(resRepo.All().AsEnumerable());
        }

        //[Fact]
        //public async Task CreateAsyncShouldThrowErrorWitInvalidData()
        //{
        //    var resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
        //    var villaRepo = new Mock<IDeletableEntityRepository<Villa>>();
        //    villaRepo.Setup(r => r.AllAsNoTracking()).Returns(this.VillasTestData().AsQueryable());
        //    IReservationsService resService = new ReservationsService(resRepo, villaRepo.Object, null);
        //    var res = new CreateReservationInputModel
        //    {
        //        From = DateTime.UtcNow.Date.AddDays(-3),
        //        To = DateTime.UtcNow.Date.AddDays(-5),
        //        Number = 10,
        //        AdultsCount = 1,
        //        ChildrenCount = 1,
        //        Email = "vesko@abv.bg",
        //        Name = "Vesko Veskov",
        //        Villa = "R1",
        //    };

        //    await resService.CreateAsync(res);
        //    var tes = resRepo.All().AsEnumerable();
        //    Assert.Empty(resRepo.All().AsEnumerable());
        //}

        [Fact]
        public async Task EditAsync_ShouldChangeReservationData()
        {
            // Arrange
            var resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            var villaRepo = new Mock<IDeletableEntityRepository<Villa>>();
            villaRepo.Setup(r => r.AllAsNoTracking()).Returns(this.VillasTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo, villaRepo.Object, null);

            int createdId = await this.CreateOneTestReservation(resService);
            var res = await resRepo.AllAsNoTracking().FirstOrDefaultAsync(r => r.Id == createdId);

            // Act
            var editModel = new EditReservationInputModel
            {
                From = DateTime.UtcNow.Date.AddDays(3),
                To = DateTime.UtcNow.Date.AddDays(5),
                Number = 12,
                AdultsCount = 3,
                ChildrenCount = 0,
                Email = "pesho@abv.bg",
                Name = "Pesho",
                VillaNumber = "R2",
                Id = createdId,
            };
            await resService.EditAsync(editModel);
            var updated = await resRepo.AllAsNoTracking().FirstOrDefaultAsync(r => r.Id == createdId);

            // Assert
            Assert.NotEqual(res.From, updated.From);
            Assert.NotEqual(res.To, updated.To);
            Assert.NotEqual(res.Number, updated.Number);
            Assert.NotEqual(res.AdultsCount, updated.AdultsCount);
            Assert.NotEqual(res.ChildrenCount, updated.ChildrenCount);
            Assert.NotEqual(res.Email, updated.Email);
            Assert.NotEqual(res.Name, updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteReservation_Successfully()
        {
            // Arrange
            var resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            var villaRepo = new Mock<IDeletableEntityRepository<Villa>>();
            villaRepo.Setup(r => r.AllAsNoTracking()).Returns(this.VillasTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo, villaRepo.Object, null);

            // Act
            int createdId = await this.CreateOneTestReservation(resService);
            await resService.DeleteAsync(createdId);

            // Assert
            Assert.Empty(resRepo.All().ToList());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowEx_WhenIdIsInvalid()
        {
            // Arrange
            var resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            var villaRepo = new Mock<IDeletableEntityRepository<Villa>>();
            villaRepo.Setup(r => r.AllAsNoTracking()).Returns(this.VillasTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo, villaRepo.Object, null);

            // Act
            await this.CreateOneTestReservation(resService);

            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await resService.DeleteAsync(3));
        }

        [Theory]
        [InlineData(true, 5)]
        [InlineData(false, 2)]
        public void GetCount_ShouldReturnCorrectCount(bool active, int expectedCount)
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var actual = resService.GetCount(active);

            // Assert
            Assert.Equal(expectedCount, actual);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetCount_ShouldReturnZero_WhenNoResults(bool active)
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(new List<Reservation>().AsQueryable);
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var actual = resService.GetCount(active);

            // Assert
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData("guest1", 1)]
        [InlineData("guest9", 2)]
        [InlineData("guest10", 0)]
        public void AllByGuestId_ShouldReturnCorrectlyUsersReservations(string userId, int expectedCount)
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act / Assert
            Assert.Equal(expectedCount, resService.AllByGuestId<ReservationListViewModel>(userId).Count());
        }

        [Theory]
        [InlineData("Vasko6@abv.bg", 1)]
        [InlineData("goshko@abv.bg", 2)]
        [InlineData("guest10@abv.bg", 0)]
        public void GetUnlockedCount_ShouldReturnCorrectly_UsersUnlockedReservations(string email, int expectedCount)
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var actualCount = resService.GetUnlockedCount(email);

            // Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("Vasko6@abv.bg", 1)]
        [InlineData("goshko@abv.bg", 2)]
        [InlineData("guest10@abv.bg", 0)]
        public void GetUnlocked_ShouldReturnCorrectListOf_UsersUnlockedReservations(string email, int expectedCount)
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var actualCount = resService.GetUnlocked<ReservationListViewModel>(email).Count();

            // Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void Unlock_ShouldReturnTrue_WithValidData()
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.All()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            var result = resService.Unlock(1, "123456", "guest1");
            Assert.True(result);
        }

        [Theory]
        [InlineData(1, "123478", "guest1")]
        [InlineData(10, "123456", "guest1")]
        public void Unlock_ShouldReturnFalse_WithInValidData(int id, string password, string userId)
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.All()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            var result = resService.Unlock(id, password, userId);
            Assert.False(result);
        }

        [Fact]
        public void UserIsOwner_ShouldReturnTrue_WhenUserIsOwner()
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var result = resService.UserIsOwner(6, "guest1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UserIsOwner_ShouldReturnFalse_WhenUserIsNotOwner()
        {
            // Arrange
            var resRepo = new Mock<IDeletableEntityRepository<Reservation>>();
            resRepo.Setup(r => r.AllAsNoTracking()).Returns(this.ReservationsTestData().AsQueryable());
            IReservationsService resService = new ReservationsService(resRepo.Object, null, null);

            // Act
            var result = resService.UserIsOwner(7, "guest1");

            // Assert
            Assert.False(result);
        }

        private async Task<int> CreateOneTestReservation(IReservationsService resService)
        {
            var res = new CreateReservationInputModel
            {
                From = DateTime.UtcNow.Date,
                To = DateTime.UtcNow.Date.AddDays(2),
                Number = 10,
                AdultsCount = 1,
                ChildrenCount = 1,
                Email = "vesko@abv.bg",
                Name = "Vesko Veskov",
                Villa = "R1",
            };
            return await resService.CreateAsync(res);
        }
    }
}
