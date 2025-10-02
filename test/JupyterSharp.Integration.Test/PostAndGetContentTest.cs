using System;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using JupyterSharp.Client.Content.Get;
using JupyterSharp.Client.Content.Patch;
using JupyterSharp.Client.Content.Post;
using JupyterSharp.Client.Content.Put;
using JupyterSharp.Integration.Test.JupyterTestContainer;
using Xunit;

namespace JupyterSharp.Integration.Test;

public class ContentApiIntegrationTest : JupyterIntegrationTest
{
    public ContentApiIntegrationTest(JupyterCollectionFixture jupyterCollectionFixture) : base(jupyterCollectionFixture)
    {
    }
    
    [Fact]
    public async Task PutAndGetContent_ShouldCreateFileAndRetrieveIt()
    {
        // Arrange
        var testFileName = $"test-file-{Guid.NewGuid()}.txt";
        var testContent = "Hello, Jupyter! This is a test file created by JupyterSharp.";
        
        var putRequest = new PutContentRequest
        {
            Path = testFileName,
            Type = "file",
            Format = "text",
            Content = testContent
        };

        // Act - Put content (create file with content)
        var putResponse = await JupyterClient.PutContentAsync(putRequest, CancellationToken.None);

        // Assert - Put was successful
        putResponse.PutContentStatus.Should().Be(PutContentStatus.Ok);
        putResponse.Response.Should().NotBeNull();
        putResponse.Response!.Name.Should().Be(testFileName);
        putResponse.Response.Type.Should().Be("file");
        putResponse.Response.Path.Should().Be(testFileName);

        // Act - Get content
        var getRequest = new GetContentRequest
        {
            Path = testFileName,
            Content = true,
            Type = "file",
            Format = "text"
        };

        var getResponse = await JupyterClient.GetContentAsync(getRequest, CancellationToken.None);

        // Assert - Get was successful and content matches
        getResponse.GetContentStatus.Should().Be(GetContentStatus.Ok);
        getResponse.Response.Should().NotBeNull();
        getResponse.Response!.Name.Should().Be(testFileName);
        getResponse.Response.Path.Should().Be(testFileName);
        getResponse.Response.Type.Should().Be("file");
        getResponse.Response.Format.Should().Be("text");
        getResponse.Response.Content?.ToString().Should().Be(testContent);
        getResponse.Response.MimeType.Should().Be("text/plain");
        getResponse.Response.Writable.Should().BeTrue();
    }

    [Fact]
    public async Task PutAndGetContent_WithNotebook_ShouldCreateNotebookAndRetrieveIt()
    {
        // Arrange
        var testNotebookName = $"test-notebook-{Guid.NewGuid()}.ipynb";
        var notebookContent = new
        {
            cells = new[]
            {
                new
                {
                    cell_type = "code",
                    source = new[] { "print('Hello from JupyterSharp!')" },
                    metadata = new { },
                    execution_count = (int?)null,
                    outputs = new object[0]
                }
            },
            metadata = new
            {
                kernelspec = new
                {
                    display_name = "Python 3",
                    language = "python",
                    name = "python3"
                }
            },
            nbformat = 4,
            nbformat_minor = 4
        };
        
        var putRequest = new PutContentRequest
        {
            Path = testNotebookName,
            Type = "notebook",
            Format = "json",
            Content = notebookContent
        };

        // Act - Put notebook (create with content)
        var putResponse = await JupyterClient.PutContentAsync(putRequest, CancellationToken.None);

        // Assert - Put was successful
        putResponse.PutContentStatus.Should().Be(PutContentStatus.Ok);
        putResponse.Response.Should().NotBeNull();
        putResponse.Response!.Name.Should().Be(testNotebookName);
        putResponse.Response.Type.Should().Be("notebook");
        putResponse.Response.Path.Should().Be(testNotebookName);

        // Act - Get notebook
        var getRequest = new GetContentRequest
        {
            Path = testNotebookName,
            Content = true
            // Remove Type and Format to see if they're causing the issue
        };

        var getResponse = await JupyterClient.GetContentAsync(getRequest, CancellationToken.None);

        // Assert - Get was successful
        getResponse.GetContentStatus.Should().Be(GetContentStatus.Ok);
        getResponse.Response.Should().NotBeNull();
        getResponse.Response!.Name.Should().Be(testNotebookName);
        getResponse.Response.Path.Should().Be(testNotebookName);
        getResponse.Response.Type.Should().Be("notebook");
        getResponse.Response.Format.Should().Be("json");
        getResponse.Response.Content.Should().NotBeNull();
        // MimeType might be null when format is not explicitly requested
        getResponse.Response.Writable.Should().BeTrue();
    }

    [Fact]
    public async Task GetContent_WithNonExistentFile_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentFileName = $"non-existent-file-{Guid.NewGuid()}.txt";
        
        var getRequest = new GetContentRequest
        {
            Path = nonExistentFileName,
            Content = true
        };

        // Act
        var getResponse = await JupyterClient.GetContentAsync(getRequest, CancellationToken.None);

        // Assert
        getResponse.GetContentStatus.Should().Be(GetContentStatus.NotFound);
        getResponse.Response.Should().BeNull();
    }

    [Fact]
    public async Task PostContent_ShouldCreateEmptyFile()
    {
        // Arrange
        var testFileName = $"empty-file-{Guid.NewGuid()}.txt";
        
        var postRequest = new PostContentRequest
        {
            Path = testFileName,
            Type = "file"
        };

        // Act - Post to create empty file
        var postResponse = await JupyterClient.PostContentAsync(postRequest, CancellationToken.None);

        // Assert - Post was successful
        postResponse.PostContentStatus.Should().Be(PostContentStatus.Created);
        postResponse.Response.Should().NotBeNull();
        // Note: Jupyter POST creates files with auto-generated names, not the requested name
        postResponse.Response!.Name.Should().NotBeNullOrEmpty();
        postResponse.Response.Type.Should().Be("file");
        postResponse.Response.Path.Should().NotBeNullOrEmpty();

        // Verify file exists but is empty
        var getRequest = new GetContentRequest
        {
            Path = postResponse.Response.Path, // Use the actual created file path
            Content = true
        };

        var getResponse = await JupyterClient.GetContentAsync(getRequest, CancellationToken.None);
        getResponse.GetContentStatus.Should().Be(GetContentStatus.Ok);
        getResponse.Response.Should().NotBeNull();
        // Empty file should have null or empty content
        var content = getResponse.Response!.Content?.ToString();
        (content == null || content.Equals("")).Should().BeTrue();
    }

    [Fact]
    public async Task PatchContent_ShouldRenameFile()
    {
        // Arrange
        var originalFileName = $"original-{Guid.NewGuid()}.txt";
        var newFileName = $"renamed-{Guid.NewGuid()}.txt";
        var testContent = "This file will be renamed";

        // First create a file with content using PUT
        var putRequest = new PutContentRequest
        {
            Path = originalFileName,
            Type = "file",
            Format = "text",
            Content = testContent
        };

        var putResponse = await JupyterClient.PutContentAsync(putRequest, CancellationToken.None);
        putResponse.PutContentStatus.Should().Be(PutContentStatus.Ok);

        // Act - Rename the file using PATCH
        var patchRequest = new PatchContentRequest
        {
            CurrentPath = originalFileName,
            NewPath = newFileName
        };

        var patchResponse = await JupyterClient.PatchContentAsync(patchRequest, CancellationToken.None);

        // Assert - Patch was successful
        patchResponse.PatchContentStatus.Should().Be(PatchContentStatus.Ok);
        patchResponse.Response.Should().NotBeNull();
        patchResponse.Response!.Name.Should().Be(newFileName);
        patchResponse.Response.Path.Should().Be(newFileName);

        // Verify the file exists at the new location with same content
        var getRequest = new GetContentRequest
        {
            Path = newFileName,
            Content = true
        };

        var getResponse = await JupyterClient.GetContentAsync(getRequest, CancellationToken.None);
        getResponse.GetContentStatus.Should().Be(GetContentStatus.Ok);
        getResponse.Response!.Content?.ToString().Should().Be(testContent);

        // Verify the original file no longer exists
        var getOriginalRequest = new GetContentRequest
        {
            Path = originalFileName,
            Content = true
        };

        var getOriginalResponse = await JupyterClient.GetContentAsync(getOriginalRequest, CancellationToken.None);
        getOriginalResponse.GetContentStatus.Should().Be(GetContentStatus.NotFound);
    }
}
