import {
  Component,
  inject,
  OnInit,
  signal
} from '@angular/core';

import { ApiService } from '../../../core/services/api.service';
import { Availability } from '../models/availability.model';

interface AvailabilityDay {
  date: string;
  label: string;
  weekday: string;
  morningAvailable: boolean;
  afternoonAvailable: boolean;
}

@Component({
  selector: 'app-availability-page',
  standalone: true,
  templateUrl: './availability-page.html',
  styleUrl: './availability-page.scss'
})
export class AvailabilityPage {

  private readonly api = inject(ApiService);


  // -------------------------------------------------------
  // STATE
  // -------------------------------------------------------

  readonly days = signal<AvailabilityDay[]>([]);

  readonly loading = signal(true);

  readonly savingDate = signal<string | null>(null);

  readonly savedDate = signal<string | null>(null);


  // -------------------------------------------------------
  // INIT
  // -------------------------------------------------------

  constructor() {
    this.loadAvailability();
  }

  // -------------------------------------------------------
  // LOAD
  // -------------------------------------------------------

  loadAvailability(): void {

    this.loading.set(true);

    const from = this.toDateString(new Date());

    const toDate = new Date();

    toDate.setDate(
      toDate.getDate() + 13
    );

    const to = this.toDateString(toDate);


    console.log(
      'Loading availability:',
      {
        from,
        to
      }
    );


    this.api.getAvailability(from, to)
      .subscribe({

        next: availability => {

          console.log(
            'Availability received:',
            availability
          );


          const availabilityMap =
            new Map<string, Availability>(
              availability.map(item => [
                item.date,
                item
              ])
            );


          const days =
            this.createDays(
              from,
              to,
              availabilityMap
            );


          console.log(
            'Days created:',
            days
          );


          this.days.set(days);

          this.loading.set(false);
        },


        error: error => {

          console.error(
            'Failed to load availability:',
            error
          );

          this.loading.set(false);
        }

      });
  }


  // -------------------------------------------------------
  // CREATE DAYS
  // -------------------------------------------------------

  private createDays(
    from: string,
    to: string,
    availabilityMap: Map<string, Availability>
  ): AvailabilityDay[] {

    const days: AvailabilityDay[] = [];

    const start =
      new Date(
        `${from}T12:00:00`
      );

    const end =
      new Date(
        `${to}T12:00:00`
      );


    const current =
      new Date(start);


    while (current <= end) {

      const date =
        this.toDateString(current);


      const existing =
        availabilityMap.get(date);


      days.push({

        date,

        label:
          current.toLocaleDateString(
            'de-CH',
            {
              day: '2-digit',
              month: '2-digit'
            }
          ),

        weekday:
          current.toLocaleDateString(
            'de-CH',
            {
              weekday: 'short'
            }
          ),

        morningAvailable:
          existing?.morningAvailable ?? false,

        afternoonAvailable:
          existing?.afternoonAvailable ?? false

      });


      current.setDate(
        current.getDate() + 1
      );
    }


    return days;
  }


  // -------------------------------------------------------
  // TOGGLE
  // -------------------------------------------------------

  toggleMorning(
    day: AvailabilityDay
  ): void {

    day.morningAvailable =
      !day.morningAvailable;


    this.days.set([
      ...this.days()
    ]);
  }


  toggleAfternoon(
    day: AvailabilityDay
  ): void {

    day.afternoonAvailable =
      !day.afternoonAvailable;


    this.days.set([
      ...this.days()
    ]);
  }


  // -------------------------------------------------------
  // SAVE
  // -------------------------------------------------------

  save(
    day: AvailabilityDay
  ): void {

    if (
      this.savingDate() === day.date
    ) {
      return;
    }


    this.savingDate.set(day.date);

    this.savedDate.set(null);


    this.api.setAvailability(

      day.date,

      day.morningAvailable,

      day.afternoonAvailable

    )
      .subscribe({

        next: result => {

          console.log(
            'Availability saved:',
            result
          );


          this.savingDate.set(null);

          this.savedDate.set(
            day.date
          );


          setTimeout(() => {

            if (
              this.savedDate() === day.date
            ) {

              this.savedDate.set(null);

            }

          }, 2000);

        },


        error: error => {

          console.error(
            'Failed to save availability:',
            error
          );


          this.savingDate.set(null);

          this.savedDate.set(null);


          // Bei einem Fehler laden wir
          // den Zustand aus der DB neu.

          this.loadAvailability();

        }

      });
  }


  // -------------------------------------------------------
  // DATE
  // -------------------------------------------------------

  private toDateString(
    date: Date
  ): string {

    return date
      .toISOString()
      .substring(0, 10);
  }

}