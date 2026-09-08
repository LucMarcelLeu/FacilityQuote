import { Component, OnInit, inject, signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { startWith, switchMap, tap } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { TranslatePipe } from '@ngx-translate/core';

import { ApiService } from '../../../core/services/api.service';
import { SiteHeaderComponent } from '../../../shared/components/site-header/site-header';

import {
  LanguageService
} from '../../../core/services/language.service';

import {
  Service,
  ServiceCategory
} from '../models/service.model';
import { RequestDraft } from '../models/request-draft.model';

type RequestStep = 1 | 2 | 3 | 4;

type TimeSlot = 'morning' | 'afternoon';

interface AvailableDate {
  date: string;
  label: string;
  weekday: string;

  morningAvailable: boolean;
  afternoonAvailable: boolean;

  morningBooked: boolean;
  afternoonBooked: boolean;
}

@Component({
  selector: 'app-request-page',
  standalone: true,
  imports: [
    AsyncPipe,
    FormsModule,
    SiteHeaderComponent,
    TranslatePipe
  ],
  templateUrl: './request-page.html',
  styleUrl: './request-page.scss'
})

export class RequestPage implements OnInit {
  private readonly api = inject(ApiService);
  private readonly translate = inject(TranslateService);
  private readonly languageService = inject(LanguageService);

  readonly services$ = this.translate.onLangChange.pipe(
    startWith(null),
    switchMap(() => this.api.getServices())
  );

  readonly categories: ServiceCategory[] = [
    'Cleaning',
    'Clearance',
    'Gardening'
  ];

  currentStep: RequestStep = 1;

  isSubmitting = signal(false);
  submitSuccess = signal(false);
  requestId = signal<string | null>(null);

  draft: RequestDraft = {
    serviceId: null,

    language: this.languageService.language,

    location: {
      street: '',
      postalCode: '',
      city: ''
    },

    appointment: {
      date: null,
      timeSlot: null
    },

    customer: {
      firstName: '',
      lastName: '',
      email: '',
      phone: '',
    },
    description: '',
    quantity: null
  };

  availableDates: AvailableDate[] = [];

  selectedServiceName = '';
  selectedServiceUnit = '';

  ngOnInit(): void {
    this.loadAvailability();

    this.translate.onLangChange.subscribe(() => {
      this.draft.language = this.languageService.language;

      if (this.draft.serviceId) {
        this.api.getServices().subscribe(services => {
          const selectedService = services.find(
            service => service.id === this.draft.serviceId
          );

          if (selectedService) {
            this.selectedServiceName = selectedService.name;
            this.selectedServiceUnit = selectedService.unit;
          }
        });
      }
    });
  }

  selectService(service: Service): void {
    this.draft.serviceId = service.id;
    this.selectedServiceName = service.name;
    this.selectedServiceUnit = service.unit;
  }

  isSelected(service: Service): boolean {
    return this.draft.serviceId === service.id;
  }


  goToStep(step: RequestStep): void {
    this.currentStep = step;
  }

  canContinueFromStep1(): boolean {
    return this.draft.serviceId !== null &&
      this.draft.quantity !== null &&
      this.draft.quantity > 0;
  }

  canContinueFromStep2(): boolean {
    return !!this.draft.location?.street &&
      !!this.draft.location?.postalCode &&
      !!this.draft.location?.city;
  }

  saveLocation(): void {
    // Wird später durch Reactive Forms ersetzt.
  }

  private loadAvailability(): void {

    const today = new Date();

    const from = this.toDateString(today);

    const toDate = new Date(today);
    toDate.setDate(today.getDate() + 14);

    const to = this.toDateString(toDate);

    this.api.getAvailableDates(from, to)
      .subscribe({
        next: availability => {

          this.availableDates = availability
            .filter(item =>
              (item.morningAvailable && !item.morningBooked) ||
              (item.afternoonAvailable && !item.afternoonBooked)
            )
            .map(item => {

              const date = new Date(
                item.date + 'T12:00:00'
              );

              return {
                date: item.date,

                label: date.toLocaleDateString(this.getDateLocale(), {
                  day: '2-digit',
                  month: '2-digit'
                }),

                weekday: date.toLocaleDateString(this.getDateLocale(), {
                  weekday: 'short'
                }),

                morningAvailable: item.morningAvailable,
                afternoonAvailable: item.afternoonAvailable,

                morningBooked: item.morningBooked,
                afternoonBooked: item.afternoonBooked
              };
            });

        },

        error: error => {
          console.error(
            'Failed to load availability',
            error
          );
        }
      });
  }

  getSelectedDate(): AvailableDate | undefined {
    const selectedDate = this.draft.appointment.date;

    if (!selectedDate) {
      return undefined;
    }

    return this.availableDates.find(
      x => x.date === selectedDate
    );
  }

  private toDateString(date: Date): string {
    return date.toISOString().substring(0, 10);
  }

  selectDate(date: string): void {
    this.draft.appointment.date = date;

    const selected = this.availableDates.find(
      x => x.date === date
    );

    if (!selected) {
      this.draft.appointment.timeSlot = null;
      return;
    }

    if (
      this.draft.appointment.timeSlot === 'morning' &&
      (!selected.morningAvailable || selected.morningBooked)
    ) {
      this.draft.appointment.timeSlot = null;
    }

    if (
      this.draft.appointment.timeSlot === 'afternoon' &&
      (!selected.afternoonAvailable || selected.afternoonBooked)
    ) {
      this.draft.appointment.timeSlot = null;
    }
  }

  selectTimeSlot(slot: TimeSlot): void {

    const selected = this.getSelectedDate();

    if (!selected) {
      return;
    }

    if (slot === 'morning') {
      if (!selected.morningAvailable || selected.morningBooked) {
        return;
      }
    }

    if (slot === 'afternoon') {
      if (!selected.afternoonAvailable || selected.afternoonBooked) {
        return;
      }
    }

    this.draft.appointment.timeSlot = slot;
  }

  isDateSelected(date: string): boolean {
    return this.draft.appointment.date === date;
  }

  isTimeSlotSelected(slot: TimeSlot): boolean {
    return this.draft.appointment.timeSlot === slot;
  }

  canContinueFromStep3(): boolean {
    return !!this.draft.appointment.date &&
      !!this.draft.appointment.timeSlot;
  }

  get selectedDateLabel(): string {
    const date = this.draft.appointment.date;

    if (!date) {
      return '';
    }

    return new Date(date + 'T12:00:00').toLocaleDateString(
      this.getDateLocale(),
      {
        weekday: 'long',
        day: '2-digit',
        month: 'long',
        year: 'numeric'
      }
    );
  }

  get selectedTimeSlotLabel(): string {
    switch (this.draft.appointment.timeSlot) {
      case 'morning':
        return `${this.translate.instant('REQUEST.STEP3.MORNING')} · ${this.translate.instant('REQUEST.STEP3.MORNING_TIME')}`;

      case 'afternoon':
        return `${this.translate.instant('REQUEST.STEP3.AFTERNOON')} · ${this.translate.instant('REQUEST.STEP3.AFTERNOON_TIME')}`;

      default:
        return '';
    }
  }

  private getDateLocale(): string {
    return this.languageService.language === 'de'
      ? 'de-CH'
      : 'en-GB';
  }

  canSubmit(): boolean {
    const customer = this.draft.customer;

    return !!customer.firstName.trim() &&
      !!customer.lastName.trim() &&
      !!customer.email.trim() &&
      !!customer.phone.trim();
  }

  submitRequest(): void {
    if (!this.canSubmit() || !this.draft.serviceId) {
      return;
    }

    this.isSubmitting.set(true);

    this.draft.language = this.languageService.language;

    this.api.createRequest(this.draft)
      .subscribe({
        next: result => {
          console.log('Request created:', result);

          this.requestId.set(result.id);
          this.submitSuccess.set(true);
          this.isSubmitting.set(false);

          console.log('submitSuccess:', this.submitSuccess());
          console.log('requestId:', this.requestId());
        },

        error: error => {
          console.error('Failed to create request', error);
          this.isSubmitting.set(false);
        }
      });
  }

}