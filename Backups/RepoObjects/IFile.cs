namespace Backups.RepoObjects;

public interface IFile : IRepoObject
{
    Stream Stream { get; }
}