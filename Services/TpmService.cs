using System.Management;

namespace Win11Inspector.Services;

public static class TpmService
{
    public static (bool Present, string Version) Get()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    @"root\CIMV2\Security\MicrosoftTpm",
                    "SELECT SpecVersion FROM Win32_Tpm");

            foreach (ManagementObject item in searcher.Get())
            {
                var specification =
                    item["SpecVersion"]?.ToString()
                    ?? "unknown";

                var version =
                    specification
                        .Split(';')[0]
                        .Trim();

                return (true, version);
            }
        }
        catch
        {
        }

        return (false, "");
    }
}