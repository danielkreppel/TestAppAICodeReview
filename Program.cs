using Microsoft.EntityFrameworkCore;
using TestAppAICodeReview.Data;
using System.Globalization;
using YourWebAPI.Services;
using System.Text;


// Check if the flag to review code is passed
if (args.Contains("--review-code"))
{
    // Check if file list argument is provided
    var fileListArgIndex = Array.IndexOf(args, "--file-list") + 1;
    if (fileListArgIndex <= 0 || fileListArgIndex >= args.Length)
    {
        Console.WriteLine("File list argument is missing.");
        return;
    }

    // Read the list of modified files from the file provided
    var fileListPath = args[fileListArgIndex];
    if (!File.Exists(fileListPath))
    {
        Console.WriteLine($"File list path '{fileListPath}' does not exist.");
        return;
    }

    var changedFiles = File.ReadAllLines(fileListPath);
    StringBuilder allCode = new StringBuilder();

    foreach (var file in changedFiles)
    {
        // Make sure to only process .cs files
        if (file.EndsWith(".cs") && File.Exists(file))
        {
            string code = File.ReadAllText(file);
            allCode.AppendLine(code);
        }
    }

    // Now you have all the code from the changed .cs files in a single string
    string codeForReview = allCode.ToString();

    // Initialize the code reviewer
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    var reviewer = new CodeReviewerService(apiKey);

    // Perform the review
    var review = await reviewer.ReviewCodeAsync(codeForReview);

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

