﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.ViewModels.ProductViewModel
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public int? CategoryId { get; set; }

        public string? ProductName { get; set; }

        public Test? Weight { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
    }
}