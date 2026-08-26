export interface CustomerRequest {
    id: string;

    service: string;

    desiredDate: string;

    earliestTime: string;
    latestTime: string;

    status: string;

    street: string;
    postalCode: string;
    city: string;
}