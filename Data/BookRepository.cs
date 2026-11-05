using Microsoft.Data.Sqlite;
using SecureBooksApi.Models;

namespace SecureBooksApi.Data;

public class BookRepository : IDisposable
{
    private readonly SqliteConnection _connection;

    public BookRepository()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        using var command = _connection.CreateCommand();

        command.CommandText = @"
            CREATE TABLE Books (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Author TEXT NOT NULL
            );";

        command.ExecuteNonQuery();
    }

    public Book AddBook(Book book)
    {
        using var command = _connection.CreateCommand();

        // Parameterized SQL: user input is never concatenated into SQL.
        command.CommandText = @"
            INSERT INTO Books (Title, Author)
            VALUES (@title, @author);
            SELECT last_insert_rowid();";

        command.Parameters.AddWithValue("@title", book.Title);
        command.Parameters.AddWithValue("@author", book.Author);

        book.Id = Convert.ToInt32(command.ExecuteScalar());

        return book;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}