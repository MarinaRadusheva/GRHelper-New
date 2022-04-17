namespace GRHelper.Web.ViewModels.Guests.Reservations
{
    public class ReservationForRequestModel : ReservationListViewModel
    {
        public string VillaNumber { get; set; }

        public override string ToString()
        {
            return "No: " + this.Number + " - " + this.VillaNumber + " - " + this.From.ToShortDateString();
        }
    }
}
