namespace GRHelper.Data.Common
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public enum PaymentType
    {
        [Display(Name = "Free")]
        Free = 0,
        [Display(Name = "Hotel Bill")]
        HotelBill = 1,
        [Display(Name = "Credit Card")]
        CreditCard = 2,
        [Display(Name = "Cash")]
        Cash = 3,
    }
}
