import { Routes } from '@angular/router';
import { AvailabilityPage } from './features/availability/availability-page/availability-page';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./features/request/request-page/request-page')
                .then(m => m.RequestPage)
    }, 
    {
        path: 'availability',
        component: AvailabilityPage
    },
];