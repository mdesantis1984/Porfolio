using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Porfolio.Code.Entidades;
using Porfolio.Code.Interface;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Porfolio.Code.Service;

public class LinkedinService
{
    public string profile;
    public string _authorizationCode;
    public string _token;
    private readonly OAuth2Services _oauth2Services;

    ILocalStorageService StorageService;
    HttpClient _httpClient;
    NavigationManager uri;

    public LinkedinService(ILocalStorageService localStorageService, HttpClient httpClient, NavigationManager navigationManager, IOptions<OAuth2Services> oauth2Options)
    {
        _httpClient = httpClient;
        StorageService = localStorageService;
        uri = navigationManager;
        _oauth2Services = oauth2Options.Value;
    }

    public async Task SaveProfile(string authorizationCode)
    {
        _token = await GetAccessToken(authorizationCode);
        var JsonBase64 = await GetUserData(_token);

        if (JsonBase64 != null)
        {
            await StorageService.SetItemAsync("profile", JsonBase64);
        }
    }

    public async Task<User> GetProfile()
    {
        User profile = new User();

        try
        {
            var strProfile = await StorageService.GetItemAsync("profile");
            var decoder64 = strProfile.Base64Decode();

            JsonSerializerOptions options = new JsonSerializerOptions().SetJsonSerializerOptions();
            profile = JsonSerializer.Deserialize<User>(decoder64, options);

        }
        catch (Exception)
        {
            throw;
        }

        return profile;
    }

    public async Task<string> GetUserData(string accessToken)
    {
        var userDataEndpoint = "https://api.linkedin.com/v2/userinfo";

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync(userDataEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                return responseContent.Base64Encode();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Error deserializando JSON: {jsonEx.Message}");
                Console.WriteLine($"Path: {jsonEx.Path}");
                Console.WriteLine($"LineNumber: {jsonEx.LineNumber}");
                Console.WriteLine($"BytePositionInLine: {jsonEx.BytePositionInLine}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                throw;
            }
        }
        else
        {
            throw new Exception($"Failed to fetch user data: {response.ReasonPhrase}");
        }
    }
    public async Task<string> GetAccessToken(string code)
    {
        var tokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken";

        var parameters = new Dictionary<string, string>
        {
            { "grant_type", _oauth2Services.OAuth2.grant_type },
            { "code", code },
            { "client_id", _oauth2Services.OAuth2.client_id },
            { "client_secret", _oauth2Services.OAuth2.client_secret },
            { "redirect_uri", _oauth2Services.OAuth2.redirect_uri }
        };

        var response = await _httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(parameters));

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            return tokenResponse.access_token;
        }
        else
        {
            throw new Exception($"Failed to obtain access token: {response.ReasonPhrase}");
        }
    }
}

public class TokenResponse
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string scope { get; set; }
    public string token_type { get; set; }
    public string id_token { get; set; }
}

