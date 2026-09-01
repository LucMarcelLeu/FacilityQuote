using FacilityQuote.Application.Common;
using FacilityQuote.Application.Quotes.Dtos;
using FacilityQuote.Application.Requests;
using FacilityQuote.Application.Services;
using FacilityQuote.Domain.Quotes;

namespace FacilityQuote.Application.Quotes;

public class QuoteService(
    IQuoteRepository quoteRepository,
    IRequestRepository requestRepository,
    IServiceRepository serviceRepository)
{
    public async Task<Quote> CreateAsync(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = await requestRepository.GetByIdAsync(
            requestId,
            cancellationToken);

        if (request is null)
        {
            throw new ResourceNotFoundException(
                $"Request '{requestId}' was not found.");
        }

        if (!request.Quantity.HasValue || request.Quantity <= 0)
        {
            throw new BusinessRuleException(
                "A valid quantity is required to create a quote.");
        }

        var existingQuote = await quoteRepository.GetByRequestIdAsync(
            requestId,
            cancellationToken);

        if (existingQuote is not null)
        {
            throw new BusinessRuleException(
                $"A quote already exists for request '{requestId}'.");
        }

        var service = await serviceRepository.GetByIdAsync(
            request.ServiceId,
            cancellationToken);

        if (service is null)
        {
            throw new ResourceNotFoundException(
                $"Service '{request.ServiceId}' was not found.");
        }

        var createdAt = DateTime.UtcNow;
        var validUntil = DateOnly.FromDateTime(createdAt).AddDays(14);

        var quoteNumber = await quoteRepository.GetNextQuoteNumberAsync(
            cancellationToken);

        var quote = new Quote
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            QuoteNumber = $"OFF-{createdAt:yyyy}-{quoteNumber:D4}",
            Status = QuoteStatus.Draft,
            CreatedAt = createdAt,
            ValidUntil = validUntil
        };

        quote.Items.Add(new QuoteItem
        {
            Id = Guid.NewGuid(),
            QuoteId = quote.Id,
            Description = service.Name,
            Quantity = request.Quantity.Value,
            Unit = service.Unit,
            UnitPrice = service.UnitPrice
        });

        request.MarkQuotationCreated();

        await quoteRepository.AddAsync(
            quote,
            cancellationToken);

        await quoteRepository.SaveChangesAsync(
            cancellationToken);

        return quote;
    }

    public Task<Quote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return quoteRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    public Task<Quote?> GetByRequestIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return quoteRepository.GetByRequestIdAsync(
            id,
            cancellationToken);
    }

    public async Task<Quote> UpdateAsync(
        Guid quoteId,
        UpdateQuoteRequest request,
        CancellationToken cancellationToken = default)
    {
        var quote = await quoteRepository.GetByIdAsync(
            quoteId,
            cancellationToken);

        if (quote is null)
        {
            throw new ResourceNotFoundException(
                $"Quote '{quoteId}' was not found.");
        }

        if (quote.Status != QuoteStatus.Draft)
        {
            throw new BusinessRuleException(
                $"Quote '{quote.QuoteNumber}' cannot be modified because its status is '{quote.Status}'.");
        }

        quote.ValidUntil = request.ValidUntil;
        quote.Notes = request.Notes;
        quote.TravelCost = request.TravelCost;

        await quoteRepository.SaveChangesAsync(
            cancellationToken);

        return quote;
    }

    public async Task<QuoteItem> UpdateItemAsync(
        Guid quoteId,
        Guid itemId,
        UpdateQuoteItemRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateItem(
            request.Description,
            request.Quantity,
            request.Unit,
            request.UnitPrice);

        var quote = await quoteRepository.GetByIdAsync(
            quoteId,
            cancellationToken);

        if (quote is null)
        {
            throw new ResourceNotFoundException(
                $"Quote '{quoteId}' was not found.");
        }

        if (quote.Status != QuoteStatus.Draft)
        {
            throw new BusinessRuleException(
                $"Quote '{quote.QuoteNumber}' cannot be modified because its status is '{quote.Status}'.");
        }

        var item = quote.Items.FirstOrDefault(
            i => i.Id == itemId);

        if (item is null)
        {
            throw new ResourceNotFoundException(
                $"Quote item '{itemId}' was not found.");
        }

        item.Description = request.Description;
        item.Quantity = request.Quantity;
        item.Unit = request.Unit;
        item.UnitPrice = request.UnitPrice;

        await quoteRepository.SaveChangesAsync(
            cancellationToken);

        return item;
    }

    public async Task DeleteItemAsync(
        Guid quoteId,
        Guid itemId,
        CancellationToken cancellationToken = default)
    {
        var quote = await quoteRepository.GetByIdAsync(
            quoteId,
            cancellationToken);

        if (quote is null)
        {
            throw new ResourceNotFoundException(
                $"Quote '{quoteId}' was not found.");
        }

        if (quote.Status != QuoteStatus.Draft)
        {
            throw new BusinessRuleException(
                $"Quote '{quote.QuoteNumber}' cannot be modified because its status is '{quote.Status}'.");
        }

        var item = quote.Items.FirstOrDefault(
            i => i.Id == itemId);

        if (item is null)
        {
            throw new ResourceNotFoundException(
                $"Quote item '{itemId}' was not found.");
        }

        quoteRepository.RemoveItem(item);

        await quoteRepository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<QuoteItem> AddItemAsync(
        Guid quoteId,
        string description,
        decimal quantity,
        string unit,
        decimal unitPrice,
        CancellationToken cancellationToken = default)
    {
        ValidateItem(
            description,
            quantity,
            unit,
            unitPrice);

        var quote = await quoteRepository.GetByIdAsync(
            quoteId,
            cancellationToken);

        if (quote is null)
        {
            throw new ResourceNotFoundException($"Quote '{quoteId}' was not found.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(unitPrice),
                "Unit price cannot be negative.");
        }

        var item = new QuoteItem
        {
            Id = Guid.NewGuid(),
            QuoteId = quoteId,
            Description = description,
            Quantity = quantity,
            Unit = unit,
            UnitPrice = unitPrice
        };

        await quoteRepository.AddItemAsync(
            item,
            cancellationToken);

        await quoteRepository.SaveChangesAsync(
            cancellationToken);

        return item;
    }

    public Task<IReadOnlyList<Quote>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return quoteRepository.GetAllAsync(
            cancellationToken);
    }

    public async Task<Quote> UpdateTravelCostAsync(
        Guid quoteId,
        decimal travelCost,
        CancellationToken cancellationToken = default)
    {
        var quote = await quoteRepository.GetByIdAsync(
            quoteId,
            cancellationToken);

        if (quote is null)
        {
            throw new InvalidOperationException(
                $"Quote '{quoteId}' was not found.");
        }

        if (travelCost < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(travelCost),
                "Travel cost cannot be negative.");
        }

        quote.TravelCost = travelCost;

        await quoteRepository.SaveChangesAsync(
            cancellationToken);

        return quote;
    }

    public async Task<Quote> UpdateStatusAsync(
        Guid quoteId,
        QuoteStatus newStatus,
        CancellationToken cancellationToken = default)
    {
        var quote = await quoteRepository.GetByIdAsync(
            quoteId,
            cancellationToken);

        if (quote is null)
        {
            throw new InvalidOperationException(
                $"Quote '{quoteId}' was not found.");
        }

        if (!IsValidTransition(quote.Status, newStatus))
        {
            throw new BusinessRuleException($"Invalid quote status transition from '{quote.Status}' to '{newStatus}'.");
        }

        if (quote.Status == QuoteStatus.Draft && newStatus == QuoteStatus.Sent)
        {
            if (!quote.ValidUntil.HasValue)
            {
                throw new BusinessRuleException(
                    $"Quote '{quote.QuoteNumber}' cannot be sent without a validity date.");
            }

            if (quote.Items.Count == 0)
            {
                throw new BusinessRuleException(
                    $"Quote '{quote.QuoteNumber}' cannot be sent without any items.");
            }
        }

        quote.Status = newStatus;

        await quoteRepository.SaveChangesAsync(cancellationToken);

        return quote;
    }

    private static bool IsValidTransition(
        QuoteStatus currentStatus,
        QuoteStatus newStatus)
    {
        return currentStatus switch
        {
            QuoteStatus.Draft =>
                newStatus == QuoteStatus.Sent,

            QuoteStatus.Sent =>
                newStatus is QuoteStatus.Accepted
                    or QuoteStatus.Rejected,

            QuoteStatus.Accepted => false,

            QuoteStatus.Rejected => false,

            _ => false
        };
    }
    private static void ValidateItem(
        string description,
        decimal quantity,
        string unit,
        decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new BusinessRuleException(
                "Item description is required.");

        if (quantity <= 0)
            throw new BusinessRuleException(
                "Item quantity must be greater than zero.");

        if (string.IsNullOrWhiteSpace(unit))
            throw new BusinessRuleException(
                "Item unit is required.");

        if (unitPrice < 0)
            throw new BusinessRuleException(
                "Item unit price cannot be negative.");
    }

    public async Task<Quote> SendAsync(
        Guid quoteId,
        CancellationToken cancellationToken = default)
    {
        var quote = await quoteRepository.GetByIdAsync(
            quoteId,
            cancellationToken);

        if (quote is null)
        {
            throw new ResourceNotFoundException(
                $"Quote '{quoteId}' was not found.");
        }

        sendQuote(quote);

        await quoteRepository.SaveChangesAsync(
            cancellationToken);

        return quote;
    }

    private void sendQuote(Quote quote)
    {
        if (quote.Status != QuoteStatus.Draft)
        {
            throw new BusinessRuleException(
                $"Quote '{quote.QuoteNumber}' can only be sent when its status is 'Draft'.");
        }

        if (!quote.Items.Any())
        {
            throw new BusinessRuleException(
                "A quote must contain at least one item.");
        }

        if (quote.Items.Any(item => item.Quantity <= 0))
        {
            throw new BusinessRuleException(
                "All quote items must have a quantity greater than zero.");
        }

        if (quote.Items.Any(item => string.IsNullOrWhiteSpace(item.Unit)))
        {
            throw new BusinessRuleException(
                "All quote items must have a unit.");
        }

        if (quote.Items.Any(item => item.UnitPrice < 0))
        {
            throw new BusinessRuleException(
                "Quote item prices cannot be negative.");
        }

        if (quote.ValidUntil is null)
        {
            throw new BusinessRuleException(
                "A quote must have a validity date.");
        }

        quote.Status = QuoteStatus.Sent;
    }


}