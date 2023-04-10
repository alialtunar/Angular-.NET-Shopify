using System.Linq;
using AutoMapper;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Helpers
{
    public class MapperProfiles:Profile 
    {
        public MapperProfiles()
        {
           
            CreateMap<Product,ProductForListDTO>()
                .ForMember(dest => dest.Image, opt => 
                    opt.MapFrom(src => src.Images.FirstOrDefault())) ;

            CreateMap<Product,ProductForDetailsDTO>();
            CreateMap<Image,ImagesForDetailsDTO>();
            CreateMap<User,UserForUserPageDTO>();
            CreateMap<UserForUpdateDTO, User>()
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

          
        }
    }
}