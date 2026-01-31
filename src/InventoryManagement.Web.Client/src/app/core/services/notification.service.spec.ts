import { TestBed } from '@angular/core/testing';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotificationService } from './notification.service';
import { vi } from 'vitest';

describe('NotificationService', () => {
  let service: NotificationService;
  let snackBarMock: any;

  beforeEach(() => {
    snackBarMock = {
      open: vi.fn()
    };

    TestBed.configureTestingModule({
      providers: [
        NotificationService,
        { provide: MatSnackBar, useValue: snackBarMock }
      ]
    });

    service = TestBed.inject(NotificationService);
  });

  describe('success', () => {
    it('should open snackbar with success configuration', () => {
      service.success('Operation successful');

      expect(snackBarMock.open).toHaveBeenCalledWith('Operation successful', 'Close', {
        duration: 3000,
        horizontalPosition: 'end',
        verticalPosition: 'top',
        panelClass: ['snackbar-success']
      });
    });
  });

  describe('error', () => {
    it('should open snackbar with error configuration', () => {
      service.error('Something went wrong');

      expect(snackBarMock.open).toHaveBeenCalledWith('Something went wrong', 'Close', {
        duration: 5000,
        horizontalPosition: 'end',
        verticalPosition: 'top',
        panelClass: ['snackbar-error']
      });
    });
  });
});
