using Blog.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace Blog.Misc;

public static class YamlHttpClientExtensions
{
    public static async Task<T?> GetFromYamlAsync<T>(this HttpClient httpClient, [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri)
    => YamlHelper.DeserializeYaml<T>(await httpClient.GetStringAsync(requestUri));

    public static async Task<T?> GetFromYamlAsync<T>(this HttpClient httpClient, Uri? requestUri)
        => YamlHelper.DeserializeYaml<T>(await httpClient.GetStringAsync(requestUri));

    public static async Task<T?> GetFromYamlAsync<T>(this HttpClient httpClient, [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken)
        => YamlHelper.DeserializeYaml<T>(await httpClient.GetStringAsync(requestUri, cancellationToken));

    public static async Task<T?> GetFromYamlAsync<T>(this HttpClient httpClient, Uri? requestUri, CancellationToken cancellationToken)
        => YamlHelper.DeserializeYaml<T>(await httpClient.GetStringAsync(requestUri, cancellationToken));

    public static async Task<T?> GetFromYamlFrontMatterAsync<T>(this HttpClient httpClient, [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri)
        => YamlHelper.DeserializeYamlFrontMatter<T>(await httpClient.GetStringAsync(requestUri));

    public static async Task<T?> GetFromYamlFrontMatterAsync<T>(this HttpClient httpClient, Uri? requestUri)
        => YamlHelper.DeserializeYamlFrontMatter<T>(await httpClient.GetStringAsync(requestUri));

    public static async Task<T?> GetFromYamlFrontMatterAsync<T>(this HttpClient httpClient, [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken)
        => YamlHelper.DeserializeYamlFrontMatter<T>(await httpClient.GetStringAsync(requestUri, cancellationToken));

    public static async Task<T?> GetFromYamlFrontMatterAsync<T>(this HttpClient httpClient, Uri? requestUri, CancellationToken cancellationToken)
        => YamlHelper.DeserializeYamlFrontMatter<T>(await httpClient.GetStringAsync(requestUri, cancellationToken));

}
