using System.IO.Compression;
using Backups.Exceptions;
using Backups.RepoObjects;
using File = System.IO.File;

namespace Backups.Repositories;

public class FileSystemRepository : IRepository
{
    private Func<string, Stream> _fileFunc;
    private Func<string, List<IRepoObject>> _folderFunc;

    public FileSystemRepository(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        Path = path ?? throw new ArgumentNullException(path);
        _fileFunc = GetStreamForFile;
        _folderFunc = GetRepoObjectsForFolder;
    }

    public string Path { get; }

    public IRepoObject GetRepoObject(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        if (File.Exists(path))
        {
            return new RepoFile(System.IO.Path.GetFileName(path), () => _fileFunc(path));
        }

        if (Directory.Exists(path))
        {
            return new RepoFolder(System.IO.Path.GetFileName(path), () => _folderFunc(path));
        }

        throw RepositoryException.InvalidPathException(path);
    }

    public Stream OpenWrite(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        Stream stream = new FileStream(@$"{Path}\{path}.zip", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        return stream;
    }

    public Stream OpenRead(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        return File.OpenRead(path);
    }

    public IRepository CreateSubRepository(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(name);
        }

        Directory.CreateDirectory(@$"{Path}\{name}");
        return new FileSystemRepository(@$"{Path}\{name}");
    }

    public override bool Equals(object? obj)
    {
        return obj is FileSystemRepository that && this.Path.Equals(that.Path);
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

        return new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
    }

    private List<IRepoObject> GetRepoObjectsForFolder(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        var repoObjects = Directory.GetDirectories(path).Select(subDirPath => GetRepoObject(subDirPath)).ToList();
        repoObjects.AddRange(Directory.GetFiles(path).Select(filePath => GetRepoObject(filePath)));
        return repoObjects;
    }
}