
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using SampleApplication.DTOs;
using SampleApplication.Models;

namespace SampleApplication.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDTO>(); CreateMap<CategoryDTO, Category>();
            CreateMap<GeneralLookup, GeneralLookupDTO>(); CreateMap<GeneralLookupDTO, GeneralLookup>();
            CreateMap<Example, ExampleDTO>(); CreateMap<ExampleDTO, Example>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Language, LanguageDTO>();
            CreateMap<LanguageDTO, Language>();
        }
    }
}
