using Backups.Algorithms;
using Backups.Archivers;
using Backups.Entities;
using Backups.Repositories;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Test;

public class Test
{
    [Fact]
    public void InMemoryRepositoryTest()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.CreateDirectory("/repository");
        memoryFileSystem.WriteAllText("/repository/a.txt", "Hi, my name is A");
        memoryFileSystem.CreateDirectory("/repository/c");
        memoryFileSystem.WriteAllText("/repository/c/c.txt", "Hi, my name is C");
        var repositoryToRead = new InMemoryRepository("/repository", memoryFileSystem);
        var repositoryToWrite = new InMemoryRepository("/backups", memoryFileSystem);
        var backupObjectA = new BackupObject("/repository/a.txt", repositoryToRead);
        var backupObjectC = new BackupObject("/repository/c", repositoryToRead);
        var backupTask = new BackupTask(new List<IBackupObject>(), "Test Name", repositoryToWrite, new SplitStorageAlgorithm(), new Archiver(), new Backup());
        backupTask.AddBackupObject(backupObjectA);
        backupTask.AddBackupObject(backupObjectC);
        backupTask.Run();
        backupTask.DeleteBackupObject(backupObjectC);
        backupTask.Run();
        Assert.Equal(2, backupTask.GetBackup().GetRestorePoints().Count);
        int storages = backupTask.GetBackup().GetRestorePoints().Sum(restorePoint =>
            memoryFileSystem.EnumeratePaths($"/backups/{backupTask.Name}/{restorePoint.Name}", "*.zip", SearchOption.TopDirectoryOnly, SearchTarget.File).Count());
        Assert.Equal(3, storages);
    }

    [Fact(Skip = "Test works only on a file system")]
    public void FileSystemRepositoryTest()
    {
        var repositoryToRead =
            new FileSystemRepository(@"C:\Users\alina\Desktop\repository");
        var repositoryToWrite =
            new FileSystemRepository(@"C:\Users\alina\Desktop\backups");
        var backupObjectA =
            new BackupObject(@"C:\Users\alina\Desktop\repository\a.txt", repositoryToRead);
        var backupObjectC = new BackupObject(@"C:\Users\alina\Desktop\repository\c", repositoryToRead);
        var backupTask2 = new BackupTask(new List<IBackupObject>(), "Single1", repositoryToWrite, new SingleStorageAlgorithm(), new Archiver(), new Backup());
        backupTask2.AddBackupObject(backupObjectA);
        backupTask2.AddBackupObject(backupObjectC);
        backupTask2.Run();
    }
}