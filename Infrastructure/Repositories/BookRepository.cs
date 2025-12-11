using PruebaUPCH.Domain.Entities;
using PruebaUPCH.Infrastructure.Database;
using System.Data;
using Dapper;
namespace PruebaUPCH.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DapperContext _context;

        public BookRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();

            return await conn.QueryAsync<Book>(
                "SpBooks_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<Book>(
                "SpBooks_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CreateAsync(Book book)
        {
            using var conn = _context.CreateConnection();

            var parameters = new
            {
                book.Title,
                book.Author,
                book.PublicationYear,
                book.Publisher,
                book.PageCount,
                book.Category
            };

            var newId = await conn.ExecuteScalarAsync<int>(
                "SpBooks_Create",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return newId;
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            using var conn = _context.CreateConnection();

            var parameters = new
            {
                book.Id,
                book.Title,
                book.Author,
                book.PublicationYear,
                book.Publisher,
                book.PageCount,
                book.Category
            };

            var affected = await conn.ExecuteScalarAsync<int>(
                "SpBooks_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();

            var affected = await conn.ExecuteScalarAsync<int>(
                "SpBooks_Delete",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }
    }
}
