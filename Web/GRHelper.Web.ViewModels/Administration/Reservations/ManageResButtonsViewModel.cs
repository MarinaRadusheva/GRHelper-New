namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    public class ManageResButtonsViewModel
    {
        public int ReservationId { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}
