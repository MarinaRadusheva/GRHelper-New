namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using GRHelper.Common;
    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Services.Data.Models;
    using GRHelper.Services.Mapping;
    using GRHelper.Services.Messaging;
    using GRHelper.Web.ViewModels.Administration.Reservations;
    using Microsoft.EntityFrameworkCore;

    public class ReservationsService : IReservationsService
    {
        private readonly IDeletableEntityRepository<Reservation> reservations;
        private readonly IDeletableEntityRepository<Villa> villas;
        private readonly IEmailSender emailSender;

        public ReservationsService(
            IDeletableEntityRepository<Reservation> reservations,
            IDeletableEntityRepository<Villa> villas,
            IEmailSender emailSender)
        {
            this.reservations = reservations;
            this.villas = villas;
            this.emailSender = emailSender;
        }

        public IEnumerable<T> All<T>(bool active, int pageNumber)
        {
            if (active)
            {
                return this.reservations.AllAsNoTracking()
               .Include(r => r.Villa)
               .Include(r => r.Requests)
               .Where(r => r.From.Date >= DateTime.UtcNow.Date || r.To.Date >= DateTime.UtcNow.Date)
               .OrderBy(r => r.From)
               .Skip((pageNumber - 1) * GlobalConstants.ItemsPerPage)
               .Take(GlobalConstants.ItemsPerPage)
               .AsQueryable()
               .To<T>()
               .ToList();
            }
            else
            {
                return this.reservations.AllAsNoTracking()
               .Include(r => r.Villa)
               .Include(r => r.Requests)
               .Where(r => r.To < DateTime.UtcNow.Date)
               .OrderByDescending(r => r.From)
               .AsQueryable()
               .To<T>()
               .ToList();
            }
        }

        public IEnumerable<T> GetBySearchTerms<T>(int? number)
        {
            if (number == null)
            {
                return Enumerable.Empty<T>();
            }

            var reservations = this.reservations.AllAsNoTracking();

            if (number != null)
            {
                reservations = reservations.Where(r => r.Number == number);
            }

            return reservations.AsQueryable().To<T>().ToList();
        }

        public async Task<int> CreateAsync(CreateReservationInputModel input)
        {
            var reservation = new Reservation
            {
                From = input.From.AddHours(15),
                To = input.To.AddHours(12),
                Number = input.Number,
                Email = input.Email,
                Name = input.Name,
                AdultsCount = input.AdultsCount,
                ChildrenCount = input.ChildrenCount,
                VillaId = this.villas.AllAsNoTracking().FirstOrDefault(v => v.Number == input.Villa).Id,
                Password = HelperMethods.GeneratePassword(DataConstants.ResPasswordLength),
            };

            await this.reservations.AddAsync(reservation);
            await this.reservations.SaveChangesAsync();
            return reservation.Id;
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
            reservation.VillaId = this.villas.AllAsNoTracking().FirstOrDefault(v => v.Number == input.VillaNumber).Id;
            reservation.Number = input.Number;

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
            return await this.reservations.AllAsNoTracking()
               .Include(r => r.Requests)
               .Where(r => r.Id == id)
               .To<T>()
               .FirstOrDefaultAsync();
        }

        public int GetCount(bool active)
        {
            var all = this.reservations.AllAsNoTracking();
            if (active)
            {
                return all.Where(r => r.From.Date >= DateTime.UtcNow.Date || r.To.Date >= DateTime.UtcNow.Date)
                    .Count();
            }
            else
            {
                return all.Where(r => r.To < DateTime.UtcNow.Date)
               .Where(r => r.To < DateTime.UtcNow.Date).Count();
            }
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

        public List<ReservationForRequestDto> AvailableByGuestId(string id)
        {
            return this.reservations.AllAsNoTracking()
                .Where(r => r.GuestId == id && r.To >= DateTime.UtcNow)
                .Include(r => r.Villa)
                .AsEnumerable()
                .AsQueryable()
                .Select(r => new ReservationForRequestDto
                {
                    Id = r.Id,
                    From = r.From,
                    To = r.To,
                    Number = r.Number,
                    VillaNumber = r.Villa.Number,
                })
                .OrderBy(r => r.From)
                .ToList();
        }

        public int GetUnlockedCount(string email)
        {
            return this.reservations.AllAsNoTracking()
                .Where(r => r.Email == email && r.Unlocked == false)
                .Count();
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

        public bool Unlock(int id, string password, string userId)
        {
            var reservation = this.reservations.All().FirstOrDefault(r => r.Id == id && r.Password == password);
            if (reservation == null)
            {
                return false;
            }

            reservation.Unlocked = true;
            reservation.GuestId = userId;
            this.reservations.SaveChangesAsync().GetAwaiter().GetResult();
            return true;
        }

        public bool UserIsOwner(int reservationId, string userId)
        {
            return this.reservations.AllAsNoTracking().Any(r => r.Id == reservationId && r.GuestId == userId);
        }

        //private Func<Reservation, bool> GetMatches(string searchTerm)
        //=> res => this.SearchMatches(res, a => a.Name, searchTerm);

        //private bool SearchMatches<T>(T res, Func<T, string> fun, string searchTerm)
        //{
        //    var words = searchTerm.Split(" ").ToArray();
        //    var result = false;
        //    for (int i = 0; i < words.Length; i++)
        //    {
        //        if (fun(res).ToLower().Contains(words[i].ToLower()))
        //        {
        //            result = true;
        //            break;
        //        }
        //    }

        //    return result;
        //}

        public async Task<bool> SendPassword(int id)
        {
            try
            {
                var reservation = await this.GetByIdAsync<ReservationDetailsViewModel>(id);
                var receiver = reservation.Email;
                var html = new StringBuilder();
                html.AppendLine("<h2>Hello from your Guest Relations Team at Vaya Beach Resort!</h2>");
                html.AppendLine("<h4>Thank you for booking with us!</h4>");
                html.AppendLine($"<p>You can easily contact us and manage your request to our team through our web application.</p><p>Your reservations is No. <span style='font-weight:bold'>{reservation.Number}</span> and the password is <span style='font-weight:bold'>{reservation.Password}</span>.</p>");
                html.AppendLine("<h4>We are looking forward to welcoming you for a memorable vacation!</h4>");
                await this.emailSender.SendEmailAsync("MarinaRadusheva@students.softuni.bg", "VayaBeach", receiver, "Reservation password", html.ToString());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
