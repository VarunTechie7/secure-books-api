using Microsoft.AspNetCore.Mvc;
using SecureBooksApi.Data;
using SecureBooksApi.Models;

namespace SecureBooksApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly BookRepository _repository;

    public BooksController(BookRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBook([FromBody] Book book)
    {
        var createdBook = _repository.AddBook(book);

        return Created($"/api/books/{createdBook.Id}", createdBook);
    }
}