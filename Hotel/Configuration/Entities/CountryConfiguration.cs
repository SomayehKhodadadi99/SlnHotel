using Hotel.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Configuration.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> _builder)
        {
            _builder.HasData(
                new Country
                {
                    Id = 1,
                    Name = "Iran",
                    ShortName = "IR"
                },
                                new Country
                                {
                                    Id = 2,
                                    Name = "UnitedStetad",
                                    ShortName = "US"
                                },
                                                new Country
                                                {
                                                    Id = 3,
                                                    Name = "Germany",
                                                    ShortName = "GM"
                                                }
                );
        }
    }
}
