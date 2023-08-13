using deli.api.Services.Contracts;

namespace deli.api.Services
{
	public class DBFileService : IFileService
	{
		public DBFileService(/* DBContext */)
		{

		}

		public Stream GetFile(string fileName, string destinationDirectory)
		{
			throw new NotImplementedException();
			//return File.Open(fileName, FileMode.Open);
		}

		public async Task UploadFileAsync(IFormFile file, string destinationDirectory)
		{
			if (file.Length > 0)
			{
				var filePath = Path.GetTempFileName();

				using (var stream = System.IO.File.Create(filePath))
				{
					await file.CopyToAsync(stream);
				}
				// отправить в базу
			}
			else
				throw new Exception("file legth <= 0");
		}
	}
}
