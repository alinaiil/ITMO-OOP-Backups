using System.IO.Compression;
using Backups.RepoObjects;

namespace Backups.Storages;

public interface IStorage
{
    IReadOnlyCollection<IRepoObject> GetRepoObjects();
}