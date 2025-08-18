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

public class csvHandler
{
    public static genreWhitelist whitelist;
    
    public static List<string> GetSubjects(JToken p_subjects)
    {
        List<string> subjects = new List<string>();

        if (p_subjects is JArray subjectsArray)
        {
            foreach (JToken subject in subjectsArray)
            {
                string genre = subject.ToString().Trim();
                if (!string.IsNullOrEmpty(genre) && Enum.TryParse(genre.Replace(" ", ""), true, out whitelist))
                {
                    subjects.Add(genre.ToLower());
                }
            }
        }
        
        return subjects;
    }
}