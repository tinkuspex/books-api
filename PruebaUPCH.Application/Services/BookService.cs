using PruebaUPCH.Application.DTOs;
using PruebaUPCH.Application.Interfaces;
using PruebaUPCH.Domain.Entities;
using PruebaUPCH.Infrastructure.Repositories;

namespace PruebaUPCH.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
        {
            var books = await _repository.GetAllAsync();
            return books.Select(x => new BookResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                PublicationYear = x.PublicationYear,
                Publisher = x.Publisher,
                PageCount = x.PageCount,
                Category = x.Category,
                CreatedAt = x.CreatedAt
            });
        }

        public async Task<BookResponseDto?> GetByIdAsync(int id)
        {
            var b = await _repository.GetByIdAsync(id);
            if (b == null) return null;

            return new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublicationYear = b.PublicationYear,
                Publisher = b.Publisher,
                PageCount = b.PageCount,
                Category = b.Category,
                CreatedAt = b.CreatedAt
            };
        }

        public async Task<int> CreateAsync(BookCreateDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                PublicationYear = dto.PublicationYear,
                Publisher = dto.Publisher,
                PageCount = dto.PageCount,
                Category = dto.Category,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.CreateAsync(book);
        }

        public async Task<bool> UpdateAsync(BookUpdateDto dto)
        {
            var book = await _repository.GetByIdAsync(dto.Id);
            if (book == null) return false;

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.PublicationYear = dto.PublicationYear;
            book.Publisher = dto.Publisher;
            book.PageCount = dto.PageCount;
            book.Category = dto.Category;

            return await _repository.UpdateAsync(book);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
