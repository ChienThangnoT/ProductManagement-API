using AutoMapper;
using SE161774.ProductManagement.Repo.Models;
using SE161774.ProductManagement.Repo.ViewModels.ProductViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void ProductMapperConfigs()
        {
            CreateMap<Product, ProductViewModel>().
                ForMember(des => des.CategoryName, otp => otp.MapFrom(x => x.Category.CategoryName))
                .ReverseMap();
        }
    }
}
