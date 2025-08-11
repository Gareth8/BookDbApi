using Microsoft.VisualBasic;
using Npgsql;

namespace BookDbApi.DataAccess
{
    public class CRUD
    {
        const string m_connectionString = "Host=localhost;Username=postgres;Password=password;Database=postgres";
        public NpgsqlConnection m_connection { get; private set; }

        public CRUD()
        {
            m_connection = new NpgsqlConnection(m_connectionString);
            m_connection.Open();
        }

        public async Task<bool> Create(Book p_book)
        {
            try
            {
                await using (var insertBook = new NpgsqlCommand("INSERT INTO bookdb.books (title, isbn) VALUES (@title, @isbn);", m_connection))
                {
                    insertBook.Parameters.AddWithValue("title", p_book.GetTitle());
                    insertBook.Parameters.AddWithValue("isbn", p_book.GetIsbn());

                    await insertBook.ExecuteNonQueryAsync();
                }

                await using (var insertAuthor = new NpgsqlCommand("INSERT INTO bookdb.authors (author_name) VALUES (@author);", m_connection))
                {
                    insertAuthor.Parameters.AddWithValue("author", p_book.GetAuthor());
                    await insertAuthor.ExecuteNonQueryAsync();
                }

                await using (var insertPublisher = new NpgsqlCommand("INSERT INTO bookdb.publishers (publisher_name) VALUES (@publisher);", m_connection))
                {
                    insertPublisher.Parameters.AddWithValue("publisher", p_book.GetPublisher());
                    await insertPublisher.ExecuteNonQueryAsync();
                }

                await using (var insertGenre = new NpgsqlCommand("INSERT INTO bookdb.genres (genre_name) VALUES (@genre);", m_connection))
                {
                    insertGenre.Parameters.AddWithValue("genre", p_book.GetGenre().ToString());
                    await insertGenre.ExecuteNonQueryAsync();
                }

                await using (var insertAuthorBookLink = new NpgsqlCommand("INSERT INTO bookdb.author_book_link" +
                    "VALUES ((SELECT book_id FROM bookdb.books WHERE title = @title)," +
                    "(SELECT author_id FROM bookdb.authors WHERE author_name = @author));"))
                {
                    insertAuthorBookLink.Parameters.AddWithValue("title", p_book.GetTitle());
                    insertAuthorBookLink.Parameters.AddWithValue("author", p_book.GetAuthor());
                    await insertAuthorBookLink.ExecuteNonQueryAsync();
                }

                await using (var insertGenreBookLink = new NpgsqlCommand("INSERT INTO bookdb.genre_book_link" +
                    "VALUES ((SELECT book_id FROM bookdb.books WHERE title = @title)," +
                    "(SELECT genre_id FROM bookdb.genres WHERE genre_name = @genre));"))
                {
                    insertGenreBookLink.Parameters.AddWithValue("title", p_book.GetTitle());
                    insertGenreBookLink.Parameters.AddWithValue("genre", p_book.GetGenre().ToString());
                    await insertGenreBookLink.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> BookExists(string p_bookTitle)
        {
            await using (var getBook = new NpgsqlCommand("SELECT COUNT(1) FROM bookdb.books WHERE title = @title;", m_connection))
            {
                getBook.Parameters.AddWithValue("title", p_bookTitle);
                var count = (long)await getBook.ExecuteScalarAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
