using System.Text.Json;

namespace TesteTecnico.Web.Helpers;

public static class ApiErrorMessage
{
    public static string FromExceptionMessage(string message)
    {
        var details = message.Split("Detalhes:", 2, StringSplitOptions.TrimEntries)
            .Last();

        return FromContent(details, message);
    }

    public static async Task<string> FromResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return FromContent(content);
    }

    private static string FromContent(string content, string? fallback = null)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return "Ocorreu um erro ao processar a solicitação.";
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                var messages = root.EnumerateArray()
                    .Where(item => item.ValueKind == JsonValueKind.String)
                    .Select(item => item.GetString())
                    .Where(message => !string.IsNullOrWhiteSpace(message))
                    .ToArray();

                if (messages.Length > 0)
                {
                    return string.Join(Environment.NewLine, messages);
                }
            }

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("errors", out var errors) &&
                    errors.ValueKind == JsonValueKind.Object)
                {
                    var messages = errors.EnumerateObject()
                        .SelectMany(error =>
                        {
                            if (error.Value.ValueKind == JsonValueKind.Array)
                            {
                                return error.Value.EnumerateArray();
                            }

                            return [error.Value];
                        })
                        .Where(message => message.ValueKind == JsonValueKind.String)
                        .Select(message => message.GetString())
                        .Where(message => !string.IsNullOrWhiteSpace(message))
                        .ToArray();

                    if (messages.Length > 0)
                    {
                        return string.Join(Environment.NewLine, messages);
                    }
                }

                foreach (var propertyName in new[] { "title", "detail" })
                {
                    if (root.TryGetProperty(propertyName, out var property) &&
                        property.ValueKind == JsonValueKind.String &&
                        !string.IsNullOrWhiteSpace(property.GetString()))
                    {
                        return property.GetString()!;
                    }
                }
            }
        }
        catch (JsonException)
        {
            // Keep the original response when it is not valid JSON.
        }

        return fallback ?? content;
    }
}
