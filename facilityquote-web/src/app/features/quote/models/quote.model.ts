export type QuoteStatus =
    | 'Draft'
    | 'Sent'
    | 'Accepted'
    | 'Rejected'
    | 'Completed';

export interface QuoteItem {
    id: string;
    description: string;
    quantity: number;
    unit: string;
    unitPrice: number;
    total: number;
}

export interface Quote {
    id: string;
    requestId: string;
    quoteNumber: string;
    status: QuoteStatus;
    createdAt: string;
    validUntil: string | null;
    notes: string | null;
    travelCost: number;
    subtotal: number;
    total: number;
    items: QuoteItem[];
}