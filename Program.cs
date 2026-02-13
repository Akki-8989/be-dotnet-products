using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.UseCors();

var connectionString = app.Configuration.GetConnectionString("DefaultConnection") ?? "";

app.MapGet("/", () => Results.Ok(new
{
    service = "be-dotnet-products",
    status = "running",
    databaseConnected = !string.IsNullOrEmpty(connectionString)
}));

app.MapGet("/api/products", async () =>
{
    if (string.IsNullOrEmpty(connectionString))
        return Results.Ok(new { source = "no-database", data = Array.Empty<object>() });

    try
    {
        var products = new List<object>();
        using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("SELECT Id, Name, Price, Category, CreatedAt FROM Products", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(new
            {
                id = reader.GetInt32(0),
                name = reader.GetString(1),
                price = reader.GetDecimal(2),
                category = reader.IsDBNull(3) ? "" : reader.GetString(3),
                createdAt = reader.GetDateTime(4)
            });
        }
        return Results.Ok(new { source = "database", data = products });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { source = "error", error = ex.Message });
    }
});

app.MapGet("/api/categories", async () =>
{
    if (string.IsNullOrEmpty(connectionString))
        return Results.Ok(new { source = "no-database", data = Array.Empty<object>() });

    try
    {
        var categories = new List<object>();
        using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("SELECT Id, Name FROM Categories", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            categories.Add(new { id = reader.GetInt32(0), name = reader.GetString(1) });
        }
        return Results.Ok(new { source = "database", data = categories });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { source = "error", error = ex.Message });
    }
});

app.Run();
