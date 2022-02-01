/*
using AutoMapper;
using dbclient.data.EF;
using Stkpnt.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbclient
{

    public class ConfigureMapper : Profile
    {
        public ConfigureMapper()
        {
            CreateMap<ApplicationUserAdd, User>()
              .ForMember(i => i.Guid, f => f.MapFrom(o => o.Id));
        }

    }
}
*/