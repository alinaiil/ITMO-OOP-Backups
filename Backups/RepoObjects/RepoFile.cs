using Backups.Exceptions;
using Backups.Visitors;
using Backups.ZipObjects;

namespace Backups.RepoObjects;

public class RepoFile : IFile
{
    private Func<Stream> _fileFunc;

    public RepoFile(string? name, Func<Stream> fileFunc)
    {
        Name = name ?? throw RepoObjectException.FileNullNameException();
        _fileFunc = fileFunc;
    }

    public Stream Stream => _fileFunc.Invoke();
    public string Name { get; }

    public void Accept(IRepoObjectVisitor repoObjectVisitor)
    {
        ArgumentNullException.ThrowIfNull(repoObjectVisitor);
        repoObjectVisitor.Visit(this);
    }
}