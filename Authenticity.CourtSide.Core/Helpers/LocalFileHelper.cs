using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Helpers
{
	public class LocalFileHelper
	{
		public async Task<string> SaveFileAsync(string fileName, IFormFile file)
		{
			var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP");
			var filePath = Path.Combine(tempDirectoryPath, fileName);

			using (var stream = File.Create(filePath))
			{
				await file.CopyToAsync(stream);
			}

			return filePath;
		}

		public async Task<bool> DeleteFileAsync(string filePath)
		{
			await Task.Yield();

			if (File.Exists(filePath) && !IsFileLocked(filePath))
			{
				File.Delete(filePath);
				return true;
			}

			return false;
		}

		private bool IsFileLocked(string filePath)
		{
			try
			{
				using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					stream.Close();
				}
			}
			catch (IOException)
			{
				return true;
			}
			return false;
		}

		public void CleanTempFolder(string folderName, int previousHours)
		{
			DateTime hours = DateTime.Now.AddHours(-previousHours);

			if (Directory.Exists(folderName))
			{
				string[] files = Directory.GetFiles(folderName);
				foreach (string fileName in files)
				{
					DateTime dt = File.GetCreationTime(fileName);

					if (dt <= hours && File.Exists(fileName))
					{
						File.Delete(fileName);
					}
				}
			}
		}

		public async Task<string> SaveUnmanagedMemoryStreamAsync(UnmanagedMemoryStream unmanagedMemoryStream, string fileName)
		{
			var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP");
			var filePath = Path.Combine(tempDirectoryPath, fileName);

			MemoryStream memoryStream = new MemoryStream();
			await unmanagedMemoryStream.CopyToAsync(memoryStream);
			using (FileStream file = File.Open(filePath, FileMode.OpenOrCreate))
			{
				memoryStream.Position = 0;
				await memoryStream.CopyToAsync(file);
				file.Close();
				return filePath;
			}
		}

		public async Task<string> SaveStreamAsync(Stream memoryStream, string fileName)
		{
			var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP");
			var filePath = Path.Combine(tempDirectoryPath, fileName);

			using (FileStream file = File.Open(filePath, FileMode.OpenOrCreate))
			{
				memoryStream.Position = 0;
				await memoryStream.CopyToAsync(file);
				file.Close();
				return filePath;
			}
		}

		public async Task<string> SaveFileStreamAsync(FileStream fileStream, string fileName)
		{
			var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP");
			var filePath = Path.Combine(tempDirectoryPath, fileName);

			using (FileStream file = File.Open(filePath, FileMode.OpenOrCreate))
			{
				fileStream.Position = 0;
				await fileStream.CopyToAsync(file);
				file.Close();
				return filePath;
			}
		}
	}
}
