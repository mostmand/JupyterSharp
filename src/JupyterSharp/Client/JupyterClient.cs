using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using JupyterSharp.Abstraction;
using JupyterSharp.Client.Content.Get;
using JupyterSharp.Client.Content.Patch;
using JupyterSharp.Client.Content.Post;
using JupyterSharp.Client.Content.Put;
using JupyterSharp.Dto;

namespace JupyterSharp.Client;

internal sealed class JupyterClient : IJupyterClient
{
    private readonly HttpClient _httpClient;

    public JupyterClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    public Task<VersionInfo> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<VersionInfo>("/api", cancellationToken)!;
    }

    public async Task<GetContentResponse> GetContentAsync(GetContentRequest getContentRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            // Build query parameters
            var queryParams = new List<string>();
            
            if (getContentRequest.Content)
                queryParams.Add("content=1");
            else
                queryParams.Add("content=0");
                
            if (!string.IsNullOrEmpty(getContentRequest.Type))
                queryParams.Add($"type={getContentRequest.Type}");
                
            if (!string.IsNullOrEmpty(getContentRequest.Format))
                queryParams.Add($"format={getContentRequest.Format}");
                
            if (getContentRequest.Hash.HasValue)
                queryParams.Add($"hash={getContentRequest.Hash.Value}");

            // Build the URL with query parameters
            var url = $"/api/contents/{getContentRequest.Path}";
            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<GetContentHttpResponse>(cancellationToken);
                return new GetContentResponse
                {
                    GetContentStatus = GetContentStatus.Ok,
                    Response = content
                };
            }

            return response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => new GetContentResponse
                {
                    GetContentStatus = GetContentStatus.BadRequest
                },
                System.Net.HttpStatusCode.Forbidden => new GetContentResponse
                {
                    GetContentStatus = GetContentStatus.Forbidden
                },
                System.Net.HttpStatusCode.NotFound => new GetContentResponse
                {
                    GetContentStatus = GetContentStatus.NotFound
                },
                System.Net.HttpStatusCode.InternalServerError => new GetContentResponse
                {
                    GetContentStatus = GetContentStatus.InternalServerError
                },
                _ => new GetContentResponse
                {
                    GetContentStatus = GetContentStatus.BadRequest
                }
            };
        }
        catch (HttpRequestException)
        {
            return new GetContentResponse
            {
                GetContentStatus = GetContentStatus.InternalServerError
            };
        }
        catch (TaskCanceledException)
        {
            return new GetContentResponse
            {
                GetContentStatus = GetContentStatus.InternalServerError
            };
        }
    }

    public async Task<PostContentResponse> PostContentAsync(PostContentRequest postContentRequest, CancellationToken cancellationToken)
    {
        try
        {
            // POST creates empty files or copies files - no content should be included
            var requestBody = new Dictionary<string, object?>();
            
            if (!string.IsNullOrEmpty(postContentRequest.Type))
                requestBody["type"] = postContentRequest.Type;
                
            if (!string.IsNullOrEmpty(postContentRequest.CopyFrom))
                requestBody["copy_from"] = postContentRequest.CopyFrom;
                
            if (!string.IsNullOrEmpty(postContentRequest.Ext))
                requestBody["ext"] = postContentRequest.Ext;

            // For POST requests, we need to extract the directory path and filename
            var path = postContentRequest.Path;
            var lastSlashIndex = path.LastIndexOf('/');
            string directoryPath;
            string fileName;
            
            if (lastSlashIndex >= 0)
            {
                directoryPath = path.Substring(0, lastSlashIndex);
                fileName = path.Substring(lastSlashIndex + 1);
            }
            else
            {
                directoryPath = "";
                fileName = path;
            }
            
            // Try different field names for the filename
            if (!string.IsNullOrEmpty(fileName))
            {
                requestBody["name"] = fileName;
                requestBody["path"] = fileName; // Some APIs might expect "path" instead
            }

            // POST to the directory path, not the full file path
            var url = string.IsNullOrEmpty(directoryPath) ? "/api/contents" : $"/api/contents/{directoryPath}";
            var response = await _httpClient.PostAsJsonAsync(url, requestBody, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<PostContentHttpResponse>(cancellationToken);
                return new PostContentResponse
                {
                    PostContentStatus = PostContentStatus.Created,
                    Response = content
                };
            }

            return response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => new PostContentResponse
                {
                    PostContentStatus = PostContentStatus.BadRequest
                },
                System.Net.HttpStatusCode.Forbidden => new PostContentResponse
                {
                    PostContentStatus = PostContentStatus.Forbidden
                },
                System.Net.HttpStatusCode.NotFound => new PostContentResponse
                {
                    PostContentStatus = PostContentStatus.NotFound
                },
                _ => new PostContentResponse
                {
                    PostContentStatus = PostContentStatus.BadRequest
                }
            };
        }
        catch (HttpRequestException)
        {
            return new PostContentResponse
            {
                PostContentStatus = PostContentStatus.BadRequest
            };
        }
        catch (TaskCanceledException)
        {
            return new PostContentResponse
            {
                PostContentStatus = PostContentStatus.BadRequest
            };
        }
    }

    public async Task<PutContentResponse> PutContentAsync(PutContentRequest putContentRequest, CancellationToken cancellationToken)
    {
        try
        {
            // PUT creates/updates files with content
            var requestBody = new Dictionary<string, object?>();
            
            if (!string.IsNullOrEmpty(putContentRequest.Type))
                requestBody["type"] = putContentRequest.Type;
                
            if (!string.IsNullOrEmpty(putContentRequest.Format))
                requestBody["format"] = putContentRequest.Format;
                
            if (putContentRequest.Content != null)
                requestBody["content"] = putContentRequest.Content;

            var response = await _httpClient.PutAsJsonAsync($"/api/contents/{putContentRequest.Path}", requestBody, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<PutContentHttpResponse>(cancellationToken);
                return new PutContentResponse
                {
                    PutContentStatus = PutContentStatus.Ok,
                    Response = content
                };
            }

            return response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => new PutContentResponse
                {
                    PutContentStatus = PutContentStatus.BadRequest
                },
                System.Net.HttpStatusCode.Forbidden => new PutContentResponse
                {
                    PutContentStatus = PutContentStatus.Forbidden
                },
                System.Net.HttpStatusCode.NotFound => new PutContentResponse
                {
                    PutContentStatus = PutContentStatus.NotFound
                },
                _ => new PutContentResponse
                {
                    PutContentStatus = PutContentStatus.BadRequest
                }
            };
        }
        catch (HttpRequestException)
        {
            return new PutContentResponse
            {
                PutContentStatus = PutContentStatus.BadRequest
            };
        }
        catch (TaskCanceledException)
        {
            return new PutContentResponse
            {
                PutContentStatus = PutContentStatus.BadRequest
            };
        }
    }

    public async Task<PatchContentResponse> PatchContentAsync(PatchContentRequest patchContentRequest, CancellationToken cancellationToken)
    {
        try
        {
            // PATCH renames files/directories
            var requestBody = new Dictionary<string, object?>
            {
                ["path"] = patchContentRequest.NewPath
            };

            var response = await _httpClient.PatchAsJsonAsync($"/api/contents/{patchContentRequest.CurrentPath}", requestBody, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<PatchContentHttpResponse>(cancellationToken);
                return new PatchContentResponse
                {
                    PatchContentStatus = PatchContentStatus.Ok,
                    Response = content
                };
            }

            return response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => new PatchContentResponse
                {
                    PatchContentStatus = PatchContentStatus.BadRequest
                },
                System.Net.HttpStatusCode.Forbidden => new PatchContentResponse
                {
                    PatchContentStatus = PatchContentStatus.Forbidden
                },
                System.Net.HttpStatusCode.NotFound => new PatchContentResponse
                {
                    PatchContentStatus = PatchContentStatus.NotFound
                },
                _ => new PatchContentResponse
                {
                    PatchContentStatus = PatchContentStatus.BadRequest
                }
            };
        }
        catch (HttpRequestException)
        {
            return new PatchContentResponse
            {
                PatchContentStatus = PatchContentStatus.BadRequest
            };
        }
        catch (TaskCanceledException)
        {
            return new PatchContentResponse
            {
                PatchContentStatus = PatchContentStatus.BadRequest
            };
        }
    }
}
