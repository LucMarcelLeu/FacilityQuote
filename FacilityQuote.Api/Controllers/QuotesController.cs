using FacilityQuote.Api.Models.Quotes;
using FacilityQuote.Application.Quotes;
using FacilityQuote.Application.Quotes.Dtos;
using FacilityQuote.Domain.Quotes;
using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotesController(
    QuoteService quoteService,
    IQuotePdfService quotePdfService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateQuoteRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var quote = await quoteService.CreateAsync(
                request.RequestId,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = quote.Id },
                new
                {
                    quote.Id,
                    quote.QuoteNumber,
                    quote.Status,
                    quote.CreatedAt
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var quotes = await quoteService.GetAllAsync(
            cancellationToken);

        return Ok(quotes.Select(q => new
        {
            q.Id,
            q.RequestId,
            q.QuoteNumber,
            q.Status,
            q.CreatedAt,
            q.ValidUntil,
            q.TravelCost,
            q.Subtotal,
            q.Total
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var quote = await quoteService.GetByIdAsync(
            id,
            cancellationToken);

        if (quote is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            quote.Id,
            quote.RequestId,
            quote.QuoteNumber,
            quote.Status,
            quote.CreatedAt,
            quote.ValidUntil,
            quote.Notes,
            quote.TravelCost,
            quote.Subtotal,
            quote.Total,
            Items = quote.Items.Select(item => new
            {
                item.Id,
                item.Description,
                item.Quantity,
                item.Unit,
                item.UnitPrice,
                item.Total
            })
        });
    }

    [HttpGet("by-request/{requestId:guid}")]
    public async Task<IActionResult> GetByRequestId(
        Guid requestId,
        CancellationToken cancellationToken)
    {
        var quote = await quoteService.GetByRequestIdAsync(
            requestId,
            cancellationToken);

        if (quote is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            quote.Id,
            quote.RequestId,
            quote.QuoteNumber,
            quote.Status,
            quote.CreatedAt,
            quote.ValidUntil,
            quote.Notes,
            quote.TravelCost,
            quote.Subtotal,
            quote.Total,
            Items = quote.Items.Select(item => new
            {
                item.Id,
                item.Description,
                item.Quantity,
                item.Unit,
                item.UnitPrice,
                item.Total
            })
        });
    }

    [HttpPost("{quoteId:guid}/items")]
    public async Task<IActionResult> AddItem(
        Guid quoteId,
        AddQuoteItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await quoteService.AddItemAsync(
            quoteId,
            request.Description,
            request.Quantity,
            request.Unit,
            request.UnitPrice,
            cancellationToken);

        return Created(
            $"/api/quotes/{quoteId}/items/{item.Id}",
            new
            {
                item.Id,
                item.Description,
                item.Quantity,
                item.Unit,
                item.UnitPrice,
                item.Total
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateQuoteRequest request,
        CancellationToken cancellationToken)
    {
        var quote = await quoteService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(new
        {
            quote.Id,
            quote.RequestId,
            quote.QuoteNumber,
            quote.Status,
            quote.CreatedAt,
            quote.ValidUntil,
            quote.Notes,
            quote.TravelCost,
            quote.Subtotal,
            quote.Total,
            Items = quote.Items.Select(item => new
            {
                item.Id,
                item.Description,
                item.Quantity,
                item.Unit,
                item.UnitPrice,
                item.Total
            })
        });
    }

    [HttpPut("{quoteId:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> UpdateItem(
        Guid quoteId,
        Guid itemId,
        UpdateQuoteItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await quoteService.UpdateItemAsync(
            quoteId,
            itemId,
            request,
            cancellationToken);

        return Ok(new
        {
            item.Id,
            item.Description,
            item.Quantity,
            item.Unit,
            item.UnitPrice,
            item.Total
        });
    }

    [HttpDelete("{quoteId:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> DeleteItem(
        Guid quoteId,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        await quoteService.DeleteItemAsync(
            quoteId,
            itemId,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{quoteId:guid}/travel-cost")]
    public async Task<IActionResult> UpdateTravelCost(
        Guid quoteId,
        UpdateTravelCostRequest request,
        CancellationToken cancellationToken)
    {
        var quote = await quoteService.UpdateTravelCostAsync(
            quoteId,
            request.TravelCost,
            cancellationToken);

        return Ok(new
        {
            quote.Id,
            quote.QuoteNumber,
            quote.TravelCost,
            quote.Subtotal,
            quote.Total
        });
    }

    [HttpPut("{quoteId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid quoteId,
        UpdateQuoteStatusRequest request,
        CancellationToken cancellationToken)
    {
        var quote = await quoteService.UpdateStatusAsync(
            quoteId,
            request.Status,
            cancellationToken);

        return Ok(new
        {
            quote.Id,
            quote.QuoteNumber,
            quote.Status
        });
    }


    [HttpGet("{id:guid}/pdf")]
    public async Task<IActionResult> GetPdf(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await quotePdfService.GenerateAsync(
            id,
            cancellationToken);

        return File(
            result.Content,
            "application/pdf",
            result.FileName);
    }

    [HttpPost("{id:guid}/send")]
    public async Task<ActionResult> Send(
        Guid id,
        CancellationToken cancellationToken)
    {
        var quote = await quoteService.SendAsync(
            id,
            cancellationToken);

        return Ok(new
        {
            quote.Id,
            quote.RequestId,
            quote.QuoteNumber,
            quote.Status,
            quote.CreatedAt,
            quote.ValidUntil,
            quote.Notes,
            quote.TravelCost,
            quote.Subtotal,
            quote.Total,
            Items = quote.Items.Select(item => new
            {
                item.Id,
                item.Description,
                item.Quantity,
                item.Unit,
                item.UnitPrice,
                item.Total
            })
        });
    }

}

public record CreateQuoteRequest(Guid RequestId);

public record UpdateTravelCostRequest(decimal TravelCost);

public record UpdateQuoteStatusRequest(QuoteStatus Status);