using Backups.Exceptions;

namespace Backups.Entities;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IReadOnlyList<RestorePoint> GetRestorePoints() => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);
        _restorePoints.Add(restorePoint);
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);
        if (!_restorePoints.Remove(restorePoint))
        {
            throw BackupException.NoSuchRestorePointException();
        }
    }
}