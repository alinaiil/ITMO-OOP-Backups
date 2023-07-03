using Backups.Archivers;
using Backups.Entities;
using Backups.RepoObjects;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IAlgorithm
{
    public IStorage Run(IReadOnlyList<IBackupObject> backupObjects, IRepository repository, IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(backupObjects);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(archiver);
        var repoObjects = backupObjects.Select(backupObject => backupObject.Repository.GetRepoObject(backupObject.Path)).ToList();
        var zipStorages = backupObjects.Select(backupObject =>
            archiver.Archive(new[] { backupObject.Repository.GetRepoObject(backupObject.Path) }, repository, backupObject.Name)).ToList();
        return new SplitStorage(zipStorages);
    }
}