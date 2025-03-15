using Microsoft.AspNetCore.Mvc;
using TranslasApp.Backend.Dtos.Carrier;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Mappers;

namespace TranslasApp.Backend.Controllers
{
    [Route("api/carrier")]
    [ApiController]
    public class CarrierController : ControllerBase
    {
        private readonly ICarrierRepository _carrierRepository;

        public CarrierController( ICarrierRepository carrierRepository)
        {
            _carrierRepository = carrierRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carriers = await _carrierRepository.GetAllAsync();
            var carrierDto =  carriers.Select(c => c.ToCarrierDto());
            return Ok(carrierDto);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var carrier = await _carrierRepository.GetByIdAsync(id);
            if (carrier == null)
            {
                return NotFound();
            }
            return Ok(carrier.ToCarrierDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCarrierRequestDto carrierDto)
        {
            var carrier = carrierDto.ToCarrierFromCreateDto();
            await _carrierRepository.CreateAsync(carrier);
            return CreatedAtAction(nameof(GetById), new { id = carrier.Id }, carrier.ToCarrierDto());

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCarrierRequestDto carrierDto)
        {
            var carrier = await _carrierRepository.UpdateAsync(id, carrierDto);
            if (carrier == null)
            {
                return NotFound();
            }

            return Ok(carrier.ToCarrierDto());
        }
    }
}
