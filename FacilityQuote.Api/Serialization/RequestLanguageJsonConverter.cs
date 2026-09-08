using System.Text.Json;
using System.Text.Json.Serialization;
using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Api.Serialization;

public sealed class RequestLanguageJsonConverter
    : JsonConverter<RequestLanguage>
{
    public override RequestLanguage Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "de" => RequestLanguage.German,
            "en" => RequestLanguage.English,
            _ => throw new JsonException(
                "Unsupported request language.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        RequestLanguage value,
        JsonSerializerOptions options)
    {
        var language = value switch
        {
            RequestLanguage.German => "de",
            RequestLanguage.English => "en",
            _ => throw new JsonException(
                "Unsupported request language.")
        };

        writer.WriteStringValue(language);
    }
}