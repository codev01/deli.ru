using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace auth.deli.ru
{
	public class Program
	{
		record class Person(string Email, string Password);
		public static void Main(string[] args)
		{

			var builder = WebApplication.CreateBuilder(args);
			IServiceCollection services = builder.Services;

			// выставляем возможные стороки для обращения к api
			builder.WebHost.UseUrls("http://0.0.0.0:1210", "http://localhost:1210");

			services.AddAuthorization();
			// Add services to the container.

			services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			//app.UseMiddleware<JWTMiddleware>();


			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapGet("/data", [Authorize] () => new { message = "Hello World!" });

			app.MapControllers();

			app.Run();
		}
	}
}