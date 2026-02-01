import '@angular/compiler';
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { BaseApiService } from './base-api.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, throwError } from 'rxjs';

// Mock dependencies
const mockHttpClient = {
  get: vi.fn(),
  post: vi.fn(),
  put: vi.fn(),
  delete: vi.fn()
};

// Mock @angular/core inject
vi.mock('@angular/core', async (importOriginal) => {
  const actual = await importOriginal<typeof import('@angular/core')>();
  return {
    ...actual,
    inject: vi.fn((token: any) => {
      if (token === 'API_URL') {
        return '/api';
      }
      if (token === HttpClient) {
        return mockHttpClient;
      }
      return null;
    })
  };
});

// Concrete implementation
@Injectable()
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

  beforeEach(() => {
    vi.clearAllMocks();
    // Instantiate directly; property initializers will call the mocked inject()
    service = new TestApiService();
  });

  it('should perform GET request with correct URL prefix', () => {
    const mockData = { id: 1 };
    mockHttpClient.get.mockReturnValue(of(mockData));

    service.testGet('test').subscribe(data => {
      expect(data).toEqual(mockData);
    });

    expect(mockHttpClient.get).toHaveBeenCalledWith('/api/test');
  });

  it('should perform POST request with correct URL prefix', () => {
    const body = { name: 'Test' };
    const response = { id: 1, ...body };
    mockHttpClient.post.mockReturnValue(of(response));

    service.testPost('create', body).subscribe(data => {
      expect(data).toEqual(response);
    });

    expect(mockHttpClient.post).toHaveBeenCalledWith('/api/create', body);
  });

  it('should handle errors correctly', () => {
    const errorResponse = { 
      status: 400, 
      error: {
        errors: {
          'Field': ['Error1']
        }
      } 
    };
    mockHttpClient.get.mockReturnValue(throwError(() => errorResponse));

    service.testGet('error').subscribe({
      next: () => { throw new Error('Should have failed'); },
      error: (error) => {
        expect(error).toBeDefined();
        // Validation error check
        expect(error.message).toContain('Field: Error1');
      }
    });
  });
});
