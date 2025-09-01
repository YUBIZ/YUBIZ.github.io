using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace Blog.Helpers;

public partial class YamlHelper
{
    private static readonly Deserializer deserializer = new();

    public static T? DeserializeYaml<T>(in string input) => deserializer.Deserialize<T>(input);

    public static T? DeserializeYamlFrontMatter<T>(in string input)
    {
        Match result = YamlFrontMatterRegex().Match(input);
        return result.Success ? DeserializeYaml<T>(result.Groups[1].Value) : default;
    }

    [GeneratedRegex(@"^---(.*?)---", RegexOptions.Singleline)]
    private static partial Regex YamlFrontMatterRegex();
}
