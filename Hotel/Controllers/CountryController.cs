using AutoMapper;
using Hotel.Data;
using Hotel.IRepository;
using Hotel.Moddels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CountryController(ILogger<CountryController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
             _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();

                var result = _mapper.Map<IList<CountryDTO>>(countries);

                return Ok(result);
            }
            catch (Exception)
            {

                _logger.LogError($"somthig went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error,Please Try Again  Later");
                
            }
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountrie(int id)
        {
            try
            {
                var countrie = await _unitOfWork.Countries.Get(x=>x.Id == id,new List<string> { "Hotels" });

                var result = _mapper.Map<CountryDTO>(countrie);

                return Ok(result);
            }
            catch (Exception)
            {
                _logger.LogError($"somthig went wrong in the {nameof(GetCountrie)}");
                return StatusCode(500, "Internal Server Error,Please Try Again  Later");

            }
        }
    }
}
