using Backups.Archivers;
using Backups.Entities;
using Backups.RepoObjects;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IAlgorithm
{
    public IStorage Run(IReadOnlyList<IBackupObject> backupObjects, IRepository repository, IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(backupObjects);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(archiver);
        var repoObjects = backupObjects.Select(backupObject => backupObject.Repository.GetRepoObject(backupObject.Path)).ToList();
        return archiver.Archive(repoObjects, repository, "SingleStorage");
    }
}