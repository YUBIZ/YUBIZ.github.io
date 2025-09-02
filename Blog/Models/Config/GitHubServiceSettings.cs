namespace Blog.Models.Config;

public record GitHubServiceSettings(string Owner, string Repo, string Ref, string DocumentFileTreeUri, string DocumentFilePathAndCommitHistoryCollectionUri);
