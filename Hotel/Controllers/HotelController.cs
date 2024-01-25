using AutoMapper;
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
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
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
        [HttpGet("{Id}", Name = "GetHotel")]
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

    }
}
