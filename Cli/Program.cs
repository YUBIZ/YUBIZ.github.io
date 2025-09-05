global using static Cli.Common;

using LibGit2Sharp;
using Models;
using System.CommandLine;

static async Task GenerateFilePathAndCommitHistoryCollectionAsync(string sourceDirPath, string outputFilePath, string[] searchPatterns, string[] excludePatterns)
{
    var repoPath = Repository.Discover(sourceDirPath);
    if (repoPath == null) return;

    using var repo = new Repository(repoPath);
    var repoRootDir = repo.Info.WorkingDirectory;

    var filePathInfos = EnumerateFilePathInfos(sourceDirPath, searchPatterns, excludePatterns);

    var filePathAndCommitHistoryCollection = filePathInfos.Select(v => new FilePathAndCommitHistory(v.RelativePath, EnumerateCommitHistory(repo, Path.GetRelativePath(repoRootDir, v.AbsolutePath).Replace("\\", "/")).ToArray()));

    await GenerateJsonAsync(outputFilePath, filePathAndCommitHistoryCollection);
}

static async Task GenerateFileTreeAsync(string sourceDirPath, string outputFilePath, string[] searchPatterns, string[] excludePatterns)
{
    var fileTree = GetFileTree(sourceDirPath, searchPatterns, excludePatterns);

    await GenerateJsonAsync(outputFilePath, fileTree);
}

static async Task GenerateSitemapAsync(string sourceDirPath, string outputFilePath, string baseUrl, string[] searchPatterns, string[] excludePatterns)
{
    var repoPath = Repository.Discover(sourceDirPath);
    if (repoPath == null) return;

    using var repo = new Repository(repoPath);
    var repoRootDir = repo.Info.WorkingDirectory;

    var filePathInfos = EnumerateFilePathInfos(sourceDirPath, searchPatterns, excludePatterns);

    var filePathAndCommitHistoryCollection = filePathInfos.Select(v => new FilePathAndCommitHistory(v.RelativePath, EnumerateCommitHistory(repo, Path.GetRelativePath(repoRootDir, v.AbsolutePath).Replace("\\", "/")).ToArray()));

    var urlSet = new UrlSet(filePathAndCommitHistoryCollection.Select(v => new Url(baseUrl + Uri.EscapeDataString(v.FilePath), v.CommitHistory.Max(v1 => v1.Date).ToString("yyyy-MM-dd"), null, null)).ToArray());

    await GenerateXmlAsync(outputFilePath, urlSet);
}

var defaultException = new InvalidOperationException();

RootCommand rootCommand = new("스크립트 명령어를 실행합니다.");

Argument<string> sourceDirPathArgument = new("source-dir-path")
{
    Description = "탐색할 디렉터리 경로입니다."
};

Argument<string> outputFilePathArgument = new("output-file-path")
{
    Description = "결과를 저장할 파일 경로입니다."
};

Option<string[]> searchPatternsOption = new("--search-patterns")
{
    Description = "일치해야하는 파일 패턴 목록입니다."
};

Option<string[]> excludePatternsOption = new("--exclude-patterns")
{
    Description = "제외해야하는 파일 패턴 목록입니다."
};

// GenerateFilePathAndCommitHistoryCollection Command
{
    var GenerateFilePathAndCommitHistoryCollectionCommand = new Command("generate-file-path-and-commit-history-collection", "파일 경로와 커밋 기록 컬렉션을 생성합니다.")
    {
        sourceDirPathArgument,
        outputFilePathArgument,
        searchPatternsOption,
        excludePatternsOption
    };

    GenerateFilePathAndCommitHistoryCollectionCommand.SetAction(
        async parseResult => await GenerateFilePathAndCommitHistoryCollectionAsync(
            parseResult.GetValue(sourceDirPathArgument) ?? throw defaultException,
            parseResult.GetValue(outputFilePathArgument) ?? throw defaultException,
            parseResult.GetValue(searchPatternsOption) ?? throw defaultException,
            parseResult.GetValue(excludePatternsOption) ?? throw defaultException
        )
    );

    rootCommand.Add(GenerateFilePathAndCommitHistoryCollectionCommand);
}

// GenerateFileTree Command
{
    var GenerateFileTreeCommand = new Command("generate-file-tree", "파일 트리를 생성합니다.")
    {
        sourceDirPathArgument,
        outputFilePathArgument,
        searchPatternsOption,
        excludePatternsOption
    };

    GenerateFileTreeCommand.SetAction(
        async parseResult => await GenerateFileTreeAsync(
            parseResult.GetValue(sourceDirPathArgument) ?? throw defaultException,
            parseResult.GetValue(outputFilePathArgument) ?? throw defaultException,
            parseResult.GetValue(searchPatternsOption) ?? throw defaultException,
            parseResult.GetValue(excludePatternsOption) ?? throw defaultException
        )
    );

    rootCommand.Add(GenerateFileTreeCommand);
}

// GenerateSitemap Command
{
    Argument<string> locPrefixArgument = new("loc-prefix")
    {
        Description = "loc 항목의 접두사입니다."
    };

    var GenerateSitemapCommand = new Command("generate-sitemap", "사이트 맵을 생성합니다.")
    {
        sourceDirPathArgument,
        outputFilePathArgument,
        locPrefixArgument,
        searchPatternsOption,
        excludePatternsOption
    };

    GenerateSitemapCommand.SetAction(
        parseResult => GenerateSitemapAsync(
            parseResult.GetValue(sourceDirPathArgument) ?? throw defaultException,
            parseResult.GetValue(outputFilePathArgument) ?? throw defaultException,
            parseResult.GetValue(locPrefixArgument) ?? throw defaultException,
            parseResult.GetValue(searchPatternsOption) ?? throw defaultException,
            parseResult.GetValue(excludePatternsOption) ?? throw defaultException
        )
    );

    rootCommand.Add(GenerateSitemapCommand);
}

return await rootCommand.Parse(args).InvokeAsync();
