import { describe, it, expect, vi, beforeEach } from 'vitest';
import { ErrorNotificationBehavior } from './error-notification.behavior';
import { NotificationService } from '@core/services/notification.service';
import { TestBed } from '@angular/core/testing';

// Mock NotificationService
class MockNotificationService {
  error = vi.fn();
}

describe('ErrorNotificationBehavior', () => {
  let behavior: ErrorNotificationBehavior;
  let notificationService: MockNotificationService;

  beforeEach(() => {
    notificationService = new MockNotificationService();
    
    // Setup TestBed to handle injection context if needed, 
    // but typically we can construct if we mock dependencies. 
    // However, since the class uses 'inject()', we need injection context.
    
    TestBed.configureTestingModule({
      providers: [
        ErrorNotificationBehavior,
        { provide: NotificationService, useValue: notificationService }
      ]
    });

    behavior = TestBed.inject(ErrorNotificationBehavior);
  });

  it('should call next and return result when no error occurs', async () => {
    const nextSpy = vi.fn().mockResolvedValue('success');
    const request = {} as any;

    const result = await behavior.handle(request, nextSpy);

    expect(nextSpy).toHaveBeenCalled();
    expect(result).toBe('success');
    expect(notificationService.error).not.toHaveBeenCalled();
  });

  it('should call notification.error and rethrow when an error occurs', async () => {
    const error = new Error('Test error');
    const nextSpy = vi.fn().mockRejectedValue(error);
    const request = {} as any;

    await expect(behavior.handle(request, nextSpy)).rejects.toThrow(error);

    expect(nextSpy).toHaveBeenCalled();
    expect(notificationService.error).toHaveBeenCalledWith('Test error');
  });

  it('should use default message for non-Error objects', async () => {
    const error = 'String error';
    const nextSpy = vi.fn().mockRejectedValue(error);
    const request = {} as any;

    try {
      await behavior.handle(request, nextSpy);
    } catch {
      // Ignore error for assertion
    }

    expect(notificationService.error).toHaveBeenCalledWith('An unexpected error occurred');
  });
});
