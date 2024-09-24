using Microsoft.EntityFrameworkCore;
using TestAppAICodeReview.Data;
using System.Globalization;
using YourWebAPI.Services;
using System.Text;
using Microsoft.CodeAnalysis.MSBuild;  // For MSBuildWorkspace
using Microsoft.Build.Locator;         // For MSBuildLocator
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


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
    var allFilesForReview = new HashSet<string>(changedFiles); // Using a HashSet to avoid duplicates

    // Analyze the solution using Roslyn to get all dependent files
    var dependentFiles = await GetDependentFilesAsync(changedFiles);

    // Add dependent files to the review set
    foreach (var dependentFile in dependentFiles)
    {
        allFilesForReview.Add(dependentFile);
    }

    // Prepare the code for review
    StringBuilder allCode = new StringBuilder();

    foreach (var file in allFilesForReview)
    {
        if (file.EndsWith(".cs") && File.Exists(file))
        {
            string code = File.ReadAllText(file);
            allCode.AppendLine(code);
        }
    }

    // Now you have all the code from the changed and dependent .cs files in a single string
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

async Task<HashSet<string>> GetDependentFilesAsync(IEnumerable<string> changedFiles)
{
    var dependentFiles = new HashSet<string>();
    using (var workspace = MSBuildWorkspace.Create())
    {
        // Adjust to the path of your .sln or .csproj file
        var solutionPath = "./TestAppAICodeReview.sln"; 
        var solution = await workspace.OpenSolutionAsync(solutionPath);

        foreach (var project in solution.Projects)
        {
            foreach (var document in project.Documents)
            {
                var syntaxTree = await document.GetSyntaxTreeAsync();
                if (syntaxTree == null)
                    continue;

                var root = await syntaxTree.GetRootAsync();
                foreach (var changedFile in changedFiles)
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(changedFile);
                    
                    // Check if the current document references any of the changed files
                    var hasReference = root.DescendantNodes().OfType<UsingDirectiveSyntax>()
                        .Any(u => u.Name.ToString().Contains(fileNameWithoutExtension));

                    if (hasReference)
                    {
                        dependentFiles.Add(document.FilePath);
                    }
                }
            }
        }
    }

    return dependentFiles;
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

