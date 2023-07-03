using Backups.Exceptions;
using Backups.Storages;

namespace Backups.Entities;

public class RestorePoint
{
    public RestorePoint(IReadOnlyCollection<IBackupObject> backupObjects, DateTime date, IStorage storage, string name)
    {
        ArgumentNullException.ThrowIfNull(backupObjects);
        ArgumentNullException.ThrowIfNull(date);
        ArgumentNullException.ThrowIfNull(storage);
        BackupObjects = backupObjects;
        Date = date;
        Storage = storage;
        Name = name;
    }

    public IReadOnlyCollection<IBackupObject> BackupObjects { get; }
    public DateTime Date { get; }
    public IStorage Storage { get; }
    public string Name { get; }
}