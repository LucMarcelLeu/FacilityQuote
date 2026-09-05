import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subject, finalize, startWith, switchMap } from 'rxjs';

import { ApiService } from '../../../core/services/api.service';
import {
    Service,
    ServiceCategory
} from '../../request/models/service.model';

@Component({
    selector: 'app-services-page',
    standalone: true,
    imports: [
        AsyncPipe,
        DecimalPipe,
        ReactiveFormsModule
    ],
    templateUrl: './services-page.html',
    styleUrl: './services-page.scss'
})
export class ServicesPage {

    private readonly api = inject(ApiService);
    private readonly fb = inject(FormBuilder);

    private readonly reloadServices$ = new Subject<void>();

    readonly services$ = this.reloadServices$.pipe(
        startWith(void 0),
        switchMap(() => this.api.getAllServices())
    );

    readonly showForm = signal(false);
    readonly isSaving = signal(false);

    editingService: Service | null = null;

    readonly serviceForm = this.fb.nonNullable.group({
        serviceCategory: ['Cleaning' as ServiceCategory, Validators.required],
        name: ['', Validators.required],
        description: [''],
        unit: ['', Validators.required],
        unitPrice: [0, [Validators.required, Validators.min(0)]],
        isActive: [true]
    });

    getCategoryLabel(category: ServiceCategory): string {
        switch (category) {
            case 'Cleaning':
                return 'Reinigung';

            case 'Clearance':
                return 'Räumung';

            case 'Gardening':
                return 'Garten';

            default:
                return category;
        }
    }

    openCreateForm(): void {
        this.editingService = null;

        this.serviceForm.reset({
            serviceCategory: 'Cleaning',
            name: '',
            description: '',
            unit: '',
            unitPrice: 0,
            isActive: true
        });

        this.showForm.set(true);
    }

    openEditForm(service: Service): void {
        this.editingService = service;

        this.serviceForm.reset({
            serviceCategory: service.category,
            name: service.name,
            description: service.description ?? '',
            unit: service.unit,
            unitPrice: service.unitPrice,
            isActive: service.isActive
        });

        this.showForm.set(true);
    }

    closeForm(): void {
        this.showForm.set(false);
        this.editingService = null;
    }

    createService(): void {
        if (this.serviceForm.invalid || this.isSaving()) {
            this.serviceForm.markAllAsTouched();
            return;
        }

        this.isSaving.set(true);

        const request = this.serviceForm.getRawValue();

        this.api.createService(request)
            .pipe(
                finalize(() => this.isSaving.set(false))
            )
            .subscribe({
                next: () => {
                    this.reloadServices$.next();
                    this.closeForm();
                },
                error: error => {
                    console.error(
                        'Fehler beim Erstellen des Services:',
                        error
                    );
                }
            });
    }

    updateService(): void {
        if (
            this.serviceForm.invalid ||
            !this.editingService ||
            this.isSaving()
        ) {
            this.serviceForm.markAllAsTouched();
            return;
        }

        this.isSaving.set(true);

        const request = this.serviceForm.getRawValue();

        this.api.updateService(
            this.editingService.id,
            request
        )
            .pipe(
                finalize(() => this.isSaving.set(false))
            )
            .subscribe({
                next: () => {
                    this.reloadServices$.next();
                    this.closeForm();
                },
                error: error => {
                    console.error(
                        'Fehler beim Aktualisieren des Services:',
                        error
                    );
                }
            });
    }

    saveService(): void {
        if (this.editingService) {
            this.updateService();
        } else {
            this.createService();
        }
    }
}