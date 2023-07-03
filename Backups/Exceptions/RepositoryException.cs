namespace Backups.Exceptions;

public class RepositoryException : BackupsException
{
    private RepositoryException(string message)
        : base(message)
    {
    }

    public static RepositoryException InvalidPathException(string path)
    {
        return new RepositoryException($"There is no file/folder on the path: ${path}");
    }
}