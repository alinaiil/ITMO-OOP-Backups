using System.IO.Compression;
using Backups.RepoObjects;
using Backups.Repositories;
using Backups.ZipObjects;

namespace Backups.Storages;

public class ZipStorage : IStorage
{
    public ZipStorage(ZipObjectFolder folder, IRepository repository, string path)
    {
        ArgumentNullException.ThrowIfNull(folder);
        ArgumentNullException.ThrowIfNull(repository);
        Folder = folder;
        Repository = repository;
        Path = path;
    }

    public ZipObjectFolder Folder { get; }
    public IRepository Repository { get; }
    public string Path { get; }

    public IReadOnlyCollection<IRepoObject> GetRepoObjects()
    {
        using Stream stream = Repository.OpenWrite(Path);
        using var archive = new ZipArchive(stream);
        return Folder.GetRepoObjects(archive);
    }
}