using AutoMapper;
using SE161774.ProductManagement.Repo.Models;
using SE161774.ProductManagement.Repo.ViewModel.CategoryViewModel;
using SE161774.ProductManagement.Repo.ViewModels.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void CategoryMapperConfigs()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CategoryUpdateModel>().ReverseMap();
        }
    }
}
