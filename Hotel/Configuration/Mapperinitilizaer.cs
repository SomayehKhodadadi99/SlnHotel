using AutoMapper;
using Hotel.Data;
using Hotel.Moddels;

namespace Hotel.Configuration
{
    public class Mapperinitilizaer:Profile
    {

        public Mapperinitilizaer()
        {
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Hotel.Data.Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel.Data.Hotel, CreateHotelDTO>().ReverseMap();
            CreateMap<Hotel.Data.ApiUser, UserDTO>().ReverseMap();

            CreateMap<Hotel.Data.ApiUser, LoginUserDTO>().ReverseMap();

        }
    }
}
