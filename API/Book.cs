namespace BookDbApi
{
    public class Book
    {
        private string m_title { get; set; } = "Unknown";
        private string m_author { get; set; } = "Unknown";
        private string m_publisher { get; set; }  = "Unknown";
        private string m_isbn { get; set; }   = "Unknown";
        private List<string> m_genres { get; set; } = new List<string>();


        #region Constructors
        public Book()
        {
        }
        public Book(string title, string author, string publisher, string isbn)
        {
            m_title = title;
            m_author = author;
            m_publisher = publisher;
            m_isbn = isbn;
        }
        public Book(string title, string author, string publisher, string isbn, List<string> genres)
        {
            m_title = title;
            m_author = author;
            m_publisher = publisher;
            m_isbn = isbn;
            m_genres = genres;
        }
        #endregion
        public override string ToString()
        {
            if (m_genres.Any())
            {
                return $"{m_title} written by {m_author}, published by {m_publisher}. ISBN: {m_isbn}. Genres: {string.Join(", ", m_genres)}";
            }
            
            return $"{m_title} written by {m_author}, published by {m_publisher}. ISBN: {m_isbn}";
        }

        #region Getters
        public string GetTitle()
        {
            return m_title;
        }
        public string GetAuthor()
        {
            return m_author;
        }
        public string GetPublisher()
        {
            return m_publisher;
        }
        public string GetIsbn()
        {
            return m_isbn;
        }
        public List<string> GetGenres()
        {
            return m_genres;
        }
        #endregion
        
        #region Setters

        public void SetTitle(string p_title)
        {
            m_title = p_title;
        }
        public void SetAuthor(string p_author)
        {
            m_author = p_author;
        }
        public void SetPublisher(string p_publisher)
        {
            m_publisher = p_publisher;
        }
        public void SetIsbn(string p_isbn)
        {
            m_isbn = p_isbn;
        }
        public void SetGenres(List<string> p_genres)
        {
            m_genres = p_genres;
        }

        public void AddGenre(string p_genre)
        {
            m_genres.Add(p_genre);
        }
        #endregion
    }
}
