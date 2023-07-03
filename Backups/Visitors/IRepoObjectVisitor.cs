using Backups.RepoObjects;
using Backups.ZipObjects;

namespace Backups.Visitors;

public interface IRepoObjectVisitor
{
    void Visit(IFolder folder);
    void Visit(IFile file);
}