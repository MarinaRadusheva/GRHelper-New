namespace GRHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Data;
    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Data.Repositories;
    using GRHelper.Services.Data.Tests.Models;
    using GRHelper.Services.Mapping;
    using GRHelper.Web.ViewModels.Administration.Requests;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class RequestsServiceTests
    {
        private ApplicationDbContext dbContext;
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        private IDeletableEntityRepository<Request> reqRepo;

        public RequestsServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.dbContext = new ApplicationDbContext(this.options.Options);
            this.reqRepo = new EfDeletableEntityRepository<Request>(this.dbContext);

            AutoMapperConfig.RegisterMappings(typeof(RequestTestModel).Assembly, typeof(Request).Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddRequestSuccessfully_WithValidData()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            var request = new CreateRequestInputModel()
            {
                Date = DateTime.UtcNow.AddDays(1),
                IsDaily = false,
                HotelServiceId = 1,
                GuestCount = 2,
                PaymentType = PaymentType.Free.ToString(),
                Time = DateTime.UtcNow.TimeOfDay,
                ReservationId = 1,
            };

            // Act
            await reqService.CreateAsync(request, null);

            // Assert
            Assert.NotEmpty(this.reqRepo.All().AsQueryable().ToList());
        }

        [Fact]
        public async void All_ShouldReturnListOfAllRequests_WnenAny()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(5, reqService);

            // Act
            var allCount = reqService.All<RequestTestModel>().ToList().Count();

            // Assert
            Assert.Equal(5, allCount);
        }

        [Fact]
        public void All_ShouldReturnEmptyList_WnenNoRequests()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);

            // Act
            var allCount = reqService.All<RequestTestModel>().ToList().Count();

            // Assert
            Assert.Equal(0, allCount);
        }

        [Theory]
        [InlineData("today", 1)]
        [InlineData("tomorrow", 2)]
        [InlineData("custom", 4)]
        public async void All_ShouldReturnCorrectList_WithDatePickerParam(string datePicker, int expectedCount)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(2, reqService);
            await this.AddPastTestRequests(2, reqService);
            List<StatusForRequestSearchModel> statuses = new List<StatusForRequestSearchModel>();
            var re = this.reqRepo.All().ToList();

            // Act
            var count = reqService.All<RequestTestModel>(datePicker, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10), statuses, null).Count();

            // Assert
            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("Waiting", 4)]
        [InlineData("Done", 1)]
        [InlineData("Cancelled", 1)]
        public async void All_ShouldReturnCorrectList_WithStatusParams(string status, int expectedResult)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(3, reqService);
            await this.AddPastTestRequests(3, reqService);
            await reqService.UpdateStatus("Done", 1);
            await reqService.UpdateStatus("Cancelled", 2);
            List<StatusForRequestSearchModel> statuses = new List<StatusForRequestSearchModel>();
            statuses.Add(new StatusForRequestSearchModel { DisplayName = status, EnumString = null, EnumValue = 0, Selected = true, });

            // Act
            var count = reqService.All<RequestTestModel>("custom", DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10), statuses, null).Count();

            // Assert
            Assert.Equal(expectedResult, count);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 0)]
        [InlineData(3, 2)]
        public async void All_ShouldReturnCorrectList_WithReservationParam(int reservationNumber, int expectedResult)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            IDeletableEntityRepository<Reservation> resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            await this.AddTestRequests(3, reqService);
            await this.AddPastTestRequests(3, reqService);
            await this.AddTestReservations(resRepo);
            List<StatusForRequestSearchModel> statuses = new List<StatusForRequestSearchModel>();

            var count = reqService.All<RequestTestModel>("custom", DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10), statuses, reservationNumber).Count();
            Assert.Equal(expectedResult, count);
        }

        [Fact]
        public async void All_ShouldReturnCorrectList_WithTodayDatePicker()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            IDeletableEntityRepository<Reservation> resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            await this.AddTestRequests(2, reqService);
            await this.AddPastTestRequests(2, reqService);
            await this.AddTestReservations(resRepo);
            List<StatusForRequestSearchModel> statuses = new List<StatusForRequestSearchModel>();
            var count = reqService.All<RequestTestModel>("today", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), statuses, null).Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async void DeleteAsync_ShouldDeleteRequest_WhenIdValid()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(2, reqService);

            // Act
            await reqService.DeleteAsync(1);
            int reqCount = this.reqRepo.All().AsEnumerable().Count();

            // Assert
            Assert.Equal(1, reqCount);
        }

        [Fact]
        public async void EditAsync_ShouldChangeRequestData_WhithValidData()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(1, reqService);
            var initial = this.reqRepo.AllAsNoTracking().FirstOrDefault(r => r.Id == 1);
            var edit = new EditRequestInputModel()
            {
                Id = 1,
                Date = DateTime.UtcNow.AddDays(3),
                IsDaily = true,
                GuestCount = 1,
                PaymentType = PaymentType.Free.ToString(),
                Time = DateTime.UtcNow.AddMinutes(5).TimeOfDay,
                ReservationId = 1,
            };
            DateTime? endDate = edit.Date.AddDays(2);

            // Act
            await reqService.EditAsync(edit, endDate);
            var edited = this.reqRepo.AllAsNoTracking().FirstOrDefault(r => r.Id == 1);

            // Assert
            Assert.NotEqual(initial.Date, edited.Date);
            Assert.NotEqual(initial.GuestCount, edited.GuestCount);
            Assert.NotEqual(initial.Time, edited.Time);
            Assert.NotEqual(initial.IsDaily, edited.IsDaily);
            Assert.NotEqual(initial.EndDate, edited.EndDate);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        public async void GetPending_ShouldReturnCorrectCountOfPendingRequests_IfAny(int count)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(count, reqService);

            // Act
            var pendingCount = reqService.GetPending<RequestTestModel>().Count();

            // Assert
            Assert.Equal(count, pendingCount);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(5, 1)]
        [InlineData(0, 0)]
        public async void AllByReservationId_ShouldReturnCorrectListOfRequests(int id, int count)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(5, reqService);

            // Act
            var reqCount = reqService.AllByReservationId<RequestTestModel>(id).Count();

            // Assert
            Assert.Equal(count, reqCount);
        }

        [Fact]
        public async void GetById_ShouldReturnOneRequest_WhenValidId()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(3, reqService);

            // Act
            var request = await reqService.GetById<RequestTestModel>(1);

            // Assert
            Assert.NotNull(request);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenInvalidId()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(3, reqService);

            // Act
            var request = await reqService.GetById<RequestTestModel>(10);

            // Assert
            Assert.Null(request);
        }

        [Theory]
        [InlineData("guest1", 2)]
        [InlineData("guest3", 1)]
        [InlineData("guest9", 0)]
        public async void AllByUserId_ShouldReturnListWithCorrectCount_WithActiveParam(string userId, int expectedResult)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            IDeletableEntityRepository<Reservation> resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            await this.AddTestRequests(3, reqService);
            await this.AddPastTestRequests(3, reqService);
            await this.AddTestReservations(resRepo);

            // Act / Assert
            Assert.Equal(expectedResult, reqService.AllByUserId<RequestTestModel>(userId, "Active").Count());

        }

        [Theory]
        [InlineData("guest1", 2)]
        [InlineData("guest3", 1)]
        [InlineData("guest9", 0)]
        public async void AllByUserId_ShouldReturnListWithCorrectCount_WithPassiveParam(string userId, int expectedResult)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            IDeletableEntityRepository<Reservation> resRepo = new EfDeletableEntityRepository<Reservation>(this.dbContext);
            await this.AddTestRequests(3, reqService);
            await this.AddPastTestRequests(3, reqService);
            await this.AddTestReservations(resRepo);

            // Act / Assert
            Assert.Equal(expectedResult, reqService.AllByUserId<RequestTestModel>(userId, "Passive").Count());
        }

        [Fact]
        public async void UserIsOwner_ShouldReturnTrue_WhenIsOwner()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            this.dbContext.Reservations.Add(new Reservation()
            {
                Id = 1,
                Number = 1,
                VillaId = 1,
                From = DateTime.Now.Date.AddDays(1),
                To = DateTime.Now.Date.AddDays(2),
                AdultsCount = 2,
                ChildrenCount = 1,
                Name = "Vasko1",
                Email = "Vasko@abv.bg",
                Requests = new List<Request>(),
                Villa = new Villa { Id = 1, Number = "R1", },
                Password = "123456",
                GuestId = "guest1",
            });
            await this.AddTestRequests(3, reqService);

            // Act
            var result = reqService.UserIsOwner(1, "guest1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void UserIsOwner_ShouldReturnFalse_WhenNotOwner()
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            this.dbContext.Reservations.Add(new Reservation()
            {
                Id = 1,
                Number = 1,
                VillaId = 1,
                From = DateTime.Now.Date.AddDays(1),
                To = DateTime.Now.Date.AddDays(2),
                AdultsCount = 2,
                ChildrenCount = 1,
                Name = "Vasko1",
                Email = "Vasko@abv.bg",
                Requests = new List<Request>(),
                Villa = new Villa { Id = 1, Number = "R1", },
                Password = "123456",
                GuestId = "guest1",
            });
            await this.AddTestRequests(3, reqService);

            // Act
            var result = reqService.UserIsOwner(1, "guest2");

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("InProgress")]
        [InlineData("Done")]
        [InlineData("Cancelled")]
        public async void UpdateStatus_ShouldChangeTheRequestStatus(string status)
        {
            // Arrange
            IRequestsService reqService = new RequestsService(this.reqRepo, null);
            await this.AddTestRequests(1, reqService);
            var currentStatus = this.reqRepo.AllAsNoTracking().FirstOrDefault(r => r.Id == 1);
            await reqService.UpdateStatus(status, 1);
            var updatetStatus = this.reqRepo.AllAsNoTracking().FirstOrDefault(r => r.Id == 1);
            Assert.NotEqual(currentStatus, updatetStatus);
        }

        private async Task AddTestRequests(int count, IRequestsService reqService)
        {
            for (int i = 1; i <= count; i++)
            {
                bool daily = false;
                DateTime? endDate = null;
                var reservationId = i;
                if (i % 2 == 0)
                {
                    daily = true;
                    endDate = DateTime.UtcNow.AddDays(i + 4);
                    reservationId = i - 1;
                }

                var request = new CreateRequestInputModel()
                {
                    Date = DateTime.UtcNow.AddDays(i),
                    IsDaily = daily,
                    HotelServiceId = i,
                    GuestCount = 2,
                    PaymentType = PaymentType.Free.ToString(),
                    Time = DateTime.UtcNow.TimeOfDay,
                    ReservationId = reservationId,
                };
                await reqService.CreateAsync(request, endDate);
            }
        }

        private async Task AddPastTestRequests(int count, IRequestsService reqService)
        {
            for (int i = 1; i <= count; i++)
            {
                bool daily = false;
                DateTime? endDate = null;
                var reservationId = i;
                if (i % 2 == 0)
                {
                    daily = true;
                    endDate = DateTime.UtcNow.AddDays(i + 4);
                    reservationId = i - 1;
                }

                var request = new CreateRequestInputModel()
                {
                    Date = DateTime.UtcNow.AddDays(i - 10),
                    IsDaily = daily,
                    HotelServiceId = i,
                    GuestCount = 2,
                    PaymentType = PaymentType.Free.ToString(),
                    Time = DateTime.UtcNow.TimeOfDay,
                    ReservationId = reservationId,
                };
                await reqService.CreateAsync(request, endDate);
            }
        }

        private async Task AddTestReservations(IDeletableEntityRepository<Reservation> resRepo)
        {
            for (int i = 1; i <= 4; i++)
            {
                var guestId = "guest" + i;
                var email = $"Vasko{i + 5}@abv.bg";
                if (i % 2 == 0)
                {
                    guestId = "guest9";
                    email = "goshko@abv.bg";
                }

                var res = new Reservation
                {
                    Id = i,
                    Number = i,
                    VillaId = i,
                    From = DateTime.Now.Date.AddDays(i),
                    To = DateTime.Now.Date.AddDays(i + 2),
                    AdultsCount = 2,
                    ChildrenCount = 1,
                    Name = $"Vasko{i}",
                    Email = email,
                    Requests = new List<Request>(),
                    Villa = new Villa { Id = i, Number = "R" + i, },
                    Password = "123456",
                    GuestId = guestId,
                };
                await resRepo.AddAsync(res);
            }

            await resRepo.SaveChangesAsync();
        }
    }
}
