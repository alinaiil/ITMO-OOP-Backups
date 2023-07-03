namespace Backups.Exceptions;

public class BackupException : BackupsException
{
    private BackupException(string message)
        : base(message)
    {
    }

    public static BackupException NoSuchRestorePointException()
    {
        return new BackupException("There is no such restore point registered in this backup");
    }
}