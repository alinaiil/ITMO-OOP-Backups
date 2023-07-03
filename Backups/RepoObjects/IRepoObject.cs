using Backups.Visitors;
using Backups.ZipObjects;

namespace Backups.RepoObjects;

public interface IRepoObject
{
    string Name { get; }
    void Accept(IRepoObjectVisitor repoObjectVisitor);
}