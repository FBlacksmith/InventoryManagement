import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

// Concrete implementation for testing abstract class
@Injectable({ providedIn: 'root' })
class TestApiService extends BaseApiService {
  public testGet<T>(url: string) {
    return this.get<T>(url);
  }

  public testPost<TRequest, TResponse>(url: string, body: TRequest) {
    return this.post<TRequest, TResponse>(url, body);
  }
}

describe('BaseApiService', () => {
  let service: TestApiService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        TestApiService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(TestApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should perform GET request and return data', () => {
    const mockData = { id: 1, name: 'Test' };
    
    service.testGet<{ id: number; name: string }>('/api/test').subscribe(data => {
      expect(data).toEqual(mockData);
    });

    const req = httpMock.expectOne('/api/test');
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });

  it('should perform POST request and return response', () => {
    const requestBody = { name: 'New Item' };
    const responseBody = { id: 123, ...requestBody };

    service.testPost('/api/test', requestBody).subscribe(data => {
      expect(data).toEqual(responseBody);
    });

    const req = httpMock.expectOne('/api/test');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(requestBody);
    req.flush(responseBody);
  });

  it('should handle 404 error appropriately', () => {
    const errorMessage = 'Not Found';

    service.testGet('/api/error').subscribe({
      next: () => { throw new Error('Should have failed'); },
      error: (error: Error) => {
        expect(error.message).toContain('Server returned code: 404');
      }
    });

    const req = httpMock.expectOne('/api/error');
    req.flush(errorMessage, { status: 404, statusText: 'Not Found' });
  });

  it('should handle 500 error appropriately', () => {
    const errorMessage = 'Internal Server Error';

    service.testGet('/api/error').subscribe({
      next: () => { throw new Error('Should have failed'); },
      error: (error: Error) => {
        expect(error.message).toContain('Server returned code: 500');
      }
    });

    const req = httpMock.expectOne('/api/error');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });

  it('should handle client-side error (ErrorEvent)', () => {
    const errorEvent = new ErrorEvent('Network error', {
      message: 'Net failure'
    });

    service.testGet('/api/network-error').subscribe({
      next: () => { throw new Error('Should have failed'); },
      error: (error: Error) => {
        expect(error.message).toContain('An error occurred: Net failure');
      }
    });

    const req = httpMock.expectOne('/api/network-error');
    req.error(errorEvent);
  });
});
