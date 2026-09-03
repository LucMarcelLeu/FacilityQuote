export interface Availability {

    date: string;

    morningAvailable: boolean;
    afternoonAvailable: boolean;

    morningBooked: boolean;
    afternoonBooked: boolean;
}

export interface AvailabilityDay {

    date: string;

    label: string;
    weekday: string;

    morningAvailable: boolean;
    afternoonAvailable: boolean;

    morningBooked: boolean;
    afternoonBooked: boolean;
}