using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.ViewModels.ProductViewModel
{
    public class ProductViewModel
    {
        [JsonPropertyName("product-id")]
        public int ProductId { get; set; }

        [JsonPropertyName("category-id")]
        public int? CategoryId { get; set; }

        [JsonPropertyName("product-name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("category-name")]
        public string? CategoryName { get; set; }

        [JsonPropertyName("weight")]
        public string? Weight { get; set; }

        [JsonPropertyName("unit-price")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("units-in-stock")]
        public int UnitsInStock { get; set; }
    }
}
