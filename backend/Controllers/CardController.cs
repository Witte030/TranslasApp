using Microsoft.AspNetCore.Mvc;
using TranslasApp.Backend.Dtos.Card;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Mappers;

namespace TranslasApp.Backend.Controllers
{
    [Route("api/card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;

        public CardController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cards = await _cardRepository.GetAllAsync();
            var cardDtos = cards.Select(c => c.ToCardDto());
            return Ok(cardDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card.ToCardDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCardRequestDto cardDto)
        {
            var card = cardDto.ToCardFromCreateDto();
            await _cardRepository.CreateAsync(card);
            return CreatedAtAction(nameof(GetById), new { id = card.Id }, card.ToCardDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCardRequestDto cardDto)
        {
            var card = await _cardRepository.UpdateAsync(id, cardDto);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card.ToCardDto());
        }
        [HttpPut("{id:int}/toggle-handled")]
        public async Task<IActionResult> ToggleHandled(int id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            
            // Toggle the handled status
            card.IsHandled = !card.IsHandled;
            
            // Update the card using repository
            var updatedCard = await _cardRepository.ToggleHandledAsync(id, card.IsHandled);
            
            if (updatedCard == null)
            {
                return NotFound();
            }
            
            return Ok(updatedCard.ToCardDto());
        }


    }
}

