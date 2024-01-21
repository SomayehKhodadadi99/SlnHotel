

using Hotel.Configuration.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Hotel.Data;

namespace Hotel.Data
{
    public class DataBaseContext : IdentityDbContext<ApiUser>
    {
        public DataBaseContext(DbContextOptions options):base(options)
        {           
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder _builder)
        {

            #region می توان مثل بالا این ها ریز در قالب یک کلاس اضافه کنیم

            base.OnModelCreating(_builder);


            _builder.ApplyConfiguration(new RoleConfiguration());
            _builder.ApplyConfiguration(new CountryConfiguration());
            _builder.ApplyConfiguration(new HotelConfiguration());



            //_builder.Entity<Country>().HasData(
            //    new Country
            //    {
            //        Id = 1,
            //        Name = "Iran",
            //        ShortName = "IR"
            //    },
            //                    new Country
            //                    {
            //                        Id = 2,
            //                        Name = "UnitedStetad",
            //                        ShortName = "US"
            //                    },
            //                                    new Country
            //                                    {
            //                                        Id = 3,
            //                                        Name = "Germany",
            //                                        ShortName = "GM"
            //                                    }
            //    );


            //_builder.Entity<Hotel>().HasData(
            //    new Hotel
            //    {
            //        Id = 1,
            //        Name = "HotelEstglal",
            //        Adderss = "Tehran",
            //        CountryId = 1,
            //        Rating = 4.5

            //    },
            //    new Hotel
            //    {
            //        Id = 2,
            //        Name = "Malhatan Tower",
            //        Adderss = "NewYork",
            //        CountryId = 2,
            //        Rating = 5
            //    },
            //    new Hotel
            //    {
            //        Id = 3,
            //        Name = "BerlinHotel",
            //        Adderss = "Berlin",
            //        CountryId = 3,
            //        Rating = 4.25
            //    }
            //    );



            #endregion




        }
    }
}
