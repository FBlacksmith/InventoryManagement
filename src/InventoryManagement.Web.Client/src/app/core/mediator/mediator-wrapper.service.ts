import { Injectable, inject, signal } from '@angular/core';
import { Mediator, RequestData } from 'mediatr-ts';
import { NotificationService } from '../services/notification.service';

export interface SendOptions {
  successMessage?: string;
}

@Injectable({
  providedIn: 'root'
})
export class MediatorWrapper {
  private mediator = inject(Mediator);
  private notification = inject(NotificationService);

  readonly isLoading = signal(false);

  async send<TResponse>(request: RequestData<TResponse>, options?: SendOptions): Promise<TResponse> {
    this.isLoading.set(true);

    try {
      const result = await this.mediator.send(request);

      if (options?.successMessage) {
        this.notification.success(options.successMessage);
      }

      return result;
    } catch (error: unknown) {
      const message = error instanceof Error 
        ? error.message 
        : 'An unexpected error occurred';
      
      this.notification.error(message);
      throw error;
    } finally {
      this.isLoading.set(false);
    }
  }
}

