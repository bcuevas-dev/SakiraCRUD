﻿using System;
using System.Collections.Generic;

namespace SakilaWebCRUD.Models;

public partial class SalesByStore
{
    public string? Store { get; set; }

    public string? Manager { get; set; }

    public decimal? TotalSales { get; set; }
}
