namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using Microsoft.EntityFrameworkCore;

    public class RequestsService : IRequestsService
    {
        private readonly IDeletableEntityRepository<Request> requests;

        public RequestsService(IDeletableEntityRepository<Request> requests)
        {
            this.requests = requests;
        }

        public Task CreateAsync(CreateRequestInputModel input)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var request = this.requests.All().FirstOrDefault(r => r.Id == id);
            this.requests.Delete(request);
            await this.requests.SaveChangesAsync();
        }

        public Task EditAsync(EditRequestInputModel input)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> AllByReservationId<T>(int id)
        {
            return this.requests.AllAsNoTracking()
                .Where(r => r.ReservationId == id)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> AllByUserId<T>(string userId)
        {
            return this.requests.AllAsNoTracking()
                .Include(r => r.Reservation)
                .Include(r => r.HotelService)
                .Where(r => r.Reservation.GuestId == userId)
                .AsQueryable()
                .To<T>()
                .ToList();
        }

        public async Task<T> GetById<T>(int id)
        {
            return await this.requests.AllAsNoTracking()
                .Where(r => r.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
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
    }
}
