using Authenticity.CourtSide.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders.FileProvider.Implementation
{
	public class FTPProvider : IFileProvider
    {
        private const int BUFFER_SIZE = 65535;

        public FTPProvider(FtpConfiguration defaultFtpConfiguration, ILogger<FTPProvider> logger)
        {
            DefaultFtpConfiguration = defaultFtpConfiguration;
            Logger = logger;
        }

        private FtpConfiguration DefaultFtpConfiguration { get; }
        public ILogger<FTPProvider> Logger { get; }

        public async Task<string> SaveFileAsync(string sourceFilePath, string targetFileName)
        {
            await Task.Yield();
            if (string.IsNullOrEmpty(targetFileName))
            {
                targetFileName = Guid.NewGuid().ToString();
            }
            
            string transcriptFolder = $"{DefaultFtpConfiguration.Uri}/{targetFileName}";

            using (Stream uploadStream = GetStreamForUploadProcess(transcriptFolder))
            using (FileStream fileStream = new FileStream(sourceFilePath, FileMode.Open))
            {
                byte[] buffer = new byte[2048];

                while (true)
                {
                    int bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead != 0)
                    {
                        uploadStream.Write(buffer, 0, bytesRead);
                    }
                    else
                    {
                        break;
                    }
                }
                fileStream.Close();
                uploadStream.Close();
            }

            return targetFileName;
        }

        public async Task<FileStream> GetFileStreamAsync(string path, string destinationFilePath)
        {
            FileshareResponse fileShareResponse = GetStreamForDownloadProcess(path);

            using (FileStream fileStream = new FileStream(destinationFilePath, FileMode.Create))
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                await fileShareResponse.Stream.CopyToAsync(fileStream, BUFFER_SIZE);
                await fileStream.FlushAsync().ConfigureAwait(false);
                fileStream.Close();
                fileShareResponse.Stream.Close();
                fileShareResponse.FtpWebResponse.Close();

                return fileStream;
            }
        }

        public async Task<string> GetFileToLocalTempDirectoryAsync(string ftpFileName, string destinationFileName)
        {
            var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP");
            var filePath = Path.Combine(tempDirectoryPath, destinationFileName);
            FileshareResponse fileShareResponse = GetStreamForDownloadProcess(ftpFileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                await fileShareResponse.Stream.CopyToAsync(fileStream, BUFFER_SIZE).ConfigureAwait(false);
                await fileStream.FlushAsync().ConfigureAwait(false);

                fileStream.Close();
                fileShareResponse.Stream.Close();
                fileShareResponse.FtpWebResponse.Close();
            }

            return filePath;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            try
            {
                await Task.Yield();
                var fullPath = $"{DefaultFtpConfiguration.Uri}/{fileName}";

                FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(fullPath);
                deleteRequest.Credentials = new NetworkCredential(DefaultFtpConfiguration.Username, DefaultFtpConfiguration.Password);
                deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                deleteRequest.KeepAlive = false;
                deleteRequest.UseBinary = true;
                deleteRequest.Proxy = null;
                deleteRequest.UsePassive = DefaultFtpConfiguration.IsSSL;
                deleteRequest.EnableSsl = DefaultFtpConfiguration.IsSSL;

                FtpWebResponse response = (FtpWebResponse)deleteRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.Message, $"Deleting file from { DefaultFtpConfiguration.Uri}", ex);
            }

        }

        private Stream GetStreamForUploadProcess(string destinationFolder)
        {
            destinationFolder.ToLower();
            destinationFolder = destinationFolder.Replace("ftps", "ftp");
            var uploadRequest = (FtpWebRequest)WebRequest.Create(destinationFolder);
            uploadRequest.Timeout = 1800000;
            uploadRequest.ReadWriteTimeout = 1800000;
            uploadRequest.Credentials = new NetworkCredential(DefaultFtpConfiguration.Username, DefaultFtpConfiguration.Password);
            uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
            uploadRequest.KeepAlive = false;
            uploadRequest.UseBinary = true;
            uploadRequest.Proxy = null;
            uploadRequest.UsePassive = DefaultFtpConfiguration.IsSSL;
            uploadRequest.EnableSsl = DefaultFtpConfiguration.IsSSL;

            Stream uploadStream = uploadRequest.GetRequestStream();

            return uploadStream;
        }

        private FileshareResponse GetStreamForDownloadProcess(string fileName)
        {
            try
            {
                var fullPath = $"{DefaultFtpConfiguration.Uri}/{fileName}".ToLower();
                fullPath = fullPath.Replace("ftps", "ftp");

                if (Uri.TryCreate(fullPath, UriKind.Absolute, out Uri filePath))
                {
                    Stream ftpResponseStream;
                    FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(filePath);

                    ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                    ftpWebRequest.Credentials = new NetworkCredential(DefaultFtpConfiguration.Username, DefaultFtpConfiguration.Password);
                    ftpWebRequest.KeepAlive = false;
                    ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpWebRequest.UseBinary = true;
                    ftpWebRequest.Proxy = null;
                    ftpWebRequest.UsePassive = DefaultFtpConfiguration.IsSSL;
                    ftpWebRequest.EnableSsl = DefaultFtpConfiguration.IsSSL;

                    FtpWebResponse ftpWebResponse = ftpWebRequest.GetResponse() as FtpWebResponse;
                    ftpResponseStream = ftpWebResponse.GetResponseStream();

                    var fileShareResponse = new FileshareResponse()
                    {
                        Stream = ftpResponseStream,
                        FtpWebResponse = ftpWebResponse
                    };

                    return fileShareResponse;
                }
                else
                {
                    throw new ArgumentException("The path is not a valid Uri");
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}
