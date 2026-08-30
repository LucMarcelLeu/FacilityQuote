export interface RequestDraft {
    serviceId: string | null;

    location: {
        street: string;
        postalCode: string;
        city: string;
    }

    appointment: {
        date: string | null;
        timeSlot: 'morning' | 'afternoon' | null;
    };

    customer: {
        firstName: string;
        lastName: string;
        email: string;
        phone: string;
    };

    description: string;
    quantity: number | null;
}