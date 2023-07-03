using System.IO.Compression;
using Backups.Exceptions;
using Backups.RepoObjects;

namespace Backups.ZipObjects;

public class ZipObjectFolder : IZipObject
{
    private readonly IReadOnlyCollection<IZipObject> _zipObjects;

    public ZipObjectFolder(IReadOnlyCollection<IZipObject> folders, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(name);
        }

        ArgumentNullException.ThrowIfNull(folders);
        _zipObjects = folders;
        Name = name;
    }

    public ZipObjectFolder(IZipObject zipObject, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(name);
        }

        ArgumentNullException.ThrowIfNull(zipObject);
        _zipObjects = new List<IZipObject>
        {
            zipObject,
        };
        Name = name;
    }

    public IReadOnlyCollection<IZipObject> ZipObjects => _zipObjects;

    public string Name { get; }

    public IRepoObject GetRepoObject(ZipArchiveEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        var repoObjects = new List<IRepoObject>();
        using (var archive = new ZipArchive(entry.Open()))
        {
            foreach (ZipArchiveEntry entryInFolder in archive.Entries)
            {
                IZipObject? zipObject =
                    _zipObjects.SingleOrDefault(zipObject => zipObject.Name.Equals(entryInFolder.Name));

                if (zipObject == null)
                {
                    throw ZipObjectException.NoSuchZipObject(entryInFolder.Name);
                }

                repoObjects.Add(zipObject.GetRepoObject(entryInFolder));
            }
        }

        return new RepoFolder(entry.Name, () => repoObjects);
    }

    public List<IRepoObject> GetRepoObjects(ZipArchive archive)
    {
        ArgumentNullException.ThrowIfNull(archive);

        var repoObjects = new List<IRepoObject>();

        foreach (ZipArchiveEntry entryInArchive in archive.Entries)
        {
            IZipObject? zipObject =
                _zipObjects.SingleOrDefault(zipObject => zipObject.Name.Equals(entryInArchive.Name));

            if (zipObject == null)
            {
                throw ZipObjectException.NoSuchZipObject(entryInArchive.Name);
            }

            repoObjects.Add(zipObject.GetRepoObject(entryInArchive));
        }

        return repoObjects;
    }
}