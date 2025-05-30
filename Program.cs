using BlockchainExplorer.Common;
using BlockChainExplorer.SQLServer.Common;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
app.UseCors("AllowAll");
Configure(app, app.Environment);
app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddHttpClient();
    services.AddEndpointsApiExplorer();

    services.AddControllers();
    services.AddSingleton<OkxApiClient>();
    services.AddSingleton<BlockChainExplorer.SQLServer.ConnectionFactory>();

    services.AddDbContext<BlockchainDbContext>(options =>
        options.UseSqlServer(configuration["ConnectionDBStrings:StandardDB"]));

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Blockchain Checker API", Version = "v1" });
    });

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.Limits.MaxRequestBodySize = 2147483648;
    });


    services.AddControllers();

    services.Configure<FormOptions>(x => {
        x.MultipartBodyLengthLimit = 2147483648;
    });
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment() || env.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blockchain Checker API V1"));
    }
    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}