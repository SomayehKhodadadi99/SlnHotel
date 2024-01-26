using AutoMapper;
using Hotel.Data;
using Hotel.Moddels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class HotelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository.IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;

        public HotelController(IMapper mapper, IRepository.IUnitOfWork unitOfWork, ILogger<HotelController> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] QueryParameters queryParameters)
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetPageedList(queryParameters); 
                var hotelDTO = _mapper.Map<IList<HotelDTO>>(hotels);
                return Ok(hotelDTO);
            }
            catch (Exception)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }


        [Authorize]
        [HttpGet("{Id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int Id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == Id, new List<string> { "Country" });
                var hotelDTO = _mapper.Map<HotelDTO>(hotel);
                return Ok(hotelDTO);
            }
            catch (Exception)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody]CreateHotelDTO hotelDTO)
        {

            if (!ModelState.IsValid) 
            {
                _logger.LogError($"Invalid Post attemp in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = _mapper.Map<Hotel.Data.Hotel>(hotelDTO);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel", new { Id = hotel.Id }, hotel);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }


        [Authorize]
        [HttpPut("{Id:int}")]
 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int Id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid || Id < 1)
            {
                _logger.LogError(500, $"Invalid Update Attap to {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {

                //destination
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == Id);
                if (hotel == null)
                {
                    _logger.LogError(500, $"Invalid Update Attap to {nameof(UpdateHotel)}");
                    return BadRequest("submit data in invalid");
                }

              //put mikoonim to hotel
              //destination hotel
              //source hotelDTO

                _mapper.Map(hotelDTO, hotel);//سورس اصلی دی تو هست برای همین اول می نویسیم
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();
                return NoContent();

            }
            catch (Exception)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }




        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int Id)
        {
            if (Id < 1)
            {
                _logger.LogError(500, $"Invalid Delete Attap to {nameof(DeleteHotel)}");
                return BadRequest();
            }
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == Id);
                if (hotel == null)
                {
                    _logger.LogError(500, $"Invalid Update Attap to {nameof(DeleteHotel)}");
                    return BadRequest("submit data in invalid");
                }
                await _unitOfWork.Hotels.Delete(Id);
                await _unitOfWork.Save();
                return NoContent();

            }
            catch (Exception)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(DeleteHotel)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }
    }
}

