using Blog.Extensions;
using Blog.Models;
using Markdig;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Blog.Services;

public class StaticResourceService(HttpClient httpClient)
{
    public async Task<string[]> GetFileListAsync(string fileListUri)
    {
        return await httpClient.GetFromJsonAsync<string[]>($"file-list/{Path.ChangeExtension(fileListUri, "json")}") ?? [];
    }

    public async Task<Commit[]> GetCommitHistoryAsync(string commitHistoryUri)
    {
        return await httpClient.GetFromJsonAsync<Commit[]>($"commit-history/{Path.ChangeExtension(commitHistoryUri, "json")}") ?? [];
    }

    public async Task<PostMetadata> GetPostMetadataAsync(string postUri)
    {
        return await httpClient.GetFromYamlFrontMatterAsync<PostMetadata>(Path.ChangeExtension(postUri, "md"));
    }

    public async Task<MarkupString> GetPostContentAsync(string postUri)
    {
        string postString = await httpClient.GetStringAsync(Path.ChangeExtension(postUri, "md"));
        MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseYamlFrontMatter().UseBootstrap().Build();
        return (MarkupString)Markdown.ToHtml(postString, markdownPipeline);
    }
}
