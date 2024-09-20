using Microsoft.EntityFrameworkCore;
using TestAppAICodeReview.Data;
using System.Globalization;
using YourWebAPI.Services;


// Check if the flag to review code is passed
if (args.Contains("--review-code"))
{
    // Read the code file(s) for review
    string code = File.ReadAllText("path/to/codefile.cs");

    // Initialize the code reviewer
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    var reviewer = new CodeReviewerService(apiKey);

    // Perform the review
    var review = await reviewer.ReviewCodeAsync(code);

    // Output the review to a file
    //await File.WriteAllTextAsync("review.txt", review);
    
    // Output the review to the console (this can be captured in GitHub Actions)
    Console.WriteLine(review);

    return;
}


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
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

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

