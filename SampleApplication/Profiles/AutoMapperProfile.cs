
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
            CreateMap<GeneralLookup, GeneralLookupDTO>(); 
            CreateMap<GeneralLookupDTO, GeneralLookup>();
        }
    }
}
