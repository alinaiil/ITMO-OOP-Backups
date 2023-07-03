namespace Backups.RepoObjects;

public interface IFolder : IRepoObject
{
    IReadOnlyList<IRepoObject> RepoObjects { get; }
}