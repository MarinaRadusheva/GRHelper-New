﻿namespace GRHelper.Web.ViewModels.Guests.Requests
{
    using System;

    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class RequestListViewModel : BaseRequestListViewModel, IMapFrom<Request>
    {
    }
}
