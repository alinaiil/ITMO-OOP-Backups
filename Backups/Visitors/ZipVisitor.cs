using System.IO.Compression;
using Backups.RepoObjects;
using Backups.ZipObjects;

namespace Backups.Visitors;

public class ZipVisitor : IRepoObjectVisitor
{
    private Stack<ZipArchive> _archives = new Stack<ZipArchive>();
    private Stack<List<IZipObject>> _zipObjects = new Stack<List<IZipObject>>();

    public ZipVisitor(ZipArchive archive)
    {
        _archives.Push(archive);
        _zipObjects.Push(new List<IZipObject>());
    }

    public IReadOnlyCollection<IZipObject> GetZipObjects() => _zipObjects.Peek().AsReadOnly();

    public void Visit(IFile file)
    {
        ZipArchive archive = _archives.Peek();
        ZipArchiveEntry entry = archive.CreateEntry(file.Name);
        using Stream entryStream = entry.Open();
        using Stream stream = file.Stream;
        stream.CopyTo(entryStream);
        var zipFile = new ZipObjectFile(file.Name);
        _zipObjects.Peek().Add(zipFile);
    }

    public void Visit(IFolder folder)
    {
        ZipArchive archive = _archives.Peek();
        ZipArchiveEntry entry = archive.CreateEntry(@$"{folder.Name}.zip");
        using Stream entryStream = entry.Open();
        using var newArchive = new ZipArchive(entryStream, ZipArchiveMode.Create);
        _archives.Push(newArchive);
        _zipObjects.Push(new List<IZipObject>());
        foreach (IRepoObject repoObject in folder.RepoObjects)
        {
            repoObject.Accept(this);
        }

        var zipFolder = new ZipObjectFolder(_zipObjects.Pop(), folder.Name);
        _zipObjects.Push(new List<IZipObject>
        {
            zipFolder,
        });
        _archives.Pop();
    }
}