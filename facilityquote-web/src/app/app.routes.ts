import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./features/request/request-page/request-page')
                .then(m => m.RequestPage)
    }
];