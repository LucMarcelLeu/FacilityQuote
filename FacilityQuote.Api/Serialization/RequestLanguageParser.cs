using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Api.Serialization;

public static class RequestLanguageParser
{
    public static RequestLanguage Parse(string language)
    {
        return language.ToLowerInvariant() switch
        {
            "de" => RequestLanguage.German,
            "en" => RequestLanguage.English,

            _ => throw new ArgumentException(
                $"Unsupported language '{language}'.")
        };
    }
}