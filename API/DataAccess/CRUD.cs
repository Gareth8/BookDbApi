using Microsoft.VisualBasic;
using Npgsql;

namespace BookDbApi.DataAccess
{
    public class CRUD
    {
        const string m_connectionString = "Host=localhost;Username=postgres;Password=password;Database=postgres";
        private NpgsqlConnection m_connection { get; set; }

        public CRUD()
        {
            m_connection = new NpgsqlConnection(m_connectionString);
            m_connection.Open();
        }

        public async Task<bool> Create(Book p_book)
        {
            try
            {
                if (!await BookExists((p_book.GetTitle())))
                {
                    await using (var insertBook = new NpgsqlCommand("INSERT INTO bookdb.books (title, isbn) VALUES (@title, @isbn);", m_connection))
                    {
                        insertBook.Parameters.AddWithValue("title", p_book.GetTitle());
                        insertBook.Parameters.AddWithValue("isbn", p_book.GetIsbn());

                        await insertBook.ExecuteNonQueryAsync();
                    }
                }

                if (!await AuthorExists(p_book.GetAuthor()))
                {
                    await using (var insertAuthor = new NpgsqlCommand("INSERT INTO bookdb.authors (author_name) VALUES (@author);", m_connection))
                    {
                        insertAuthor.Parameters.AddWithValue("author", p_book.GetAuthor());
                        await insertAuthor.ExecuteNonQueryAsync();
                    }
                }

                if (!await PublisherExists(p_book.GetPublisher()))
                {
                    await using (var insertPublisher = new NpgsqlCommand("INSERT INTO bookdb.publishers (publisher_name) VALUES (@publisher);", m_connection))
                    {
                        insertPublisher.Parameters.AddWithValue("publisher", p_book.GetPublisher());
                        await insertPublisher.ExecuteNonQueryAsync();
                    }
                }
                
                await using (var updateBookPublisherLink = new NpgsqlCommand("UPDATE bookdb.books " +
                    "SET publisher_id = (SELECT publisher_id FROM bookdb.publishers WHERE publisher_name = @publisher) " +
                    "WHERE title = @title;", m_connection))
                {
                    updateBookPublisherLink.Parameters.AddWithValue("publisher", p_book.GetPublisher());
                    updateBookPublisherLink.Parameters.AddWithValue("title", p_book.GetTitle());
                    await updateBookPublisherLink.ExecuteNonQueryAsync();
                }

                await using (var insertAuthorBookLink = new NpgsqlCommand("INSERT INTO bookdb.author_book_link " +
                    "VALUES ((SELECT book_id FROM bookdb.books WHERE title = @title LIMIT 1)," +
                    "(SELECT author_id FROM bookdb.authors WHERE author_name = @author));",  m_connection))
                {
                    insertAuthorBookLink.Parameters.AddWithValue("title", p_book.GetTitle());
                    insertAuthorBookLink.Parameters.AddWithValue("author", p_book.GetAuthor());
                    await insertAuthorBookLink.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
        public async Task<bool> AuthorExists(string p_author)
        {
            await using (var getAuthor = new NpgsqlCommand("SELECT COUNT(1) FROM bookdb.authors WHERE author_name = @author;", m_connection))
            {
                getAuthor.Parameters.AddWithValue("author", p_author);
                var count = (long)await getAuthor.ExecuteScalarAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> PublisherExists(string p_publisher)
        {
            await using (var getPublisher = new NpgsqlCommand("SELECT COUNT(1) FROM bookdb.publishers WHERE publisher_name = @publisher;", m_connection))
            {
                getPublisher.Parameters.AddWithValue("publisher", p_publisher);
                var count = (long)await getPublisher.ExecuteScalarAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
