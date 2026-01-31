import { ApplicationConfig, provideBrowserGlobalErrorListeners, Injector } from '@angular/core';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { Mediator } from 'mediatr-ts';
import { AngularMediatorResolver } from './core/mediator/angular-mediator-resolver';
import { APPLICATION_HANDLERS } from './app.handlers';
import { ErrorNotificationBehavior } from './application/common/behaviors/error-notification.behavior';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withFetch()),
    {
      provide: Mediator,
      useFactory: (resolver: AngularMediatorResolver) => {
        const mediator = new Mediator({ resolver });
        mediator.pipelineBehaviors.setOrder([ErrorNotificationBehavior]);
        return mediator;
      },
      deps: [AngularMediatorResolver]
    },
    ...APPLICATION_HANDLERS
  ]
};
