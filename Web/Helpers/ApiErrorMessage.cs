using System.Text.Json;

namespace TesteTecnico.Web.Helpers;

public static class ApiErrorMessage
{
    public static async Task<string> FromResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
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

        return content;
    }
}
