namespace Backups.Entities;

public interface IBackup
{
    void AddRestorePoint(RestorePoint restorePoint);
    void DeleteRestorePoint(RestorePoint restorePoint);
    IReadOnlyList<RestorePoint> GetRestorePoints();
}