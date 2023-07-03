using Backups.RepoObjects;

namespace Backups.Storages;

public class SplitStorage : IStorage
{
    public SplitStorage(List<IStorage> storages)
    {
        ArgumentNullException.ThrowIfNull(storages);
        Storages = storages;
    }

    public List<IStorage> Storages { get; }

    public IReadOnlyCollection<IRepoObject> GetRepoObjects()
    {
        var repoObjects = new List<IRepoObject>();
        repoObjects.AddRange(Storages.SelectMany(storage => storage.GetRepoObjects()));
        return repoObjects;
    }
}