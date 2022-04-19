namespace GRHelper.Web.ViewModels.Guests.Requests
{
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class EditRequestInputModel : BaseRequestInputModel, IMapFrom<Request>
    {
        public int Id { get; set; }
    }
}
