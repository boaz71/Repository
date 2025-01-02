using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowLocalhost")]

public class RepositoryController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public RepositoryController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }




    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> SearchRepositories([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest("Keyword is required.");
        }

        var githubApiUrl = $"https://api.github.com/search/repositories?q={keyword}";
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubSearchApp");

        try
        {
            var response = await _httpClient.GetAsync(githubApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from GitHub API.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var repositories = JsonSerializer.Deserialize<object>(content);
            return Ok(repositories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Server error: {ex.Message}");
        }
    }

    [HttpGet("GetSecureData")]
    [Authorize(Roles = "admin")]
    public IActionResult GetSecureData()
    {
        return Ok(new { Message = "This is a secure endpoint!" });
    }

    [HttpGet("GetMessage")]
    public IActionResult GetString()
    {
        return Ok(new { Message = "Hello world!" });
    }


}
