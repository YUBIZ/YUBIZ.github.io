namespace Blog.Models;

public readonly record struct Document(string Title, string ThumbnailUri, string[] Tags, string[] Authors, DateTime[] Timestamps);
