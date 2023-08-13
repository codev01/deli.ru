namespace deli.api.Services.Contracts
{
	public interface IFileService
	{
		/// <param name="destinationDirectory"> Конечная папка. К примеру id пользователя </param>
		Stream GetFile(string fileName, string destinationDirectory);
		Task UploadFileAsync(IFormFile file, string destinationDirectory);
		public void UploadFiles(IFormFileCollection files, string destinationDirectory)
		{
			foreach (IFormFile file in files)
			{
				UploadFileAsync(file, destinationDirectory);
			}
		}
	}
}
