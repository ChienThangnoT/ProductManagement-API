using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.Mappers
{
    public partial class MapperConfigs : Profile
    {
        public MapperConfigs()
        {
            // add category mapper
            CategoryMapperConfigs();
            ProductMapperConfigs();
        }
        partial void CategoryMapperConfigs();
        partial void ProductMapperConfigs();
    }
}
