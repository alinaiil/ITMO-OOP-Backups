using System.IO.Compression;
using Backups.RepoObjects;

namespace Backups.ZipObjects;

public interface IZipObject
{
    string Name { get; }
    IRepoObject GetRepoObject(ZipArchiveEntry entry);
}