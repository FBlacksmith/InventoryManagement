import { Injectable, inject } from '@angular/core';
import { pipelineBehavior, PipelineBehavior, RequestData } from 'mediatr-ts';
import { NotificationService } from '@core/services/notification.service';

@Injectable({
  providedIn: 'root'
})
@pipelineBehavior()
export class ErrorNotificationBehavior implements PipelineBehavior {
  private notification = inject(NotificationService);

  async handle(request: RequestData<any>, next: () => unknown): Promise<any> {
    try {
      return await (next() as Promise<any>);
    } catch (error: unknown) {
      const message = error instanceof Error 
        ? error.message 
        : 'An unexpected error occurred';
      
      this.notification.error(message);
      throw error;
    }
  }
}
