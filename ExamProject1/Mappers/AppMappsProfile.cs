using AutoMapper;
using ExamProject.Models;
using ExamProject1.Dto;
using ExamProject1.Models;

namespace ExamProject1.Mappers
{
    public class AppMappsProfile : Profile
    {
        public AppMappsProfile() 
        {
            CreateMap<UserGetDto, User>().ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<ContactCreateDto, Contact>().ReverseMap();
            CreateMap<ContactChangeDto, Contact>().ReverseMap();
            CreateMap<SaleCreateDto, Sale>().ReverseMap();
            CreateMap<LeadCreateDto, Lead>().ReverseMap();
        }
    }
}
