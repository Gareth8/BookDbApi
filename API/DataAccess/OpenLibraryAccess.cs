using BookDbApi.Utilities;
using Newtonsoft.Json.Linq;

namespace BookDbApi.DataAccess;

public static class OpenLibraryAccess
{
    static csvHandler csvParser = new csvHandler();
    const string m_baseAddress = "https://openlibrary.org/";
    
    private static readonly HttpClient m_httpClient = new HttpClient()
    {
        BaseAddress = new Uri(m_baseAddress),
    };

    public static async Task<Book> GetBook(string p_ISBN)
    {
        string l_request = $"isbn/{p_ISBN}.json";
        string l_title = "";
        string l_publisher = "";
        string l_author = "";
        List<string>? l_genres = new List<string>();

        HttpResponseMessage isbnResponse = await m_httpClient.GetAsync(m_baseAddress + l_request);

        isbnResponse.EnsureSuccessStatusCode();

        string isbnJsonResponse = await isbnResponse.Content.ReadAsStringAsync();

        if (isbnJsonResponse is "" or "{}")
        {
            throw new Exception("Book not found");
        }

        try
        {
            l_title = (string)JObject.Parse(isbnJsonResponse)["title"];
            l_publisher = (string)JObject.Parse(isbnJsonResponse)["publishers"][0];

            string l_titlePath = $"search.json?q={l_title}&fields=author_name,subject&limit=1";

            HttpResponseMessage titleResponse = await m_httpClient.GetAsync(m_baseAddress + l_titlePath);
            titleResponse.EnsureSuccessStatusCode();
            string titleJsonResponse = await titleResponse.Content.ReadAsStringAsync();

            if (titleJsonResponse is "" or "{}")
            {
                throw new Exception("Book not found");
            }
            
            var titleJObject = JObject.Parse(titleJsonResponse);
            var titleToken = titleJObject.SelectToken("docs[0].author_name[0]");
            l_genres = csvHandler.GetSubjects(titleJObject["docs"]?[0]?["subject"]);

            if (titleToken == null)
            {
                l_author = "unknown";
            }
            else
            {
                l_author = titleToken.ToString();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (!l_genres.Any())
        {
            return new Book(l_title, l_author, l_publisher, p_ISBN);
        }
        
        return new Book(l_title, l_author, l_publisher, p_ISBN,  l_genres);
    }
}