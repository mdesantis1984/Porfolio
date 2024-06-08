using Microsoft.JSInterop;
using Porfolio.Code.Interface;

using System.Text.Json;
using System.Threading.Tasks;
using static MudBlazor.Colors;

namespace Porfolio.Code.Implementacion;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;


    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetItemAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        try
        {
            return json;
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Error deserializando JSON: {jsonEx.Message}");
            Console.WriteLine($"Path: {jsonEx.Path}");
            Console.WriteLine($"LineNumber: {jsonEx.LineNumber}");
            Console.WriteLine($"BytePositionInLine: {jsonEx.BytePositionInLine}");
            Console.WriteLine(json); // Imprimir el JSON completo para depuración
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
            throw;
        }
    }


    public async Task SetItemAsync(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }
}