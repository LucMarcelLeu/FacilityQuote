import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { ApiService } from '../../core/services/api.service';
import { Quote } from '../quote/models/quote.model';

@Component({
    selector: 'app-quotes',
    imports: [
        DatePipe,
        DecimalPipe
    ],
    templateUrl: './quotes-page.html',
    styleUrl: './quotes-page.scss'
})
export class QuotesPage implements OnInit {

    private readonly api = inject(ApiService);

    readonly quotes = signal<Quote[]>([]);
    readonly loading = signal(true);

    ngOnInit(): void {
        this.loadQuotes();
    }

    private loadQuotes(): void {
        this.loading.set(true);

        this.api.getQuotes().subscribe({
            next: quotes => {
                this.quotes.set(quotes);
                this.loading.set(false);
            },
            error: error => {
                console.error('Failed to load quotes', error);
                this.loading.set(false);
            }
        });
    }
}