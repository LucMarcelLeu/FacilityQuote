export interface Customer {
    id: string;
    firstName: string;
    lastName: string;
    companyName: string | null;
    street: string;
    postalCode: string;
    city: string;
    email: string;
    phone: string | null;
}