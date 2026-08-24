import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Service } from '../../features/request/models/service.model';
import { RequestDraft } from '../../features/request/models/request-draft.model';
import { Availability } from '../../features/availability/models/availability.model';

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    private readonly http = inject(HttpClient);

    private readonly baseUrl = environment.apiUrl;

    getServices(): Observable<Service[]> {
        return this.http.get<Service[]>(
            `${this.baseUrl}/services`
        );
    }

    createRequest(request: RequestDraft): Observable<{ id: string }> {

        const apiRequest = {
            firstName: request.customer.firstName,
            lastName: request.customer.lastName,
            companyName: null,

            customerStreet: request.location.street,
            customerPostalCode: request.location.postalCode,
            customerCity: request.location.city,

            email: request.customer.email,
            phone: request.customer.phone,

            locationStreet: request.location.street,
            locationPostalCode: request.location.postalCode,
            locationCity: request.location.city,

            serviceId: request.serviceId,

            desiredDate: request.appointment.date,

            earliestTime:
                request.appointment.timeSlot === 'morning'
                    ? '08:00:00'
                    : '13:00:00',

            latestTime:
                request.appointment.timeSlot === 'morning'
                    ? '12:00:00'
                    : '17:00:00',

            description: request.description
        };

        return this.http.post<{ id: string }>(
            `${this.baseUrl}/requests`,
            apiRequest
        );
    }

    getAvailableDates(
        from: string,
        to: string
    ): Observable<Availability[]> {

        return this.http.get<Availability[]>(
            '/api/requests/available-dates',
            {
                params: {
                    from,
                    to
                }
            }
        );
    }

    getAdminAvailability(
        from: string,
        to: string
    ): Observable<Availability[]> {

        return this.http.get<Availability[]>(
            '/api/admin/availability',
            {
                params: {
                    from,
                    to
                }
            }
        );
    }

    getAvailability(
        from: string,
        to: string
    ): Observable<Availability[]> {

        return this.http.get<Availability[]>(
            `/api/availability?from=${from}&to=${to}`
        );
    }

    setAvailability(
        date: string,
        morningAvailable: boolean,
        afternoonAvailable: boolean
    ): Observable<Availability> {

        return this.http.put<Availability>(
            `/api/admin/availability/${date}`,
            {
                morningAvailable,
                afternoonAvailable
            }
        );
    }
}