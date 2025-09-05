namespace Models;

public readonly record struct DocumentMetadata(string Title, string ThumbnailUri, string[] Tags, string[] Authors, DateTime[] Timestamps);
