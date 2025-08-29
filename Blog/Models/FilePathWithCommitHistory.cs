namespace Blog.Models;

public readonly record struct FilePathWithCommitHistory(string FilePath, CommitMetadata[] CommitHistory);
