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
                ReservationNumber = input.ReservationNumber,
                Email = input.Email,
                Name = input.Name,
                AdultsCount = input.AdultsCount,
                ChildrenCount = input.ChildrenCount,
                VillaId = this.villas.AllAsNoTracking().FirstOrDefault(v => v.VillaNumber == input.Villa).Id,
                Password = HelperMethods.GeneratePassword(DataConstants.ResPasswordLength),
            };

            await this.reservations.AddAsync(reservation);
            await this.reservations.SaveChangesAsync();
        }

        public async Task EditAsync(EditReservationInputModel input)
        {
            var reservation = this.reservations.All().FirstOrDefault(r => r.Id == input.Id);
            if (reservation == null)
            {
                throw new NullReferenceException("Reservation does not exist");
            }

            reservation.From = input.From.AddHours(15);
            reservation.To = input.To.AddHours(12);
            reservation.Email = input.Email;
            reservation.Name = input.Name;
            reservation.AdultsCount = input.AdultsCount;
            reservation.ChildrenCount = input.ChildrenCount;
            reservation.VillaId = this.villas.AllAsNoTracking().FirstOrDefault(v => v.VillaNumber == input.VillaNumber).Id;
            reservation.ReservationNumber = input.ReservationNumber;

            this.reservations.Update(reservation);
            await this.reservations.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = this.reservations.All().FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                throw new NullReferenceException("Reservation does not exist");
            }

            this.reservations.Delete(reservation);
            await this.reservations.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var reservation = this.reservations.AllAsNoTracking().Where(r => r.Id == id).To<T>().FirstOrDefault();
            return reservation;
        }

        public int GetCount()
        {
            return this.reservations.AllAsNoTracking().Count();
        }

        public IEnumerable<T> AllByGuestId<T>(string id)
        {
            var reservations = this.reservations.AllAsNoTracking()
                .Where(r => r.GuestId == id)
                .AsQueryable()
                .To<T>()
                .ToList();

            return reservations;
        }

        public IEnumerable<T> GetUnlocked<T>(string email)
        {
            var reservations = this.reservations.AllAsNoTracking()
                .Where(r => r.Email == email && r.Unlocked == false)
                .AsQueryable()
                .To<T>()
                .ToList();

            return reservations;
        }

        public bool Unlock(int id, string password)
        {
            var reservation = this.reservations.All().FirstOrDefault(r => r.Id == id && r.Password == password);
            if (reservation == null)
            {
                return false;
            }

            reservation.Unlocked = true;
            this.reservations.SaveChangesAsync().GetAwaiter().GetResult();
            return true;
        }
    }
}
