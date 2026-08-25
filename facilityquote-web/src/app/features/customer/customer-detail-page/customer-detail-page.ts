import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ApiService } from '../../../core/services/api.service';
import { Customer } from '../../customer/models/customer.model';

@Component({
    selector: 'app-customer-detail-page',
    standalone: true,
    templateUrl: './customer-detail-page.html',
    styleUrl: './customer-detail-page.scss'
})
export class CustomerDetailPage {

    private readonly api = inject(ApiService);
    private readonly route = inject(ActivatedRoute);
    private readonly router = inject(Router);

    readonly customer = signal<Customer | null>(null);

    readonly loading = signal(true);

    readonly error = signal('');

    readonly fullName = computed(() => {
        const customer = this.customer();

        if (!customer) {
            return '';
        }

        return `${customer.firstName} ${customer.lastName}`;
    });

    ngOnInit(): void {

        const id = this.route.snapshot.paramMap.get('id');

        if (!id) {
            this.error.set('Keine Kunden-ID angegeben.');
            this.loading.set(false);
            return;
        }

        this.loadCustomer(id);
    }

    private loadCustomer(id: string): void {

        this.loading.set(true);
        this.error.set('');

        this.api.getCustomer(id).subscribe({
            next: customer => {

                console.log('CUSTOMER RESPONSE:', customer);

                this.customer.set(customer);
                this.loading.set(false);

                console.log('CUSTOMER LOADING:', this.loading());
            },

            error: err => {

                console.error('Failed to load customer:', err);

                if (err.status === 404) {
                    this.error.set(
                        'Der Kunde wurde nicht gefunden.'
                    );
                } else {
                    this.error.set(
                        'Der Kunde konnte nicht geladen werden.'
                    );
                }

                this.loading.set(false);
            }
        });
    }

    goBack(): void {
        this.router.navigate(['/customers']);
    }
}