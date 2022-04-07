namespace GRHelper.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;
    using GRHelper.Web.ViewModels.Administration.Reservations;
    using Microsoft.EntityFrameworkCore;

    public class ReservationsService : IReservationsService
    {
        private readonly IDeletableEntityRepository<Reservation> reservations;
        private readonly IDeletableEntityRepository<Villa> villas;

        public ReservationsService(
            IDeletableEntityRepository<Reservation> reservations, 
            IDeletableEntityRepository<Villa> villas)
        {
            this.reservations = reservations;
            this.villas = villas;
        }

        public IEnumerable<T> All<T>()
        {
            return this.reservations.AllAsNoTracking()
                .Include(r => r.Villa)
                .Include(r => r.Requests)
                .AsQueryable()
                .To<T>()
                .ToList();
        }

        public async Task CreateAsync(CreateReservationInputModel input)
        {
            var reservation = new Reservation
            {
                From = input.From.AddHours(15),
                To = input.To.AddHours(12),
                Email = input.Email,
                AdultsCount = input.AdultsCount,
                ChildrenCount = input.ChildrenCount,
                VillaId = this.villas.AllAsNoTracking().FirstOrDefault(v => v.VillaNumber == input.Villa).Id,
                Password = HelperMethods.GeneratePassword(DataConstants.ResPasswordLength),
            };

            await this.reservations.AddAsync(reservation);
            await this.reservations.SaveChangesAsync();
        }

        public async Task<T> GetById<T>(int id)
        {
            var reservation = this.reservations.AllAsNoTracking().Where(r => r.Id == id).To<T>().FirstOrDefault();
            return reservation;
        }

        public int GetCount()
        {
            return this.reservations.AllAsNoTracking().Count();
        }
    }
}
