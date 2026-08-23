export interface Availability {
    date: string;
    morningAvailable: boolean;
    afternoonAvailable: boolean;
}

export interface AvailabilityDay {
    date: string;
    label: string;
    weekday: string;
    morningAvailable: boolean;
    afternoonAvailable: boolean;
}