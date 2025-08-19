using BookDbApi.Utilities;
using Newtonsoft.Json.Linq;

namespace BookDbApi.DataAccess;

public static class OpenLibraryAccess
{
    static readonly jsonUtilities jsonUtility = new();
    const string m_baseAddress = "https://openlibrary.org/";
    
    private static readonly HttpClient m_httpClient = new()
    {
        BaseAddress = new Uri(m_baseAddress),
    };

    public static async Task<Book> GetBook(string p_ISBN)
    {
        string l_request = $"isbn/{p_ISBN}.json";

        Book newBook = new Book();
        newBook.SetIsbn(p_ISBN);

        HttpResponseMessage isbnResponse = await m_httpClient.GetAsync(m_baseAddress + l_request);

        isbnResponse.EnsureSuccessStatusCode();

        string isbnJsonResponse = await isbnResponse.Content.ReadAsStringAsync();

        if (isbnJsonResponse is "" or "{}")
        {
            throw new Exception("Book not found");
        }

        try
        {
            newBook.SetTitle((string)JObject.Parse(isbnJsonResponse)["title"]);
            newBook.SetPublisher((string)JObject.Parse(isbnJsonResponse)["publishers"][0]);
            
            string l_titlePath = $"search.json?q={newBook.GetTitle()}&fields=author_name,subject&limit=1";
            
            HttpResponseMessage titleResponse = await m_httpClient.GetAsync(m_baseAddress + l_titlePath);
            titleResponse.EnsureSuccessStatusCode();
            string titleJsonResponse = await titleResponse.Content.ReadAsStringAsync();
            
            if (titleJsonResponse is "" or "{}")
            {
                throw new Exception("Book not found");
            }
            
            var titleJObject = JObject.Parse(titleJsonResponse);
            var titleToken = titleJObject.SelectToken("docs[0].author_name[0]");
            jsonUtility.GetSubjects(titleJObject["docs"]?[0]?["subject"], newBook);

            if (titleToken != null)
            {
                newBook.SetAuthor(titleToken.ToString());
            }
            else
            {
                newBook.SetAuthor("Unknown");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return newBook;
    }
}