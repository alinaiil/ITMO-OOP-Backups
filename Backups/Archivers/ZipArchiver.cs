using System.IO.Compression;
using Backups.RepoObjects;
using Backups.Repositories;
using Backups.Storages;
using Backups.Visitors;
using Backups.ZipObjects;

namespace Backups.Archivers;

public class Archiver : IArchiver
{
    public void Archive(IRepoObject repoObject, IRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repoObject);
        ArgumentNullException.ThrowIfNull(repository);
        Stream stream = repository.OpenWrite(repoObject.Name);
        using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);
        var visitor = new ZipVisitor(zipArchive);
        repoObject.Accept(visitor);

        // var zipFolder = new ZipObjectFolder(visitor.GetZipObjects(), repoObject.Name);
        // var zipStorage = new ZipStorage(zipFolder, repository, @$"{repository.Path}/{repoObject.Name}.zip");
        // return zipStorage;
    }

    public IStorage Archive(IReadOnlyCollection<IRepoObject> repoObjects, IRepository repository, string name)
    {
        ArgumentNullException.ThrowIfNull(repoObjects);
        ArgumentNullException.ThrowIfNull(repository);
        Stream stream = repository.OpenWrite(name);
        using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);
        var visitor = new ZipVisitor(zipArchive);
        foreach (IRepoObject repoObject in repoObjects)
        {
            repoObject.Accept(visitor);
        }

        var zipFolders = visitor.GetZipObjects();
        var zipFolder = new ZipObjectFolder(zipFolders, $"{name}.zip");
        var zipStorage = new ZipStorage(zipFolder, repository, name);
        return zipStorage;
    }
}