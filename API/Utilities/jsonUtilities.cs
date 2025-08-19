using Newtonsoft.Json.Linq;

namespace BookDbApi.Utilities;

public enum genreWhitelist
{
    fiction,
    nonfiction,
    mystery,
    thriller,
    fantasy,
    sciencefiction,
    romance,
    historical,
    horror,
    biography,
    poetry,
    adventure,
    youngadult,
    children,
    selfhelp,
    philosophy,
    graphicnovel
}

public class jsonUtilities
{
    public static genreWhitelist whitelist;
    
    public void GetSubjects(JToken p_subjects, Book p_book)
    {
        if (p_subjects is not JArray subjectsArray) return;
        foreach (JToken subject in subjectsArray)
        {
            string genre = subject.ToString().Trim();
            if (!string.IsNullOrEmpty(genre) && Enum.TryParse(genre.Replace(" ", ""), true, out whitelist))
            {
                p_book.AddGenre(genre.ToLower());
            }
        }
    }
}