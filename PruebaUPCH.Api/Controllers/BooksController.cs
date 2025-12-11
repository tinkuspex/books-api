using Microsoft.AspNetCore.Mvc;
using PruebaUPCH.Application.DTOs;
using PruebaUPCH.Application.Interfaces;
using PruebaUPCH.Domain.Entities;

namespace PruebaUPCH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _service.GetAllAsync();
            return Ok(books);
        }

        // GET: api/books/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDto book)
        {
            var newId = await _service.CreateAsync(book);

            // Construimos un DTO de retorno con el ID asignado
            var createdBook = new BookResponseDto
            {
                Id = newId,
                Title = book.Title,
                Author = book.Author,
                PublicationYear = book.PublicationYear,
                Publisher = book.Publisher,
                PageCount = book.PageCount,
                Category = book.Category
            };

            return CreatedAtAction(nameof(GetById), new { id = newId }, createdBook);
        }

        // PUT: api/books
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BookUpdateDto book)
        {
            var updated = await _service.UpdateAsync(book);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        // DELETE: api/books/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
