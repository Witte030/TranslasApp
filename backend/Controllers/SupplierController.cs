using Microsoft.AspNetCore.Mvc;
using TranslasApp.Backend.Dtos.Supplier;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Mappers;

namespace TranslasApp.Backend.Controllers
{
    [Route("api/supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            var supplierDtos = suppliers.Select(s => s.ToSupplierDto());
            return Ok(supplierDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier.ToSupplierDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierRequestDto supplierDto)
        {
            var supplier = supplierDto.ToSupplierFromCreateDto();
            await _supplierRepository.CreateAsync(supplier);
            return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier.ToSupplierDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSupplierRequestDto supplierDto)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            // Update the supplier using the repository
            await _supplierRepository.UpdateAsync(id, supplierDto);

            return NoContent(); // Or return Ok() if you prefer
        }

    }
}