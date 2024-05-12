using SE161774.ProductManagement.Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public int? MemberId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public decimal? Freight { get; set; }
    }
}
