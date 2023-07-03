namespace Backups.Exceptions;

public class ZipObjectException : BackupsException
{
    private ZipObjectException(string message)
        : base(message)
    {
    }

    public static ZipObjectException NoSuchZipObject(string name)
    {
        return new ZipObjectException($"Cannot find zip object with such name: {name}");
    }

    public static ZipObjectException EntryNameIsDifferentException(string fileName, string entryName)
    {
        return new ZipObjectException(
            $"Cannot get repoObject from a file {fileName}, because given entry doesn't have the same name: {entryName}");
    }
}