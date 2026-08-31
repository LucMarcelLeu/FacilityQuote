import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ApiService } from '../../../core/services/api.service';
import { UpdateQuoteRequest } from '../models/quote-update.model';

export interface QuoteItem {
  id: string;
  description: string;
  quantity: number;
  unit: string;
  unitPrice: number;
  total: number;
}

export interface Quote {
  id: string;
  requestId: string;
  quoteNumber: string;
  status: string;
  createdAt: string;
  validUntil: string | null;
  notes: string | null;
  travelCost: number;
  subtotal: number;
  total: number;
  items: QuoteItem[];
}

@Component({
  selector: 'app-quote-detail-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './quote-detail-page.html',
  styleUrl: './quote-detail-page.scss'
})
export class QuoteDetailPage implements OnInit {

  private readonly api = inject(ApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly quote = signal<Quote | null>(null);

  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  readonly editing = signal(false);
  readonly saving = signal(false);

  readonly editValidUntil = signal<string | null>(null);
  readonly editNotes = signal('');
  readonly editTravelCost = signal(0);

  updatingStatus = signal(false);
  downloadingPdf = signal(false);

  ngOnInit(): void {

    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.error.set('Keine Offerte angegeben.');
      this.loading.set(false);
      return;
    }

    this.loadQuote(id);
  }

  private loadQuote(id: string): void {

    this.loading.set(true);
    this.error.set(null);

    this.api.getQuote(id)
      .subscribe({

        next: quote => {

          this.quote.set(quote);
          this.loading.set(false);
        },

        error: error => {

          console.error(
            'Failed to load quote',
            error
          );

          this.error.set(
            'Die Offerte konnte nicht geladen werden.'
          );

          this.loading.set(false);
        }
      });
  }

  startEditing(): void {

    const quote = this.quote();

    if (!quote) {
      return;
    }

    this.editValidUntil.set(
      quote.validUntil
        ? quote.validUntil.substring(0, 10)
        : null
    );

    this.editNotes.set(
      quote.notes ?? ''
    );

    this.editTravelCost.set(
      quote.travelCost
    );

    this.editing.set(true);
  }

  cancelEditing(): void {
    this.editing.set(false);
  }

  saveQuote(): void {

    const quote = this.quote();

    if (!quote) {
      return;
    }

    this.saving.set(true);

    const request: UpdateQuoteRequest = {
      validUntil: this.editValidUntil(),
      notes: this.editNotes().trim() || null,
      travelCost: Number(this.editTravelCost())
    };

    this.api.updateQuote(
      quote.id,
      request
    ).subscribe({

      next: updatedQuote => {

        this.quote.set(updatedQuote);

        this.editing.set(false);
        this.saving.set(false);
      },

      error: error => {

        console.error(
          'Failed to update quote',
          error
        );

        this.saving.set(false);
      }
    });
  }

  sendQuote(): void {
    const currentQuote = this.quote();

    if (!currentQuote) {
      return;
    }

    if (!confirm(
      `Offerte ${currentQuote.quoteNumber} wirklich senden?\n\n` +
      'Danach kann die Offerte nicht mehr bearbeitet werden.'
    )) {
      return;
    }

    this.updatingStatus.set(true);

    this.api.updateQuoteStatus(
      currentQuote.id,
      'Sent'
    ).subscribe({
      next: updatedQuote => {

        this.quote.update(current => current
          ? {
            ...current,
            status: updatedQuote.status
          }
          : current
        );

        this.updatingStatus.set(false);
      },

      error: error => {
        console.error(
          'Failed to send quote',
          error
        );

        this.updatingStatus.set(false);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/quotes']);
  }

  formatDate(value: string | null): string {

    if (!value) {
      return '';
    }

    return new Date(value).toLocaleDateString(
      'de-CH',
      {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      }
    );
  }

  formatCurrency(value: number): string {

    return value.toLocaleString(
      'de-CH',
      {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      }
    );
  }

  get statusLabel(): string {

    const status = this.quote()?.status;

    switch (status) {

      case 'Draft':
        return 'Entwurf';

      case 'Sent':
        return 'Gesendet';

      case 'Accepted':
        return 'Angenommen';

      case 'Rejected':
        return 'Abgelehnt';

      case 'Completed':
        return 'Abgeschlossen';

      default:
        return status ?? '';
    }
  }

  downloadPdf(): void {
    const quote = this.quote();
    if (!quote) { return; } this.downloadingPdf.set(true);
    this.api.downloadQuotePdf(quote.id)
      .subscribe({
        next: blob => {
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url; link.download = `${quote.quoteNumber}.pdf`;
          link.click(); window.URL.revokeObjectURL(url);
          this.downloadingPdf.set(false);
        },
          error: error => {
          console.error('Failed to download quote PDF', error);
          this.downloadingPdf.set(false);
        }
      });
  }
}