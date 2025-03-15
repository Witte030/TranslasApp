using Microsoft.AspNetCore.Mvc;
using TranslasApp.Backend.Dtos.Receiver;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Mappers;

namespace TranslasApp.Backend.Controllers
{
    [Route("api/receiver")]
    [ApiController]

    public class ReceiverController : ControllerBase
    {
        private readonly IReceiverRepository _receiverRepository;
        public ReceiverController(IReceiverRepository receiverRepository)
        {
            _receiverRepository = receiverRepository;
        }
     
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var receiver = await _receiverRepository.GetAllAsync();
            var receiverDto =  receiver.Select(r => r.ToReceiverDto());
            return Ok(receiverDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var receiver = await _receiverRepository.GetByIdAsync(id);
            if (receiver == null)
            {
                return NotFound();
            }
            return Ok(receiver.ToReceiverDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReceiverRequestDto receiverDto)
        {
            var receiver = receiverDto.ToReceiverFromCreateDto();
            await _receiverRepository.CreateAsync(receiver);
            return CreatedAtAction(nameof(GetById), new { id = receiver.Id }, receiver.ToReceiverDto());

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateReceiverRequestDto receiverDto)
        {
            var receiver = await _receiverRepository.UpdateAsync(id, receiverDto);
            if (receiver == null)
            {
                return NotFound();
            }

            return Ok(receiver.ToReceiverDto());
        }
    }
}

