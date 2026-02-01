import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export abstract class BaseApiService {
  protected http = inject(HttpClient);
  protected apiUrl = inject<string>('API_URL' as any);

  protected get<T>(url: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${url}`).pipe(
      catchError((error) => this.handleError(error))
    );
  }

  protected post<TRequest, TResponse>(url: string, body: TRequest): Observable<TResponse> {
    return this.http.post<TResponse>(`${this.apiUrl}/${url}`, body).pipe(
      catchError((error) => this.handleError(error))
    );
  }

  protected put<TRequest, TResponse>(url: string, body: TRequest): Observable<TResponse> {
    return this.http.put<TResponse>(`${this.apiUrl}/${url}`, body).pipe(
      catchError((error) => this.handleError(error))
    );
  }

  protected delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/${url}`).pipe(
      catchError((error) => this.handleError(error))
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';

    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `An error occurred: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code
      const backendError = error.error;
      if (backendError && backendError.errors) {
        // Validation errors e.g. { "field": ["msg1", "msg2"] }
        const validationErrors = Object.entries(backendError.errors)
          .map(([key, msgs]) => `${key}: ${(msgs as string[]).join(', ')}`)
          .join('; ');
        errorMessage = validationErrors || backendError.title || backendError.message || `Server returned code: ${error.status}`;
      } else if (backendError && (backendError.message || backendError.detail)) {
        errorMessage = backendError.message || backendError.detail;
      } else {
        errorMessage = `Server returned code: ${error.status}, error message is: ${error.message}`;
      }
    }

    console.error(errorMessage, error);
    // Optionally dispatch to ErrorService here
    
    return throwError(() => new Error(errorMessage));
  }
}
