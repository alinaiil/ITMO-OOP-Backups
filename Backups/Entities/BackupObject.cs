using Backups.RepoObjects;
using Backups.Repositories;

namespace Backups.Entities;

public class BackupObject : IBackupObject
{
    public BackupObject(string path, IRepository repository)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        ArgumentNullException.ThrowIfNull(repository);

        Path = path;
        Name = System.IO.Path.GetFileName(path);
        Repository = repository;
    }

    public string Path { get; }
    public string Name { get; }
    public IRepository Repository { get; }

    public IRepoObject GetRepoObject()
    {
        return Repository.GetRepoObject(Path);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BackupObject that) return false;
        return this.Path.Equals(that.Path) && this.Repository.Equals(that.Repository);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Path.GetHashCode(), Repository.GetHashCode());
    }
}