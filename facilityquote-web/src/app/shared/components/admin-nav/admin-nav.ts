import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import {
    KEYCLOAK_EVENT_SIGNAL
} from 'keycloak-angular';
import Keycloak from 'keycloak-js';

@Component({
    selector: 'app-admin-nav',
    standalone: true,
    imports: [
        RouterLink,
        RouterLinkActive
    ],
    templateUrl: './admin-nav.html',
    styleUrl: './admin-nav.scss'
})
export class AdminNavComponent {

    private readonly keycloak = inject(Keycloak);

    // sorgt dafür, dass Angular auf Keycloak-Events reagiert
    private readonly keycloakEvent = inject(KEYCLOAK_EVENT_SIGNAL);

    get isLoggedIn(): boolean {
        return this.keycloak.authenticated ?? false;
    }

    get username(): string {
        return this.keycloak.tokenParsed?.['preferred_username'] ?? '';
    }

    get isAdmin(): boolean {
        const roles =
            this.keycloak.tokenParsed?.['realm_access']?.['roles'] ?? [];

        return roles.includes('admin');
    }

    async login(): Promise<void> {
        await this.keycloak.login({
            redirectUri: window.location.origin
        });
    }

    async logout(): Promise<void> {
        await this.keycloak.logout({
            redirectUri: window.location.origin
        });
    }

    async refresh(): Promise<void> {
        await this.keycloak.updateToken(30);
    }
}