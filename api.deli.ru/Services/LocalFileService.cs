using api.deli.ru.ConfigModels;
using api.deli.ru.Services.Contracts;

namespace api.deli.ru.Services
{
	public class LocalFileService : IFileService
	{
		private readonly ApiConfigurations _config;
		public LocalFileService(ApiConfigurations apiConfigurations)
		{
			_config = apiConfigurations;
		}

		public Stream GetFile(string fileName, string destinationDirectory)
		{
			return File.Open(fileName, FileMode.Open);
		}

		public async Task UploadFileAsync(IFormFile file, string destinationDirectory)
		{
			try
			{
				if (string.IsNullOrEmpty(_config.LocalFilesPath))
					throw new Exception("LocalFilesPath -> Null Or Empty");

				string path = Utils.CreatePathStr(Directory.GetCurrentDirectory()/*AppDomain.CurrentDomain.BaseDirectory*/, _config.LocalFilesPath, destinationDirectory);

				if (file.Length > 0 && file.Length > file.Length / Math.Pow(1024, 2))
				{
					var filePath = Path.Combine(path, file.FileName /*Path.GetRandomFileName()*/);
					var qwe = Path.GetDirectoryName(filePath);
					var w = Directory.GetCurrentDirectory();
					Directory.CreateDirectory(qwe);

					bool we = Directory.Exists(qwe);

					using (FileStream? stream = System.IO.File.Create(filePath))
					{
						file.CopyTo(stream);

						stream.Close();
					}
				}
				else
					throw new Exception("file.legth <= 0 || file.legth < maxLength");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
