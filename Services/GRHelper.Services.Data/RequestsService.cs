namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Common;
    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Services.Data.Models;
    using GRHelper.Services.Mapping;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using GRHelper.Web.ViewModels.Guests.Reservations;
    using Microsoft.EntityFrameworkCore;

    public class RequestsService : IRequestsService
    {
        private readonly IDeletableEntityRepository<Request> requests;
        private readonly IDeletableEntityRepository<HotelService> hotelServices;

        public RequestsService(IDeletableEntityRepository<Request> requests, IDeletableEntityRepository<HotelService> hotelServices)
        {
            this.requests = requests;
            this.hotelServices = hotelServices;
        }

        public IEnumerable<T> All<T>()
        {
            return this.requests.AllAsNoTracking().To<T>().ToList();
        }

        public IEnumerable<T> All<T>(DateTime startDate, DateTime endDate)
        {
            return this.requests.AllAsNoTracking().Where(r => r.Date >= startDate && (r.EndDate != null ? r.EndDate <= endDate : r.Date <= endDate)).To<T>().ToList();
        }

        public async Task CreateAsync(CreateRequestInputModel input, DateTime? endDate)
        {
            decimal? price = null;
            if (input.PaymentType != "Free")
            {
                var hotelService = this.hotelServices.AllAsNoTracking().Include(s => s.SubCategory).FirstOrDefault(s => s.Id == input.HotelServiceId);
                if (hotelService.SubCategory.Name == "Transfer")
                {
                    price = hotelService.Price;
                }
                else
                {
                    price = input.GuestCount * hotelService.Price;
                }
            }

            var newRequest = new Request()
            {
                HotelServiceId = input.HotelServiceId,
                ReservationId = input.ReservationId,
                IsDaily = input.IsDaily,
                Date = input.Date,
                Time = input.Time,
                EndDate = endDate,
                GuestCount = input.GuestCount,
                Price = price,
                PaymentType = (PaymentType)Enum.Parse(typeof(PaymentType), input.PaymentType),
            };

            await this.requests.AddAsync(newRequest);
            await this.requests.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var request = this.requests.All().FirstOrDefault(r => r.Id == id);
            this.requests.Delete(request);
            await this.requests.SaveChangesAsync();
        }

        public async Task EditAsync(EditRequestInputModel input)
        {
            var requestToEdit = this.requests.All().FirstOrDefault(r => r.Id == input.Id);
            requestToEdit.IsDaily = input.IsDaily;
            requestToEdit.Date = input.Date;
            requestToEdit.Time = input.Time;
            requestToEdit.GuestCount = input.GuestCount;
            requestToEdit.PaymentType = (PaymentType)Enum.Parse(typeof(PaymentType), input.PaymentType);
            this.requests.Update(requestToEdit);
            await this.requests.SaveChangesAsync();
        }

        public IEnumerable<T> GetPending<T>()
        {
            return this.requests.AllAsNoTracking()
                .Where(r => (r.RequestStatus == RequestStatus.Waiting) && (r.Date >= DateTime.UtcNow.Date || (r.EndDate != null && r.EndDate >= DateTime.UtcNow.Date)))
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> AllByReservationId<T>(int id)
        {
            return this.requests.AllAsNoTracking()
                .Where(r => r.ReservationId == id)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> AllByUserId<T>(string userId, string showType)
        {
            var requests = this.requests.AllAsNoTracking()
                .Include(r => r.Reservation)
                .Include(r => r.HotelService)
                .Where(r => r.Reservation.GuestId == userId)
                .OrderByDescending(x => x.Date)
                .AsQueryable();
            if (showType == GlobalConstants.ArchivedRequestsRouteId)
            {
                return requests.Where(r => r.Date < DateTime.Now.Date || r.RequestStatus == RequestStatus.Cancelled)
                    .To<T>()
                    .ToList();
            }
            else
            {
                return requests.Where(r => r.Date >= DateTime.UtcNow.Date && r.RequestStatus != RequestStatus.Cancelled)
                    .To<T>()
                    .ToList();
            }
        }

        public async Task<T> GetById<T>(int id)
        {
            return await this.requests.AllAsNoTracking()
                .Where(r => r.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public bool UserIsOwner(int id, string userId)
        {
            return this.requests.AllAsNoTracking()
                .Include(r => r.Reservation)
                .Any(r => r.Id == id && r.Reservation.GuestId == userId);
        }

        public async Task UpdateStatus(string status, int id)
        {
            var request = this.requests.All().FirstOrDefault(r => r.Id == id);
            if (request == null)
            {
                throw new NullReferenceException("Request not found.");
            }

            request.RequestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), status);
            await this.requests.SaveChangesAsync();
        }

        public CreateRequestInputModel GenerateRequestModel(HotelServiceForRequestDto serviceInfo, List<ReservationForRequestDto> reservations)
        {
            var paymentTypesToDisplay = GetPaymentList(serviceInfo);
            return new CreateRequestInputModel()
            {
                Title = serviceInfo.DisplayName,
                Reservations = reservations.Select(x => new ReservationForRequestModel
                {
                    Id = x.Id,
                    From = x.From,
                    To = x.To,
                    Number = x.Number,
                    VillaNumber = x.VillaNumber,
                })
                .ToList(),
                HotelServiceId = serviceInfo.Id,
                PaymentTypes = paymentTypesToDisplay,
            };
        }

        public EditRequestInputModel GenerateEditModel(EditRequestInputModel model, HotelServiceForRequestDto serviceInfo)
        {
            var paymentTypesToDisplay = GetPaymentList(serviceInfo);
            model.PaymentTypes = paymentTypesToDisplay;
            model.Title = serviceInfo.DisplayName;
            return model;
        }

        private static List<PaymentTypeForRequest> GetPaymentList(HotelServiceForRequestDto serviceInfo)
        {
            var paymentTypesToDisplay = new List<PaymentTypeForRequest>();
            if (serviceInfo.Paid)
            {
                var paymentTypes = HelperMethods.GetPaymentTypes();
                foreach (var payType in paymentTypes)
                {
                    if (payType.ToString() != "Free")
                    {
                        paymentTypesToDisplay.Add(new PaymentTypeForRequest
                        {
                            DisplayName = HelperMethods.GetAttribute<DisplayAttribute>(payType).Name,
                            EnumValue = (int)payType,
                            EnumString = payType.ToString(),
                        });
                    }
                }
            }

            return paymentTypesToDisplay;
        }
    }
}
