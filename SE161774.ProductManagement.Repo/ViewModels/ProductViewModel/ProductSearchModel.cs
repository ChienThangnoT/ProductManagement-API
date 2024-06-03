using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.ViewModels.ProductViewModel
{
    public class ProductSearchModel
    {
        [JsonPropertyName("page-index")]
        public int? page_index { get; set; }

        [JsonPropertyName("page-size")]
        public int? page_size { get; set; }

        [JsonPropertyName("product-name")]
        public string? product_name { get; set; }

        [JsonPropertyName("unit-price-minx")]
        public decimal? unit_price_min { get; set; }

        [JsonPropertyName("unit-price-max")]
        public decimal? unit_price_max { get; set; }
        [JsonPropertyName("sort-by-price")]
        public bool? sort_by_price { get; set; } = false;
        public bool? descending { get; set; } = false;
        
    }
}
