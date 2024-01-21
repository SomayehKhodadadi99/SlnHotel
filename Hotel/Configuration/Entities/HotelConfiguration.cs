using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hotel.Data;

namespace Hotel.Configuration.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel.Data.Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel.Data.Hotel> _builder)
        {
            _builder.HasData(
                new Hotel.Data.Hotel
                {
                    Id = 1,
                    Name = "HotelEstglal",
                    Adderss = "Tehran",
                    CountryId = 1,
                    Rating = 4.5

                },
                new Hotel.Data.Hotel
                {
                    Id = 2,
                    Name = "Malhatan Tower",
                    Adderss = "NewYork",
                    CountryId = 2,
                    Rating = 5
                },
                new Hotel.Data.Hotel
                {
                    Id = 3,
                    Name = "BerlinHotel",
                    Adderss = "Berlin",
                    CountryId = 3,
                    Rating = 4.25
                }
                );
        }
    }
}
