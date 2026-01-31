import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { IngredientApiService } from './ingredient-api.service';
import { IngredientDTO } from '../dtos/ingredient.dto';

describe('IngredientApiService', () => {
  let service: IngredientApiService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        IngredientApiService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(IngredientApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get ingredients and update signals (isLoading, ingredients)', () => {
    const mockData: IngredientDTO[] = [
      { id: '1', name: 'Salt', measurementUnit: 'g' },
      { id: '2', name: 'Sugar', measurementUnit: 'kg' }
    ];

    // Initial state
    expect(service.ingredients().length).toBe(0);
    expect(service.isLoading()).toBe(false);

    // Call method
    service.getIngredients();

    // Verify loading state
    expect(service.isLoading()).toBe(true);

    const req = httpMock.expectOne('/api/ingredients');
    expect(req.request.method).toBe('GET');
    req.flush(mockData);

    // Verify final state
    expect(service.isLoading()).toBe(false);
    expect(service.ingredients().length).toBe(2);
    expect(service.ingredients()[0].name).toBe('Salt');
  });

  it('should handle error when getting ingredients', () => {
    service.getIngredients();
    expect(service.isLoading()).toBe(true);
    expect(service.error()).toBeNull();

    const req = httpMock.expectOne('/api/ingredients');
    req.flush('Server Error', { status: 500, statusText: 'Internal Server Error' });

    expect(service.isLoading()).toBe(false);
    expect(service.error()).toContain('500');
    expect(service.ingredients().length).toBe(0);
  });

  it('should create ingredient and update list', () => {
    const newIngredientReq = { name: 'Pepper', measurementUnit: 'g' };
    const createdRequestResponse: IngredientDTO = { id: '3', ...newIngredientReq };

    service.createIngredient(newIngredientReq);
    expect(service.isLoading()).toBe(true);

    const req = httpMock.expectOne('/api/ingredients');
    expect(req.request.method).toBe('POST');
    req.flush(createdRequestResponse);

    expect(service.isLoading()).toBe(false);
    // Should depend on logic: logic appends to current list
    expect(service.ingredients().length).toBe(1); 
    expect(service.ingredients()[0].name).toBe('Pepper');
  });
});
