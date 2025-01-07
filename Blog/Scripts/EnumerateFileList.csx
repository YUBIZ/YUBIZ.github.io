#nullable enable

#r "nuget:System.CommandLine, 2.0.0-beta4.22272.1"

#load "Common.csx"

using System.CommandLine;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

static void EnumerateFileList(string sourceDir, string outputPath, string[] searchPatterns, string[] excludePatterns, string? stripPath)
{
    IEnumerable<string> files = GetFiles(sourceDir, searchPatterns, excludePatterns, SearchOption.AllDirectories)
                               .Select(f => Path.GetRelativePath(stripPath ?? sourceDir, f));

    JsonSerializerOptions jsonSerializerOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

    string outputDir = Path.GetDirectoryName(outputPath) ?? throw new("출력 디렉터리가 null입니다.");
    if (!Directory.Exists(outputDir))
    {
        Directory.CreateDirectory(outputDir);
    }

    File.WriteAllText(outputPath, JsonSerializer.Serialize(files, jsonSerializerOptions));
}

Argument<string> argument = new("source-dir", "탐색할 디렉터리입니다.");
Argument<string> argument1 = new("output-path", "결과를 저장할 JSON 파일입니다.");

Option<string[]> option = new("--search-patterns", "일치해야하는 파일 패턴 목록입니다.");
Option<string[]> option1 = new("--exclude-patterns", "제외해야하는 파일 패턴 목록입니다.");
Option<string?> option2 = new("--strip-path", "파일 경로에서 제거할 경로입니다.");

RootCommand rootCommand = new("source 내 모든 파일의 목록을 JSON 형식으로 output에 출력합니다.")
{
    argument,
    argument1,
    option,
    option1,
    option2
};

rootCommand.SetHandler(EnumerateFileList, argument, argument1, option, option1, option2);

return await rootCommand.InvokeAsync([.. Args]);
