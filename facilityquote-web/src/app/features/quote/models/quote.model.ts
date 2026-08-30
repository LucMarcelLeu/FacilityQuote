export interface Quote {
    id: string;
    requestId: string;
    quoteNumber: string;
    status: 'Draft' | 'Sent' | 'Accepted' | 'Rejected';
    createdAt: string;
    validUntil: string | null;
    travelCost: number;
    subtotal: number;
    total: number;
}