namespace Models;

public readonly record struct FileTree(DirectoryNameAndFileNames Value, FileTree[] SubTrees)
{
    public static implicit operator Tree<DirectoryNameAndFileNames>(FileTree fileTree)
        => new(fileTree.Value, fileTree.SubTrees.Select(v => (Tree<DirectoryNameAndFileNames>)v).ToArray());

    public static implicit operator FileTree(Tree<DirectoryNameAndFileNames> tree)
        => new(tree.Value, tree.SubTrees.Select(v => (FileTree)v).ToArray());
}
