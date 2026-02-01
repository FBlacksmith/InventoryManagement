import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslocoService } from '@ngneat/transloco';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private snackBar = inject(MatSnackBar);
  private transloco = inject(TranslocoService);

  private readonly defaultDuration = 3000;
  private readonly errorDuration = 5000;

  success(message: string): void {
    const translatedMessage = this.transloco.translate(message);
    this.snackBar.open(translatedMessage, 'Close', {
      duration: this.defaultDuration,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: ['snackbar-success']
    });
  }

  error(message: string): void {
    const translatedMessage = this.transloco.translate(message);
    this.snackBar.open(translatedMessage, 'Close', {
      duration: this.errorDuration,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: ['snackbar-error']
    });
  }
}
