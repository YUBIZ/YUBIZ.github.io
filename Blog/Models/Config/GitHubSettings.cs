namespace Blog.Models.Config;

public record GitHubSettings(string Owner, string Repo, string Ref, string DocumentFileTreeUri, string DocumentFilePathAndCommitHistoryCollectionUri);
