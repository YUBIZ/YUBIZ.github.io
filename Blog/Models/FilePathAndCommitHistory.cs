namespace Blog.Models;

public readonly record struct FilePathAndCommitHistory(string FilePath, CommitMetadata[] CommitHistory);
