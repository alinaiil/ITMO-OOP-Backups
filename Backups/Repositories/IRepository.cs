using Backups.RepoObjects;

namespace Backups.Repositories;

public interface IRepository
{
    string Path { get; }
    IRepoObject GetRepoObject(string path);
    Stream OpenWrite(string path);
    IRepository CreateSubRepository(string name);
}