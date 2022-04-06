using GRHelper.Data.Common;
using GRHelper.Data.Common.Repositories;
using GRHelper.Data.Models;
using GRHelper.Web.ViewModels.Administration.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRHelper.Services.Data
{
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

        public async Task CreateAsync(CreateReservationInputModel input)
        {
            var reservation = new Reservation
            {
                From = input.From,
                To = input.To,
                Email = input.Email,
                AdultsCount = input.AdultsCount,
                ChildrenCount = input.ChildrenCount,
                VillaId = this.villas.AllAsNoTracking().FirstOrDefault(v => v.VillaNumber == input.Villa).Id,
                Password = HelperMethods.GeneratePassword(DataConstants.ResPasswordLength),
            };

            await this.reservations.AddAsync(reservation);
            await this.reservations.SaveChangesAsync();
        }

        public int GetCount()
        {
            return this.reservations.AllAsNoTracking().Count();
        }
    }
}
