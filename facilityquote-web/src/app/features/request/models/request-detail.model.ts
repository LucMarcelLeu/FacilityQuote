export interface RequestDetail {
    id: string;

    customerId: string;
    customerName: string;
    email: string;
    phone: string | null;

    serviceId: string;
    service: string;

    desiredDate: string;
    earliestTime: string;
    latestTime: string;

    street: string;
    postalCode: string;
    city: string;

    description: string | null;

    status: string;

    createdAt: string;
}