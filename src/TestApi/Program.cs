using System.Data.SqlClient;
using System.Text.RegularExpressions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!, tags: ["ready"]);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints

app.MapGet("/", () => Results.Ok("Hello, world! (v2)"));

app.MapGet("/sqlserver-version", (IConfiguration configuration, ILogger<Program> logger) =>
{
    string connectionString = configuration.GetConnectionString("DefaultConnection")!;
    string maskedConnectionString = MaskPassword(connectionString);

    using SqlConnection connection = new(connectionString);

    logger.LogInformation("Tentando conectar ao SQL Server. Connection String: {ConnectionString}",
        maskedConnectionString);

    connection.Open();

    logger.LogInformation("Conexão com o SQL Server estabelecida com sucesso.");

    string sql = "SELECT @@VERSION";
    using SqlCommand command = new(sql, connection);
    using SqlDataReader reader = command.ExecuteReader();

    while (reader.Read())
        return Results.Ok(reader.GetString(0));

    return Results.Problem("Não foi possível obter a versão do SQL Server.");
});

app.MapGet("/sqlserver-test", (IConfiguration configuration, ILogger<Program> logger) =>
{
    string connectionString = configuration.GetConnectionString("DefaultConnection")!;
    string maskedConnectionString = MaskPassword(connectionString);

    using SqlConnection connection = new(connectionString);

    logger.LogInformation("Tentando conectar ao SQL Server. Connection String: {ConnectionString}",
        maskedConnectionString);

    connection.Open();

    logger.LogInformation("Conexão com o SQL Server estabelecida com sucesso.");

    string sql = "SELECT 1";
    using SqlCommand command = new(sql, connection);
    using SqlDataReader reader = command.ExecuteReader();

    while (reader.Read())
        return Results.Ok(reader.GetInt32(0));

    return Results.Problem("Não foi possível retornar o valor do SQL Server.");
});

// Health Check Mappings

app.MapHealthChecks("/health");

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/details", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

partial class Program
{
    [GeneratedRegex(@"(?<=Password=)[^;]*")]
    private static partial Regex GetPasswordValueRegex();

    static string MaskPassword(string connectionString)
    {
        return GetPasswordValueRegex().Replace(connectionString, "***");
    }
}