using Backups.Exceptions;
using Backups.RepoObjects;
using Backups.Repositories;
using Zio;
using Zio.FileSystems;

namespace Backups.Test;

public class InMemoryRepository : IRepository
{
    private Func<string, Stream> _fileFunc;
    private Func<string, List<IRepoObject>> _folderFunc;

    public InMemoryRepository(string path, MemoryFileSystem memorySystem)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        ArgumentNullException.ThrowIfNull(memorySystem);

        Path = path;
        MemorySystem = memorySystem;
        _fileFunc = GetStreamForFile;
        _folderFunc = GetRepoObjectsForFolder;
    }

    public string Path { get; }
    public MemoryFileSystem MemorySystem { get; }

    public IRepoObject GetRepoObject(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        if (MemorySystem.FileExists(path))
        {
            return new RepoFile(System.IO.Path.GetFileName(path), () => _fileFunc(path));
        }

        if (MemorySystem.DirectoryExists(path))
        {
            return new RepoFolder(System.IO.Path.GetFileName(path), () => _folderFunc(path));
        }

        throw RepositoryException.InvalidPathException(path.ToString());
    }

    public Stream OpenWrite(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        return MemorySystem.OpenFile(@$"{this.Path}\{path}.zip", FileMode.Create, FileAccess.ReadWrite);
    }

    public Stream OpenRead(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        return MemorySystem.OpenFile(path, FileMode.Open, FileAccess.Read);
    }

    public IRepository CreateSubRepository(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(name);
        }

        MemorySystem.CreateDirectory(@$"{Path}\{name}");
        return new InMemoryRepository(@$"{Path}\{name}", MemorySystem);
    }

    public override bool Equals(object? obj)
    {
        return obj is InMemoryRepository that && this.Path.Equals(that.Path);
    }

    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    private Stream GetStreamForFile(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        return MemorySystem.OpenFile(path, FileMode.Open, FileAccess.ReadWrite);
    }

    private List<IRepoObject> GetRepoObjectsForFolder(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        var paths = MemorySystem.EnumeratePaths(path, "*.*", SearchOption.TopDirectoryOnly, SearchTarget.Both);
        return paths.Select(pathInFolder => this.GetRepoObject(pathInFolder.ToString())).ToList();
    }
}