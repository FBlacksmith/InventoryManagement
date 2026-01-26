import { ApplicationConfig, provideBrowserGlobalErrorListeners, Injector } from '@angular/core';
import { provideRouter } from '@angular/router';
import { Mediator } from 'mediatr-ts';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    {
      provide: Mediator,
      useFactory: (injector: Injector) => new Mediator({ resolver: { resolve: (token: any) => injector.get(token) } as any }),
      deps: [Injector]
    }
  ]
};
