import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';

import { ApiService } from '../../core/services/api.service';
import { SiteHeaderComponent } from '../../shared/components/site-header/site-header';

import {
    Service,
    ServiceCategory
} from '../request/models/service.model';
import { startWith, switchMap } from 'rxjs';

@Component({
    selector: 'app-home',
    standalone: true,
    imports: [
        AsyncPipe,
        RouterLink,
        TranslatePipe,
        SiteHeaderComponent
    ],
    templateUrl: './home.html',
    styleUrl: './home.scss'
})
export class HomePage {

    private readonly api = inject(ApiService);

    private readonly translate = inject(TranslateService);

    readonly services$ = this.translate.onLangChange.pipe(
        startWith(null),
        switchMap(() => this.api.getServices())
    );

    readonly currentYear = new Date().getFullYear();

    readonly categories: {
        key: ServiceCategory;
        number: string;
        titleKey: string;
        descriptionKey: string;
    }[] = [
            {
                key: 'Cleaning',
                number: '01',
                titleKey: 'HOME.SERVICES.CLEANING.TITLE',
                descriptionKey: 'HOME.SERVICES.CLEANING.DESCRIPTION'
            },
            {
                key: 'Clearance',
                number: '02',
                titleKey: 'HOME.SERVICES.CLEARANCE.TITLE',
                descriptionKey: 'HOME.SERVICES.CLEARANCE.DESCRIPTION'
            },
            {
                key: 'Gardening',
                number: '03',
                titleKey: 'HOME.SERVICES.GARDENING.TITLE',
                descriptionKey: 'HOME.SERVICES.GARDENING.DESCRIPTION'
            }
        ];

    getServices(category: ServiceCategory, services: Service[]): Service[] {
        return services.filter(
            service => service.category === category
        );
    }
}