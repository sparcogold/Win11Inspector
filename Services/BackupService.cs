namespace Win11Inspector.Services;

public static class BackupService
{
    public static string CreateBackup(
        string directory)
    {
        Directory.CreateDirectory(directory);

        var filename =
            $"Win11Inspector-backup-" +
            $"{DateTime.Now:yyyyMMdd-HHmmss}.txt";

        var path =
            Path.Combine(directory, filename);

        File.WriteAllText(
            path,
            """
            Win11 Inspector backup

            Ta wersja programu wykonuje wyĹ‚Ä…cznie
            diagnostykÄ™ i nie modyfikuje automatycznie
            ustawieĹ„ bootowania, TPM, Secure Boot
            ani rejestru.
            """);

        return path;
    }
}