using Application.Abstracts;

namespace Application.Services;

public class SpamService : ISpamService
{
    public async Task<bool> IsSpamAsync(string content)
    {
        var client = new HttpClient();
        var response =
            await client.GetAsync($"http://localhost:8080/spam-check?content={Uri.EscapeDataString(content)}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        return bool.Parse(result);
    }
}