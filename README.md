# AI Code Reviewer

This project demonstrates how an AI-driven code reviewer works using OpenAI's API to review C# code. It is integrated with GitHub Actions, so whenever a pull request is opened or updated, the AI automatically reviews the changed code and provides feedback directly in the pull request comments.

## Features

- **Automated Code Reviews**: The app uses OpenAI's API to review C# code for potential issues.
- **GitHub Actions Integration**: The code review is triggered automatically when a pull request is made, allowing continuous feedback.
- **Support for Contextual Reviews**: The AI takes into account changes in individual files and dependent files.
  
## How It Works

1. **Triggering the Review**: When a pull request is opened or updated in GitHub, the AI code reviewer is triggered.
2. **Analyzing the Code**: The AI checks for issues in the modified files, taking into account dependencies,
3. **Posting Feedback**: The review is posted as a comment in the pull request, highlighting any potential issues and suggesting improvements.

## Setup and Installation

To set up this app locally or within your GitHub repository, follow the steps below:

### Prerequisites

- **.NET SDK**: Version 7.x or higher is required. You can download it from [here](https://dotnet.microsoft.com/download).
- **OpenAI API Key**: You must have an API key from OpenAI to perform the code reviews. You can sign up and get the key [here](https://platform.openai.com/signup).

### Installing the App

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/AI-Code-Reviewer.git
   cd AI-Code-Reviewer

2. **Set up Environment Variables. Make sure you have your OpenAI API key set in your environment or in Github repository (Go to your GitHub repository settings and add the following secret: OPENAI_API_KEY: "Your OpenAI API key")
   ```bash
   export OPENAI_API_KEY=your-openai-api-key
   
3. **Build the Project: Restore dependencies and build the project:
   ```bash
   dotnet restore
   dotnet build
   
4. **Push changes and create a pull request
   
6. **To run locally, add code files with changes to "modified_files.txt" and run:
   ```
   dotnet run --project ./TestAppAICodeReview.csproj --review-code --file-list modified_files.txt

7. **GitHub Actions Integration: This repository includes a GitHub Actions workflow that automatically triggers the AI code review on every pull request. Please check .github/workflows folder


![AI Code Review](https://github.com/user-attachments/assets/38a783c4-2413-44f6-86e5-3244370be041)


## Example

1. **Making a change that does not causes build errors, however, is not coerent with the rest of the app context
![image](https://github.com/user-attachments/assets/f6c15ed4-b7b8-4b7a-9f3d-c7ec9b68c8ae)

2. **Create a new pull request:
<img width="500" alt="image" src="https://github.com/user-attachments/assets/4b4ef8fa-3e37-4896-9ad1-5388af57a5d8">

3. **Wait for the github action to run, call OpenAI with the changed files (including other dependent code files related) and prompt to review it:
![image](https://github.com/user-attachments/assets/6cdc6b65-5bbb-4ce6-bad9-8c70b0113b1e)

4. **AI suggestions are posted as a comment in the pull request, catching the issue realted to the latest change and many other suggestions:
![image](https://github.com/user-attachments/assets/2d24a03e-4913-44ef-83f8-8744490fe025)

   
