import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ApiService } from '../../../core/services/api.service';
import { RequestDetail } from '../models/request-detail.model';

@Component({
    selector: 'app-request-detail-page',
    standalone: true,
    templateUrl: './request-detail-page.html',
    styleUrl: './request-detail-page.scss'
})
export class RequestDetailPage {

    private readonly api = inject(ApiService);
    private readonly route = inject(ActivatedRoute);
    private readonly router = inject(Router);

    readonly request = signal<RequestDetail | null>(null);
    readonly loading = signal(true);
    readonly error = signal('');

    readonly updatingStatus = signal(false);
    readonly showRejectConfirmation = signal(false);

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');

        if (!id) {
            this.error.set('Die Anfrage-ID fehlt.');
            this.loading.set(false);
            return;
        }

        this.loadRequest(id);
    }

    private loadRequest(id: string): void {
        this.loading.set(true);
        this.error.set('');

        this.api.getRequest(id).subscribe({
            next: request => {
                this.request.set(request);
                this.loading.set(false);
            },
            error: err => {
                console.error('Failed to load request:', err);

                this.error.set(
                    'Die Anfrage konnte nicht geladen werden.'
                );

                this.loading.set(false);
            }
        });
    }

    goBack(): void {
        const request = this.request();

        if (request) {
            this.router.navigate([
                '/customers',
                request.customerId
            ]);

            return;
        }

        this.router.navigate(['/customers']);
    }

    formatDate(date: string): string {
        return new Intl.DateTimeFormat('de-CH', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        }).format(new Date(date));
    }

    formatTime(time: string): string {
        return time.substring(0, 5);
    }

    updateStatus(status: string): void {
        const request = this.request();

        if (!request || request.status === status) {
            return;
        }

        this.updatingStatus.set(true);

        this.api.updateRequestStatus(request.id, status).subscribe({
            next: response => {
                this.request.update(current =>
                    current
                        ? {
                            ...current,
                            status: response.status
                        }
                        : current
                );

                this.updatingStatus.set(false);
            },
            error: err => {
                console.error('Failed to update request status:', err);
                this.updatingStatus.set(false);
            }
        });
    }

    confirmReject(): void {
        this.showRejectConfirmation.set(true);
    }

    cancelReject(): void {
        this.showRejectConfirmation.set(false);
    }

    rejectRequest(): void {
        this.showRejectConfirmation.set(false);
        this.updateStatus('Rejected');
    }
}