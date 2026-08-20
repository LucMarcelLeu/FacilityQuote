import { Component, inject, signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ApiService } from '../../../core/services/api.service';
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
}

@Component({
  selector: 'app-request-page',
  standalone: true,
  imports: [
    AsyncPipe,
    FormsModule
  ],
  templateUrl: './request-page.html',
  styleUrl: './request-page.scss'
})

export class RequestPage {
  private readonly api = inject(ApiService);

  readonly services$ = this.api.getServices();

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
    description: ''
  };

  selectedServiceName = '';

  selectService(service: Service): void {
    this.draft.serviceId = service.id;
    this.selectedServiceName = service.name;
  }

  isSelected(service: Service): boolean {
    return this.draft.serviceId === service.id;
  }

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

  goToStep(step: RequestStep): void {
    this.currentStep = step;
  }

  canContinueFromStep1(): boolean {
    return this.draft.serviceId !== null;
  }

  canContinueFromStep2(): boolean {
    return !!this.draft.location?.street &&
      !!this.draft.location?.postalCode &&
      !!this.draft.location?.city;
  }

  saveLocation(): void {
    // Wird später durch Reactive Forms ersetzt.
  }

  readonly availableDates: AvailableDate[] =
    this.createAvailableDates();

  private createAvailableDates(): AvailableDate[] {
    const dates: AvailableDate[] = [];

    const today = new Date();

    for (let i = 1; i <= 14; i++) {
      const date = new Date(today);

      date.setDate(today.getDate() + i);

      dates.push({
        date: this.toDateString(date),
        label: date.toLocaleDateString('de-CH', {
          day: '2-digit',
          month: '2-digit'
        }),
        weekday: date.toLocaleDateString('de-CH', {
          weekday: 'short'
        })
      });
    }

    return dates;
  }

  private toDateString(date: Date): string {
    return date.toISOString().substring(0, 10);
  }

  selectDate(date: string): void {
    this.draft.appointment.date = date;
  }

  selectTimeSlot(slot: TimeSlot): void {
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
      'de-CH',
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
        return 'Vormittag · 08:00 – 12:00';

      case 'afternoon':
        return 'Nachmittag · 13:00 – 17:00';

      default:
        return '';
    }
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