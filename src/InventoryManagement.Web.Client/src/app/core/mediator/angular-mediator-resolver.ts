import { Injectable, Injector, runInInjectionContext } from '@angular/core';
import { Resolver } from 'mediatr-ts';

@Injectable({
  providedIn: 'root'
})
export class AngularMediatorResolver implements Resolver {
  constructor(private injector: Injector) {}

  resolve<T>(token: any): T {
    try {
      return this.injector.get<T>(token);
    } catch (error) {
       console.error(`Error resolving handler for token: ${token?.name || token}`, error);
       throw error;
    }
  }

  add(token: any): void {
      // Not used in this implementation as registration is handled via Angular DI
  }
  
  remove(token: any): void {
      // Not used
  }

  clear(): void {
      // Not used
  }
  
  *[Symbol.iterator](): Iterator<any> {
      // Not used
  }
}
