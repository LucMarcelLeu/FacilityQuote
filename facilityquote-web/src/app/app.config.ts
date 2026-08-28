import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection
} from '@angular/core';

import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { environment } from '../environments/environment.development';

import {
  provideKeycloak,
  createInterceptorCondition,
  IncludeBearerTokenCondition,
  INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
  includeBearerTokenInterceptor
} from 'keycloak-angular';

import { routes } from './app.routes';

const adminApiCondition =
  createInterceptorCondition<IncludeBearerTokenCondition>({
    urlPattern: /^\/api\/admin\/.*$/i,
    bearerPrefix: 'Bearer'
  });

export const appConfig: ApplicationConfig = {

  providers: [

    provideBrowserGlobalErrorListeners(),

    provideZonelessChangeDetection(),

    provideKeycloak({
      config: {
        url: environment.keycloak.url,
        realm: 'facilityquote',
        clientId: 'facilityquote-web'
      },

      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri:
          window.location.origin + '/silent-check-sso.html',
        checkLoginIframe: false
      }
    }),

    {
      provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
      useValue: [adminApiCondition]
    },

    provideRouter(routes),

    provideHttpClient(
      withInterceptors([
        includeBearerTokenInterceptor
      ])
    )

  ]

};