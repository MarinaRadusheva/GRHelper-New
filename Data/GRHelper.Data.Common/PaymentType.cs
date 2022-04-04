namespace GRHelper.Data.Common
{
    using System.Runtime.Serialization;

    public enum PaymentType
    {
        Free = 0,
        [EnumMember(Value = "Hotel Bill")]
        HotelBill = 1,
        [EnumMember(Value = "Credit Card")]
        CreditCard = 2,
        Cash = 3,
    }
}
