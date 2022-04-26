namespace GRHelper.Web.ViewModels.Guests.Requests
{
    public class ManageButtonsViewModel
    {
        public int Id { get; set; }

        public bool IsEmployee { get; set; }

        public bool CanEdit { get; set; }

        public bool CanCancel { get; set; }

        public bool CanDelete { get; set; }

        public bool CanChange { get; set; }

        public string ChangeBtnText { get; set; } = "Change status";
    }
}
