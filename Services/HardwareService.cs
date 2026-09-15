using System.Management;

namespace Win11Inspector.Services;

public static class HardwareService
{
    public static string GetDeviceDescription()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Manufacturer, Model FROM Win32_ComputerSystem");

            foreach (ManagementObject item in searcher.Get())
            {
                var manufacturer =
                    item["Manufacturer"]?.ToString() ?? "";

                var model =
                    item["Model"]?.ToString() ?? "";

                return $"{manufacturer} {model}".Trim();
            }
        }
        catch
        {
        }

        return "Nieznane urzÄ…dzenie";
    }

    public static string GetCpu()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Name FROM Win32_Processor");

            foreach (ManagementObject item in searcher.Get())
            {
                return item["Name"]?.ToString()
                       ?? "Nieznany";
            }
        }
        catch
        {
        }

        return "Nieznany";
    }

    public static double GetRamGb()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");

            foreach (ManagementObject item in searcher.Get())
            {
                return Convert.ToDouble(
                    item["TotalVisibleMemorySize"])
                    / 1024
                    / 1024;
            }
        }
        catch
        {
        }

        return 0;
    }

    public static string GetGpu()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Name FROM Win32_VideoController");

            var names = new List<string>();

            foreach (ManagementObject item in searcher.Get())
            {
                if (item["Name"] is string name &&
                    !string.IsNullOrWhiteSpace(name))
                {
                    names.Add(name);
                }
            }

            return string.Join(", ", names);
        }
        catch
        {
        }

        return "Nieznana";
    }

    public static string GetWindowsVersion()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Caption, Version, BuildNumber FROM Win32_OperatingSystem");

            foreach (ManagementObject item in searcher.Get())
            {
                return $"{item["Caption"]} " +
                       $"({item["Version"]}, build {item["BuildNumber"]})";
            }
        }
        catch
        {
        }

        return Environment.OSVersion.VersionString;
    }
}