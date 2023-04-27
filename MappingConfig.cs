using AutoMapper;
using MyPersonalProject.Models;
using MyPersonalProject.Models.Dto;

namespace MyPersonalProject
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>();

            CreateMap<ContactDto,ContactCreateDto>().ReverseMap();
            CreateMap<ContactDto, ContactUpdateDto>().ReverseMap();

            CreateMap<Contact, ContactUpdateDto>().ReverseMap();
            CreateMap<Contact, ContactCreateDto>().ReverseMap();

            CreateMap<Contact,PhotoUploadDto>().ReverseMap();
            CreateMap<ApplicationUser,UserDto>().ReverseMap();  
        }
    }
}
