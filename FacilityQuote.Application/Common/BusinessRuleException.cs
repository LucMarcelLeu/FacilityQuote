namespace FacilityQuote.Application.Common;

public class BusinessRuleException(string message) : Exception(message)
{
}