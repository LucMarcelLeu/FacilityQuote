namespace FacilityQuote.Application.Common;

public class ResourceNotFoundException(string message) : Exception(message)
{
}