namespace Backups.Exceptions;

public class RepoObjectException : BackupsException
{
    private RepoObjectException(string message)
        : base(message)
    {
    }

    public static RepoObjectException FolderNullNameException()
    {
        return new RepoObjectException($"Folder name cannot be null");
    }

    public static RepoObjectException FileNullNameException()
    {
        return new RepoObjectException($"File name cannot be null");
    }
}