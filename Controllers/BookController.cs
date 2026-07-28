using BookApi.DTOs;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return Ok(await _bookService.GetBooksAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if(book == null)
                return NotFound();

            return Ok(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Book>> Create(CreateBookDto dto)
        {
            var book = await _bookService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new {id = book.Id },
                book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> Update(int id, UpdateBookDto dto)
        {
            var book = await _bookService.UpdateAsync(id, dto);

            if(book == null)
                return NotFound();

            return Ok(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _bookService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
