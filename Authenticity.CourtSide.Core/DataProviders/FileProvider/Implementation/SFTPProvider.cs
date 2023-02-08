using Authenticity.CourtSide.Core.Models;
using Renci.SshNet;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Authenticity.CourtSide.Core.DataProviders.FileProvider.Implementation
{
    public class SFTPProvider : IFileProvider
    {
        public SFTPProvider(SftpConfiguration defaultStpConfiguration, ILogger<SFTPProvider> logger)
        {
            DefaultStpConfiguration = defaultStpConfiguration;
            Logger = logger;
        }
        public SftpConfiguration DefaultStpConfiguration { get; }
        private ILogger<SFTPProvider> Logger { get; }

        public async Task DeleteFileAsync(string fileName)
        {
            await Task.Yield();
            using (var client = new SftpClient(DefaultStpConfiguration.Uri, DefaultStpConfiguration.Port, DefaultStpConfiguration.Username, DefaultStpConfiguration.Password))
            {
                try
                {
                    client.Connect();
                    client.DeleteFile($"/{DefaultStpConfiguration.RootFolder}/{fileName}");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex.Message, $"Deleting file from { DefaultStpConfiguration.Uri}", ex);
                }
                finally
                {
                    client.Disconnect();
                }
            }
        }

        public async Task<FileStream> GetFileStreamAsync(string fileName, string destinationFilePath)
        {

            using (var client = new SftpClient(DefaultStpConfiguration.Uri, DefaultStpConfiguration.Port, DefaultStpConfiguration.Username, DefaultStpConfiguration.Password))
            using (FileStream fileStream = new FileStream(destinationFilePath, FileMode.Create))
            {
                try
                {
                    client.Connect();
                    client.DownloadFile($"/{DefaultStpConfiguration.RootFolder}/{fileName}", fileStream);
                    await fileStream.FlushAsync().ConfigureAwait(false);
                    fileStream.Close();
                    return fileStream;

                }
                catch (Exception ex)
                {
                    if (File.Exists(destinationFilePath))
                    {
                        File.Delete(destinationFilePath);
                    }
                    throw ex;
                }
                finally
                {
                    client.Disconnect();
                }
            }
        }

        public async Task<string> GetFileToLocalTempDirectoryAsync(string ftpRoute, string destinationFileName)
        {
            await Task.Yield();

            var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP");
            var filePath = Path.Combine(tempDirectoryPath, destinationFileName);

            using (var client = new SftpClient(DefaultStpConfiguration.Uri, DefaultStpConfiguration.Port, DefaultStpConfiguration.Username, DefaultStpConfiguration.Password))
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    client.Connect();
                    client.DownloadFile($"/{DefaultStpConfiguration.RootFolder}/{ftpRoute}", fileStream);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    client.Disconnect();
                }

            }
            return filePath;

        }

        public async Task<string> SaveFileAsync(string sourceFilePath, string targetFileName)
        {
            await Task.Yield();
            if (string.IsNullOrEmpty(targetFileName))
            {
                targetFileName = Guid.NewGuid().ToString();
            }

            using (var client = new SftpClient(DefaultStpConfiguration.Uri, DefaultStpConfiguration.Port, DefaultStpConfiguration.Username, DefaultStpConfiguration.Password))
            using (FileStream fileStream = new FileStream(sourceFilePath, FileMode.Open))
            {
                client.KeepAliveInterval = TimeSpan.FromSeconds(600);
                client.ConnectionInfo.Timeout = TimeSpan.FromMinutes(20);
                client.OperationTimeout = TimeSpan.FromMinutes(20);
                try
                {
                    client.Connect();
                    client.UploadFile(fileStream, $"/{DefaultStpConfiguration.RootFolder}/{targetFileName}");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"SFTP ERROR {ex.Message}");
                    throw ex;
                }
                finally
                {
                    client.Disconnect();
                }
            }

            return targetFileName;
        }
    }
}
