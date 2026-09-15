using System.IO;
using Win11Inspector.Models;

namespace Win11Inspector.Services;

public static class CompatibilityScanner
{
    public static List<CheckResult> Scan()
    {
        var results = new List<CheckResult>();

        CheckMemory(results);
        CheckDisk(results);
        CheckUefi(results);
        CheckSecureBoot(results);
        CheckTpm(results);
        CheckCpu(results);
        CheckGpu(results);
        CheckDevice(results);
        CheckWindows(results);

        return results;
    }

    private static void CheckMemory(
        List<CheckResult> results)
    {
        var ram = HardwareService.GetRamGb();

        results.Add(new CheckResult(
            "PamiÄ™Ä‡ RAM",
            $"{ram:0.0} GB",
            ram >= 4
                ? CheckState.Pass
                : CheckState.Fail,
            ram >= 4
                ? "Minimum 4 GB RAM jest speĹ‚nione."
                : "Windows 11 wymaga minimum 4 GB RAM."));
    }

    private static void CheckDisk(
        List<CheckResult> results)
    {
        var disk = GetSystemDiskGb();

        results.Add(new CheckResult(
            "Dysk systemowy",
            $"{disk:0.0} GB",
            disk >= 64
                ? CheckState.Pass
                : CheckState.Fail,
            disk >= 64
                ? "Minimum 64 GB jest speĹ‚nione."
                : "Windows 11 wymaga minimum 64 GB pamiÄ™ci."));
    }

    private static void CheckUefi(
        List<CheckResult> results)
    {
        var uefi = BootService.IsUefi();

        results.Add(new CheckResult(
            "UEFI",
            uefi ? "Aktywne" : "Nie wykryto",
            uefi
                ? CheckState.Pass
                : CheckState.Fail,
            uefi
                ? "System uruchomiono w trybie UEFI."
                : "System nie zostaĹ‚ wykryty jako uruchomiony w UEFI."));
    }

    private static void CheckSecureBoot(
        List<CheckResult> results)
    {
        var secureBoot =
            BootService.IsSecureBootEnabled();

        results.Add(new CheckResult(
            "Secure Boot",
            secureBoot
                ? "WĹ‚Ä…czony"
                : "WyĹ‚Ä…czony / nieznany",
            secureBoot
                ? CheckState.Pass
                : CheckState.Warning,
            secureBoot
                ? "Secure Boot jest aktywny."
                : "Secure Boot nie jest aktywny lub jego stan jest niedostÄ™pny."));
    }

    private static void CheckTpm(
        List<CheckResult> results)
    {
        var tpm = TpmService.Get();

        var compatible =
            tpm.Present &&
            tpm.Version.StartsWith(
                "2.",
                StringComparison.OrdinalIgnoreCase);

        results.Add(new CheckResult(
            "TPM 2.0",
            tpm.Present
                ? tpm.Version
                : "Nie wykryto",
            compatible
                ? CheckState.Pass
                : CheckState.Fail,
            compatible
                ? "TPM 2.0 zostaĹ‚ wykryty."
                : "Nie wykryto TPM 2.0. Instalacja z obejĹ›ciem moĹĽe byÄ‡ nieobsĹ‚ugiwana przez Microsoft.",
            true));
    }

    private static void CheckCpu(
        List<CheckResult> results)
    {
        var cpu = HardwareService.GetCpu();

        results.Add(new CheckResult(
            "Procesor",
            cpu,
            CheckState.Warning,
            "Model zostaĹ‚ wykryty. PeĹ‚na zgodnoĹ›Ä‡ wymaga porĂłwnania z aktualnÄ… listÄ… obsĹ‚ugiwanych procesorĂłw."));
    }

    private static void CheckGpu(
        List<CheckResult> results)
    {
        var gpu = HardwareService.GetGpu();

        results.Add(new CheckResult(
            "Grafika",
            gpu,
            CheckState.Warning,
            "Adapter graficzny zostaĹ‚ wykryty. SzczegĂłĹ‚owa walidacja DirectX i WDDM wymaga dodatkowej weryfikacji."));
    }

    private static void CheckDevice(
        List<CheckResult> results)
    {
        var device =
            HardwareService.GetDeviceDescription();

        if (device.Contains(
                "Apple",
                StringComparison.OrdinalIgnoreCase) ||
            device.Contains(
                "Mac",
                StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new CheckResult(
                "UrzÄ…dzenie Apple",
                device,
                CheckState.Pass,
                "Wykryto komputer Apple. Program uwzglÄ™dnia specyfikÄ™ Boot Camp."));
        }
    }

    private static void CheckWindows(
        List<CheckResult> results)
    {
        results.Add(new CheckResult(
            "Windows",
            HardwareService.GetWindowsVersion(),
            CheckState.Pass,
            "System operacyjny zostaĹ‚ wykryty."));
    }

    private static double GetSystemDiskGb()
    {
        try
        {
            var root =
                Path.GetPathRoot(
                    Environment.SystemDirectory)
                ?? "C:\\";

            var drive =
                new DriveInfo(root);

            return drive.TotalSize /
                   1_000_000_000d;
        }
        catch
        {
            return 0;
        }
    }
}