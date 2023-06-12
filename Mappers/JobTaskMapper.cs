using AutoMapper;
using Sandbox.Entities;
using Sandbox.Models;

namespace Sandbox.Mappers
{
    public class JobTaskMappingProfile : Profile
    {
        public JobTaskMappingProfile()
        {
            CreateMap<Entities.JobTasks, Models.JobTasks>()
                .ForMember(d => d.OrderJobResultTask, b => b.MapFrom(s => s.OrderJobTask));
        }
    }
}