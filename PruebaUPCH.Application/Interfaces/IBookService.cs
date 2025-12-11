using PruebaUPCH.Application.DTOs;

namespace PruebaUPCH.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponseDto>> GetAllAsync();
        Task<BookResponseDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(BookCreateDto dto);
        Task<bool> UpdateAsync(BookUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
