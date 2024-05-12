using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SE161774.ProductManagement.Repo.Interface;
using SE161774.ProductManagement.Repo.Mappers;
using SE161774.ProductManagement.Repo.Models;
using SE161774.ProductManagement.Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfractstructure(this IServiceCollection services, IConfiguration config)
        {
            //add DJ
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(MapperConfigs).Assembly);
            //Add DB local
            services.AddDbContext<FstoreDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("ProductManagement"), options => options.EnableRetryOnFailure()));


            return services;

            //add automapper
            
        }
    }
}
