using Backups.RepoObjects;
using Backups.Repositories;

namespace Backups.Entities;

public interface IBackupObject
{
    string Path { get; }
    string Name { get; }
    IRepository Repository { get; }
    IRepoObject GetRepoObject();
}