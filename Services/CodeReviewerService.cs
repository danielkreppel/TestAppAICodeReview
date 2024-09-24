using OpenAI_API;

namespace YourWebAPI.Services
{
    public class CodeReviewerService
    {
        private readonly OpenAIAPI _openAIAPI;

        public CodeReviewerService(string apiKey)
        {
            _openAIAPI = new OpenAIAPI(apiKey);
        }

        public async Task<string> ReviewCodeAsync(string code)
        {
            string prompt = $"Review the following C# code:\n\n{code}\n\n Provide feedback about possible issues.";

            Console.WriteLine(prompt); // This will print the code that is being sent
            
            var completion = await _openAIAPI.Completions.CreateCompletionAsync(
                prompt, 
                max_tokens: 500, 
                temperature: 0.7 
            );

            return completion.Completions.First().Text;
        }
    }
}
