import { Component, inject, signal, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { ApiService } from '../../core/services/api.service';
import { Customer } from '../customer/models/customer.model';

@Component({
    selector: 'app-customers-page',
    standalone: true,
    imports: [FormsModule, RouterLink],
    templateUrl: './customers-page.html',
    styleUrl: './customers-page.scss'
})
export class CustomersPage {

    private readonly api = inject(ApiService);

    readonly customers = signal<Customer[]>([]);
    readonly loading = signal(true);
    readonly error = signal('');
    readonly searchTerm = signal('');

    readonly filteredCustomers = computed(() => {

        const customers = this.customers();
        const term = this.searchTerm().trim().toLowerCase();

        if (!term) {
            return customers;
        }

        return customers.filter(customer =>
            this.getFullName(customer).toLowerCase().includes(term) ||
            (customer.companyName ?? '').toLowerCase().includes(term) ||
            customer.email.toLowerCase().includes(term) ||
            (customer.phone ?? '').toLowerCase().includes(term) ||
            customer.city.toLowerCase().includes(term) ||
            customer.postalCode.toLowerCase().includes(term)
        );
    });

    ngOnInit(): void {
        this.loadCustomers();
    }

    loadCustomers(): void {

        this.loading.set(true);
        this.error.set('');

        this.api.getCustomers().subscribe({

            next: customers => {

                console.log('CUSTOMERS RESPONSE:', customers);

                this.customers.set(customers);
                this.loading.set(false);

                console.log('LOADING:', this.loading());
                console.log('CUSTOMERS:', this.customers().length);
            },

            error: err => {

                console.error(
                    'Failed to load customers:',
                    err
                );

                this.error.set(
                    'Die Kunden konnten nicht geladen werden.'
                );

                this.loading.set(false);
            }
        });
    }

    getFullName(customer: Customer): string {
        return `${customer.firstName} ${customer.lastName}`;
    }
}