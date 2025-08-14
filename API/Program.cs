
using BookDbApi.DataAccess;
using BookDbApi.Pipeline;
using BookDbApi.Shared;

namespace BookDbApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<SharedError>();
            builder.Services.AddTransient<BookDbApi.Pipeline.ErrorHandlingMiddleware>();

            builder.Services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add<ActionErrorHandlingFilter>();
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();
            
            
            string root = Directory.GetCurrentDirectory();
            string dotEnvPath = Path.Combine(root, ".env");
            DotEnv.Load(dotEnvPath);

            app.Run();
        }
    }
}
