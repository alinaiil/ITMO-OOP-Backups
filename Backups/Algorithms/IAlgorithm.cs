using Backups.Archivers;
using Backups.Entities;
using Backups.RepoObjects;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public interface IAlgorithm
{
    IStorage Run(IReadOnlyList<IBackupObject> backupObjects, IRepository repository, IArchiver archiver);
}