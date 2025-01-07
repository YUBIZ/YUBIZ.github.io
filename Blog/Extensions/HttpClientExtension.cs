using YamlDotNet.Serialization;

namespace Blog.Extensions;

public static class HttpClientExtension
{
    public static async Task<T> GetFromYamlFrontMatterAsync<T>(this HttpClient httpClient, string? requestUri)
    {
        string[] lines = (await httpClient.GetStringAsync(requestUri))
                                          .Split('\n')
                                          .Select(static line => line.Trim())
                                          .ToArray();
        if (lines.Length == 0 || lines.First() != "---") throw new("YAML Front Matter의 시작을 찾을 수 없습니다.");

        int lastIndex = Array.FindIndex(lines, 1, static line => line == "---");
        if (lastIndex == -1) throw new("YAML Front Matter의 끝을 찾을 수 없습니다.");

        return new Deserializer().Deserialize<T>(string.Join('\n', lines[1..lastIndex]));
    }
}
