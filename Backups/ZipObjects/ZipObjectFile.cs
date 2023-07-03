using System.IO.Compression;
using Backups.Exceptions;
using Backups.RepoObjects;

namespace Backups.ZipObjects;

public class ZipObjectFile : IZipObject
{
    public ZipObjectFile(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(name);
        }

        Name = name;
    }

    public string Name { get; }

    public IRepoObject GetRepoObject(ZipArchiveEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        if (entry.Name != Name)
        {
            throw ZipObjectException.EntryNameIsDifferentException(Name, entry.Name);
        }

        return new RepoFile(entry.Name, () => entry.Open());
    }
}