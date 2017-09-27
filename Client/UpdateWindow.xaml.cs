using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using MatchmakingMonitor.Services;

namespace MatchmakingMonitor
{
  /// <summary>
  ///   Interaction logic for UpdateWindow.xaml
  /// </summary>
  public partial class UpdateWindow
  {
    private readonly string _executableBackupPath =
      Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchmakingMonitor.exe.bak");

    private readonly string _executablePath =
      Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchmakingMonitor.exe");

    public UpdateWindow(Uri downloadUri)
    {
      InitializeComponent();

      DownloadInstall(downloadUri, IoCKernel.Get<ILogger>());
    }

    private async void DownloadInstall(Uri downloadUri, ILogger logger)
    {
      try
      {
        var client = new WebClient();
        client.DownloadProgressChanged += (sender, args) =>
        {
          var bytesIn = double.Parse(args.BytesReceived.ToString());
          var totalBytes = double.Parse(args.TotalBytesToReceive.ToString());
          var percentage = bytesIn / totalBytes * 100;
          ProgressText.Text = $"{percentage} / 100";
          ProgressBar.Value = int.Parse(Math.Truncate(percentage).ToString(CultureInfo.InvariantCulture));
        };
        if (File.Exists(_executableBackupPath))
          File.Delete(_executableBackupPath);
        File.Move(_executablePath, _executableBackupPath);
        File.Copy(_executableBackupPath, _executablePath);
        var bytes = await client.DownloadDataTaskAsync(downloadUri);
        await Task.Run(() =>
        {
          using (var ms = new MemoryStream(bytes))
          {
            var zipArchive = new ZipArchive(ms);
            foreach (var entry in zipArchive.Entries)
            {
              var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, entry.FullName);
              // ReSharper disable once AssignNullToNotNullAttribute
              Directory.CreateDirectory(Path.GetDirectoryName(filePath));
              if (!string.IsNullOrEmpty(Path.GetFileName(filePath)))
                entry.ExtractToFile(filePath, true);
            }
          }
        });
        var messageBoxResult = MessageBox.Show("Application has been updated to the latest version. Restart required.",
          "Application updated", MessageBoxButton.OK);
        if (messageBoxResult != MessageBoxResult.OK) return;
        if (Application.ResourceAssembly.Location != null) Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
      }
      catch (Exception e)
      {
        logger.Error("Error during automatic app update", e);
        var messageBoxResult =
          MessageBox.Show(
            "An error occured when trying to update the app. Try disabling automatic update and download the latest version manually",
            "Update error", MessageBoxButton.OK);
        if (messageBoxResult != MessageBoxResult.OK) return;
        Close();
      }
    }
  }
}