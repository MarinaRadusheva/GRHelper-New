﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRHelper.Services.Data.Models
{
    public class HotelServiceForRequestDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public bool Paid { get; set; }
    }
}
