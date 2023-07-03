using Backups.Algorithms;
using Backups.Archivers;
using Backups.Exceptions;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Entities;

public class BackupTask : IBackupTask
{
    private readonly List<IBackupObject> _backupObjects;
    private readonly IBackup _backup;

    public BackupTask(List<IBackupObject> backupObjects, string name, IRepository repositoryToWrite, IAlgorithm algorithm, IArchiver archiver, IBackup backup)
    {
        _backupObjects = backupObjects;
        Name = name + "_" + Guid.NewGuid();
        RepositoryToWrite = repositoryToWrite;
        Algorithm = algorithm;
        Archiver = archiver;
        _backup = backup;
    }

    public IReadOnlyList<IBackupObject> BackupObjects => _backupObjects;
    public string Name { get; }
    public IRepository RepositoryToWrite { get; }
    public IAlgorithm Algorithm { get; }
    public IArchiver Archiver { get; }
    public IBackup GetBackup() => _backup;

    public void AddBackupObject(IBackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        if (_backupObjects.Contains(backupObject))
        {
            throw BackupTaskException.BackupObjectAlreadyExists(backupObject.Path);
        }

        _backupObjects.Add(backupObject);
    }

    public void DeleteBackupObject(IBackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        if (!_backupObjects.Remove(backupObject))
        {
            throw BackupTaskException.NoSuchBackupObjectException();
        }
    }

    public void Run()
    {
        DateTime currentTime = DateTime.Now;
        IRepository taskRepository = RepositoryToWrite.CreateSubRepository(Name);
        IRepository pointRepository = taskRepository.CreateSubRepository(GetNameFromTime(currentTime));
        IStorage storage = Algorithm.Run(BackupObjects, pointRepository, Archiver);
        CreateRestorePoint(currentTime, storage);
    }

    private static string GetNameFromTime(DateTime time)
    {
        return $"{time.Year}-{time.Month}-{time.Day}_{time.Hour}-{time.Minute}-{time.Second}-{time.Millisecond}";
    }

    private void CreateRestorePoint(DateTime currentTime, IStorage storage)
    {
        var restorePoint = new RestorePoint(BackupObjects, currentTime, storage, GetNameFromTime(currentTime));
        _backup.AddRestorePoint(restorePoint);
    }
}