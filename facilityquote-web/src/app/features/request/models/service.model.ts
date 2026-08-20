export type ServiceCategory =
    | 'Cleaning'
    | 'Clearance'
    | 'Gardening';

export interface Service {
    id: string;
    category: ServiceCategory;
    name: string;
    description?: string;
}