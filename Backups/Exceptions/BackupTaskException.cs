using Backups.Entities;

namespace Backups.Exceptions;

public class BackupTaskException : BackupsException
{
    private BackupTaskException(string message)
        : base(message)
    {
    }

    public static BackupTaskException NoSuchBackupObjectException()
    {
        return new BackupTaskException("There is no such backup object in this backup task");
    }

    public static BackupTaskException BackupObjectAlreadyExists(string path)
    {
        return new BackupTaskException($"Backup object with path {path} already exists");
    }
}