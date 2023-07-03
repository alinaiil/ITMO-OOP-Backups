using Backups.Algorithms;
using Backups.Archivers;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Entities;

public interface IBackupTask
{
    string Name { get; }
    IRepository RepositoryToWrite { get; }
    IAlgorithm Algorithm { get; }
    IArchiver Archiver { get; }
    IBackup GetBackup();
    void AddBackupObject(IBackupObject backupObject);
    void DeleteBackupObject(IBackupObject backupObject);
    void Run();
}