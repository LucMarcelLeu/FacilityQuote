import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { ApiService } from '../../core/services/api.service';
import {
    Service,
    ServiceCategory
} from '../request/models/service.model';

@Component({
    selector: 'app-home',
    standalone: true,
    imports: [
        AsyncPipe,
        RouterLink
    ],
    templateUrl: './home.html',
    styleUrl: './home.scss'
})
export class HomePage {

    private readonly api = inject(ApiService);

    readonly services$ = this.api.getServices();

    readonly currentYear = new Date().getFullYear();

    readonly categories: {
        key: ServiceCategory;
        number: string;
        title: string;
        description: string;
    }[] = [
            {
                key: 'Cleaning',
                number: '01',
                title: 'Reinigung',
                description: 'Saubere Lösungen für private und gewerbliche Räume.'
            },
            {
                key: 'Clearance',
                number: '02',
                title: 'Räumung',
                description: 'Zuverlässige Räumungen und fachgerechte Entsorgung.'
            },
            {
                key: 'Gardening',
                number: '03',
                title: 'Garten',
                description: 'Unterstützung bei Pflege, Unterhalt und Gartenarbeiten.'
            }
        ];

    getServices(category: ServiceCategory, services: Service[]): Service[] {
        return services.filter(
            service => service.category === category
        );
    }
}