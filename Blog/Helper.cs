using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace Blog;

public static partial class Helper
{
    public static T? DeserializeYamlFrontMatter<T>(string input)
    {
        Match result = YamlFrontMatterRegex().Match(input);
        return result.Success ? new Deserializer().Deserialize<T>(result.Groups[1].Value) : default;
    }

    [GeneratedRegex(@"^---(.*?)---", RegexOptions.Singleline)]
    private static partial Regex YamlFrontMatterRegex();
}
