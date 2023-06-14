using System.Net.Http.Headers;

namespace ConsoleClient
{
	internal class Program
	{
		static async void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");

			string iUri = null;
			string iToken = null;
			bool isWritedToken = false;
			while (true)
			{
				try
				{
					if (!isWritedToken)
					{
						Console.Write("Введите токен: ");
						iToken = CRead();
						isWritedToken = true;
					}

					Console.Write("Введите строку запроса или 'con' чтобы поторить операцию с вводом токена: ");
					iToken = CRead();

					if (iToken == "con")
					{
						isWritedToken = false;
						continue;
					}


					using (var httpClient = new HttpClient())
					{
						using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, iUri))
						{
							requestMessage.Headers.Authorization =
								new AuthenticationHeaderValue("Bearer", iToken);

							await httpClient.SendAsync(requestMessage);
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}

			}
		}

		static string CRead()
		{
			string inStr = Console.ReadLine();
			if (inStr == "exit")
				Environment.Exit(0);

			return inStr;
		}
	}
}