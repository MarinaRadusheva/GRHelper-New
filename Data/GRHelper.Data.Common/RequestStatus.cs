namespace GRHelper.Data.Common
{
    using System.Runtime.Serialization;

    public enum RequestStatus
    {
        NA = 0,
        Waiting = 1,
        [EnumMember(Value = "In progress")]
        InProgress = 2,
        Done = 3,
        Cancelled = 4,
    }
}
