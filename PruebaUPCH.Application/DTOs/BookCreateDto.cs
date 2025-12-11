namespace PruebaUPCH.Application.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public int PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public int PageCount { get; set; }
        public string? Category { get; set; }
    }
}
