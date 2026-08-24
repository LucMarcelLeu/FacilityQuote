import {
    ActivatedRouteSnapshot,
    CanActivateFn,
    Router,
    RouterStateSnapshot,
    UrlTree
} from '@angular/router';

import { inject } from '@angular/core';

import {
    AuthGuardData,
    createAuthGuard
} from 'keycloak-angular';

const isAccessAllowed = async (
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
    authData: AuthGuardData
): Promise<boolean | UrlTree> => {

    const { authenticated, grantedRoles } = authData;

    const router = inject(Router);

    /*
     * Benutzer ist nicht eingeloggt.
     * Wir schicken ihn zum Keycloak Login.
     */
    if (!authenticated) {

        await authData.keycloak.login({
            redirectUri: window.location.origin + state.url
        });

        return false;
    }

    /*
     * Prüfen, ob der Benutzer die Realm-Rolle "admin" besitzt.
     *
     * Dein JWT enthält:
     *
     * realm_access.roles = [
     *   ...
     *   "admin"
     * ]
     */

    const hasAdminRole =
        grantedRoles.realmRoles.includes('admin');

    if (!hasAdminRole) {
        console.warn('Access denied: admin role required.');

        return router.parseUrl('/');
    }

    return true;
};


export const adminGuard =
    createAuthGuard<CanActivateFn>(isAccessAllowed);