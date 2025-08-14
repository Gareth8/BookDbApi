namespace BookDbApi.DataAccess;

public class DotEnv
{
    public static void Load(string p_filePath)
    {
        if (!File.Exists(p_filePath))
        {
            Console.WriteLine($"No such file at: {p_filePath}");
            return;
        }

        foreach (var line in File.ReadLines(p_filePath))
        {
            var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                continue;
            }
            
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}