using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.Title = "Cinema Catalog API";
        options.WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();