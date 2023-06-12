using AutoMapper;
using Sandbox.Entities;
using Sandbox.Models;

namespace Sandbox.Mappers
{
    public class OrderJobMappingProfile : Profile
    {
        public OrderJobMappingProfile()
        {
            //eventinfo related to errors?

            CreateMap<OrderJobRequest,OrderJobResult_Accept>()
                .ForMember(d => d.JobId, b => b.MapFrom(s => s.JobId))
                .ForMember(d => d.RequestId, b => b.MapFrom(s => s.RequestId));
        
            CreateMap<OrderJobRequest,OrderJobResult_ToteInduct>()
                .ForMember(d => d.JobId, b => b.MapFrom(s => s.JobId));

            CreateMap<OrderJobRequest,OrderJobResult_Pick_PickComplete>()
                .ForMember(d => d.JobId, b => b.MapFrom(s => s.JobId));
                //.ForMember
        }
    }
}