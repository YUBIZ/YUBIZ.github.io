using LibGit2Sharp;
using Models;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Xml;
using System.Xml.Serialization;

namespace Cli;

public static class Common
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

    private static readonly XmlWriterSettings xmlWriterSettings = new() { Encoding = System.Text.Encoding.UTF8, Async = true };

    public static async Task GenerateJsonAsync<T>(string outputFilePath, T value, bool indent = false)
    {
        if (Path.GetDirectoryName(outputFilePath) is string outputDirPath && !string.IsNullOrEmpty(outputDirPath) && !Directory.Exists(outputDirPath))
        {
            Directory.CreateDirectory(outputDirPath);
        }

        jsonSerializerOptions.WriteIndented = indent;

        await File.WriteAllTextAsync(outputFilePath, JsonSerializer.Serialize(value, jsonSerializerOptions));
    }

    public static async Task GenerateXmlAsync<T>(string outputFilePath, T value, bool indent = false)
    {
        if (Path.GetDirectoryName(outputFilePath) is string outputDirPath && !string.IsNullOrEmpty(outputDirPath) && !Directory.Exists(outputDirPath))
        {
            Directory.CreateDirectory(outputDirPath);
        }

        var serializer = new XmlSerializer(typeof(T));

        xmlWriterSettings.Indent = indent;
        await using var writer = XmlWriter.Create(outputFilePath, xmlWriterSettings);
        serializer.Serialize(writer, value, new XmlSerializerNamespaces([new XmlQualifiedName("", "")]));
    }

    public static Regex? GetRegex(string[] patterns)
    {
        if (patterns.Length == 0) return null;

        string combinedPattern = string.Join("|", patterns.Select(p => $"({Regex.Escape(p).Replace(@"\*", ".*").Replace(@"\?", ".")})"));

        return new Regex($"^({combinedPattern})$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    public static IEnumerable<PathInfo> EnumerateFilePathInfos(string sourceDirPath, string[] searchPatterns, string[] excludePatterns)
    {
        sourceDirPath = Path.TrimEndingDirectorySeparator(sourceDirPath);

        string sourceDirFullPath = Path.GetFullPath(sourceDirPath);
        string sourceDirName = Path.GetFileName(sourceDirPath);

        var pathInfos = Directory.EnumerateFiles(sourceDirFullPath, "*", SearchOption.AllDirectories).Select(v => new PathInfo(v.Replace("\\", "/"), Path.Combine(sourceDirName, Path.GetRelativePath(sourceDirFullPath, v)).Replace("\\", "/")));

        if (GetRegex(searchPatterns) is Regex searchRegex)
        {
            pathInfos = pathInfos.Where(v => searchRegex.IsMatch(v.RelativePath));
        }

        if (GetRegex(excludePatterns) is Regex excludeRegex)
        {
            pathInfos = pathInfos.Where(v => !excludeRegex.IsMatch(v.RelativePath));
        }

        return pathInfos;
    }

    public static IEnumerable<CommitMetadata> EnumerateCommitHistory(Repository repo, string relativeFilePath)
    {
        var logEntries = repo.Commits.QueryBy(relativeFilePath, new CommitFilter { SortBy = CommitSortStrategies.Topological });

        return logEntries.Select(
            entry =>
            {
                var commit = entry.Commit;
                return new CommitMetadata(
                    commit.Author.Name,
                    commit.Author.Email,
                    commit.Committer.Name,
                    commit.Committer.Email,
                    commit.Message,
                    commit.Committer.When.DateTime
                );
            }
        ).
        DefaultIfEmpty(
            new CommitMetadata(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                default
            )
        );
    }

    public static FileTree GetFileTree(string sourceDirPath, string[] searchPatterns, string[] excludePatterns)
    {
        sourceDirPath = Path.TrimEndingDirectorySeparator(sourceDirPath);

        string sourceDirFullPath = Path.GetFullPath(sourceDirPath);
        string rootDirName = Path.GetFileName(sourceDirPath);

        return GetFileTreeInternal(sourceDirFullPath, sourceDirFullPath, rootDirName, GetRegex(searchPatterns), GetRegex(excludePatterns));
    }

    public static FileTree GetFileTreeInternal(string rootDirPath, string dirPath, string rootDirName, Regex? searchRegex, Regex? excludeRegex)
    {
        var subTrees = Directory.EnumerateDirectories(dirPath)
                                .Select(v => GetFileTreeInternal(rootDirPath, v, rootDirName, searchRegex, excludeRegex))
                                .Where(v => v != default)
                                .ToArray();

        var EnumerableFiles = Directory.EnumerateFiles(dirPath)
                                       .Select(v => Path.Combine(rootDirName, Path.GetRelativePath(rootDirPath, v)));

        if (searchRegex != null)
        {
            EnumerableFiles = EnumerableFiles.Where(v => searchRegex.IsMatch(v));
        }

        if (excludeRegex != null)
        {
            EnumerableFiles = EnumerableFiles.Where(v => !excludeRegex.IsMatch(v));
        }

        var files = EnumerableFiles.Select(Path.GetFileName)
                                   .OfType<string>()
                                   .ToArray();

        if (files.Length == 0 && subTrees.Length == 0) return default;

        return new FileTree(new(Path.GetFileName(dirPath), files), subTrees);
    }
}