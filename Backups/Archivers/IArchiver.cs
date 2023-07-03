using Backups.RepoObjects;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Archivers;

public interface IArchiver
{
    IStorage Archive(IReadOnlyCollection<IRepoObject> repoObjects, IRepository repository, string name);
}