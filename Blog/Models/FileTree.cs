namespace Blog.Models;

public readonly record struct FileTree(string Name, FileTree[] SubTrees, string[] Files);
