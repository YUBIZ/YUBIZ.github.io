#r "nuget:System.CommandLine, 2.0.0-beta4.22272.1"

#load "Common.csx"

using System.CommandLine;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public readonly record struct Commit(string Messasge, DateTime Timestamp);

static Commit[] GetCommitHistory(string filePath)
{
    ProcessStartInfo startInfo = new()
    {
        FileName = "git",
        Arguments = $"log --format=%s%n%aI --follow -- {filePath}",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true,
    };

    using Process process = new() { StartInfo = startInfo };
    process.Start();
    process.WaitForExit();
    string[] outputLines = process.StandardOutput.ReadToEnd().Trim().Split('\n');

    if (outputLines.Length == 0) return [];

    IEnumerable<string> messages = outputLines.Where((_, i) => i % 2 == 0);
    IEnumerable<DateTime> timestamps = outputLines.Where((_, i) => i % 2 != 0).Select(DateTime.Parse);

    return messages.Zip(timestamps, (message, timestamp) => new Commit(message, timestamp)).Reverse().ToArray();
}

static void EnumerateCommitHistory(string sourceDir, string outputDir, string[] searchPatterns, string[] excludePatterns)
{
    string[] files = GetFiles(sourceDir, searchPatterns, excludePatterns, SearchOption.AllDirectories);

    Queue<Commit[]> gitLogs = new(files.Select(GetCommitHistory));

    files = files.Select(f => Path.GetRelativePath(sourceDir, f)).ToArray();

    JsonSerializerOptions jsonSerializerOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

    foreach (var item in files)
    {
        string outputPath = Path.Combine(outputDir, Path.ChangeExtension(item, "json"));

        string fileOutputDir = Path.GetDirectoryName(outputPath) ?? throw new("출력 디렉터리가 null입니다.");
        if (!Directory.Exists(fileOutputDir))
        {
            Directory.CreateDirectory(fileOutputDir);
        }

        File.WriteAllText(outputPath, JsonSerializer.Serialize(gitLogs.Dequeue(), jsonSerializerOptions));
    }
}

Argument<string> argument = new("source-dir", "탐색할 디렉터리입니다.");
Argument<string> argument1 = new("output-path", "결과를 저장할 디렉터리입니다.");

Option<string[]> option = new("--search-patterns", "일치해야하는 파일 패턴입니다.");
Option<string[]> option1 = new("--exclude-patterns", "제외해야하는 파일 패턴입니다.");

RootCommand rootCommand = new("source 내 모든 파일의 메시지와 타임스탬프를 포함한 커밋 이력을 JSON 형식으로 각각 output에 출력합니다.")
{
    argument,
    argument1,
    option,
    option1
};

rootCommand.SetHandler(EnumerateCommitHistory, argument, argument1, option, option1);

return await rootCommand.InvokeAsync([.. Args]);
