namespace Models;

public readonly record struct FilePathAndCommitHistory(string FilePath, CommitMetadata[] CommitHistory);
