using Hotel.Data;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Moddels
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "نام هتل طولانی است")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "آدرس هتل طولانی است")]
        public string Adderss { get; set; }
        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        public int CountryId { get; set; }
    }
    public class UpdateHotelDTO : CreateHotelDTO
    {

    }
    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }
        public CountryDTO Country { get; set; }
    }
}
