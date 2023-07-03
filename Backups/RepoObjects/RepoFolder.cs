using Backups.Exceptions;
using Backups.Visitors;
using Backups.ZipObjects;

namespace Backups.RepoObjects;

public class RepoFolder : IFolder
{
    private Func<List<IRepoObject>> _folderFunc;

    public RepoFolder(string? name, Func<List<IRepoObject>> folderFunc)
    {
        ArgumentNullException.ThrowIfNull(folderFunc);
        Name = name ?? throw RepoObjectException.FolderNullNameException();
        _folderFunc = folderFunc;
    }

    public IReadOnlyList<IRepoObject> RepoObjects => _folderFunc.Invoke();
    public string Name { get; }

    public void Accept(IRepoObjectVisitor repoObjectVisitor)
    {
        ArgumentNullException.ThrowIfNull(repoObjectVisitor);
        repoObjectVisitor.Visit(this);
    }
}