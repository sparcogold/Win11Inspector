using System.Windows;
using Win11Inspector.Models;
using Win11Inspector.Services;

namespace Win11Inspector;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            RunScan();
        };
    }

    private void RunScan()
    {
        var results =
            CompatibilityScanner.Scan();

        ResultList.ItemsSource =
            results;

        var pass =
            results.Count(x =>
                x.State == CheckState.Pass);

        var warning =
            results.Count(x =>
                x.State == CheckState.Warning);

        var fail =
            results.Count(x =>
                x.State == CheckState.Fail);

        var score =
            (int)Math.Round(
                pass * 100.0 /
                Math.Max(results.Count, 1));

        ScoreText.Text =
            $"{score}%";

        DeviceText.Text =
            HardwareService.GetDeviceDescription();

        SummaryText.Text =
            $"{pass} speĹ‚nionych  â€˘  " +
            $"{warning} do sprawdzenia  â€˘  " +
            $"{fail} niespeĹ‚nionych";
    }

    private void Rescan_Click(
        object sender,
        RoutedEventArgs e)
    {
        RunScan();
    }

    private void Prepare_Click(
        object sender,
        RoutedEventArgs e)
    {
        var failed =
            CompatibilityScanner
                .Scan()
                .Where(x =>
                    x.State == CheckState.Fail)
                .ToList();

        if (failed.Count == 0)
        {
            MessageBox.Show(
                "Nie wykryto podstawowych blokad. " +
                "MoĹĽesz przejĹ›Ä‡ do oficjalnego " +
                "procesu instalacji Windows 11.",
                "Gotowe",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return;
        }

        var result =
            MessageBox.Show(
                "Wykryto wymagania, ktĂłrych komputer " +
                "nie speĹ‚nia.\n\n" +
                "Instalacja z obejĹ›ciami moĹĽe byÄ‡ " +
                "nieobsĹ‚ugiwana przez Microsoft i " +
                "moĹĽe mieÄ‡ ograniczenia dotyczÄ…ce " +
                "aktualizacji lub funkcji bezpieczeĹ„stwa.\n\n" +
                "Czy chcesz otworzyÄ‡ kreator przygotowania?",
                "Instalacja nieobsĹ‚ugiwana",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            MessageBox.Show(
                "W tej wersji aplikacja wykonuje " +
                "wyĹ‚Ä…cznie diagnostykÄ™.\n\n" +
                "Nie sÄ… modyfikowane ustawienia TPM, " +
                "Secure Boot, bootowania ani rejestru.",
                "Win11 Inspector",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}