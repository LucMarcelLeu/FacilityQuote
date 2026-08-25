import {
    ActivatedRouteSnapshot,
    CanActivateFn,
    RouterStateSnapshot,
    UrlTree
} from '@angular/router';

import {
    AuthGuardData,
    createAuthGuard
} from 'keycloak-angular';

const isAccessAllowed = async (
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
    authData: AuthGuardData
): Promise<boolean | UrlTree> => {

    if (authData.authenticated) {
        return true;
    }

    await authData.keycloak.login({
        redirectUri: window.location.origin + state.url
    });

    return false;
};

export const authGuard =
    createAuthGuard<CanActivateFn>(isAccessAllowed);