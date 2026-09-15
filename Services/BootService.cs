using Microsoft.Win32;

namespace Win11Inspector.Services;

public static class BootService
{
    public static bool IsUefi()
    {
        try
        {
            using var key =
                Registry.LocalMachine.OpenSubKey(
                    @"SYSTEM\CurrentControlSet\Control");

            return key?
                .GetValue("PEFirmwareType")?
                .ToString() == "2";
        }
        catch
        {
            return false;
        }
    }

    public static bool IsSecureBootEnabled()
    {
        try
        {
            using var key =
                Registry.LocalMachine.OpenSubKey(
                    @"SYSTEM\CurrentControlSet\Control\SecureBoot\State");

            return Convert.ToInt32(
                key?.GetValue("UEFISecureBootEnabled") ?? 0) == 1;
        }
        catch
        {
            return false;
        }
    }
}