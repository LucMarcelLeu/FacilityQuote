import { Routes } from '@angular/router';

import { AvailabilityPage } from './features/availability/availability-page/availability-page';
import { adminGuard } from './core/guards/admin.guard';
import { CustomersPage } from './features/customer/customers-page';
import { CustomerDetailPage } from './features/customer/customer-detail-page/customer-detail-page';
import { RequestDetailPage } from './features/request/request-detail-page/request-detail-page';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./features/request/request-page/request-page')
                .then(m => m.RequestPage)
    },
    {
        path: 'availability',
        component: AvailabilityPage,
        canActivate: [adminGuard]
    },
    {
        path: 'customers',
        component: CustomersPage,
        canActivate: [adminGuard]
    },
    {
        path: 'customers/:id',
        component: CustomerDetailPage,
        canActivate: [adminGuard]
    },
    {
        path: 'requests/:id',
        component: RequestDetailPage,
        canActivate: [adminGuard]
    },
];