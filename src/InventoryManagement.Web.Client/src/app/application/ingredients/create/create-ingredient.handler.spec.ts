import { TestBed } from '@angular/core/testing';
import { CreateIngredientHandler, CreateIngredientRequest } from './create-ingredient.handler';
import { IngredientApiService } from '@infra/services/ingredient-api.service';
import { vi } from 'vitest';

describe('CreateIngredientHandler', () => {
  let handler: CreateIngredientHandler;
  let apiMock: any;

  beforeEach(() => {
    apiMock = {
      createIngredient: vi.fn()
    };
    
    TestBed.configureTestingModule({
      providers: [
        CreateIngredientHandler,
        { provide: IngredientApiService, useValue: apiMock }
      ]
    });

    handler = TestBed.inject(CreateIngredientHandler);
  });

  it('should call api.createIngredient with correct data', async () => {
    const data = { name: 'Flour', measurementUnit: 'Grams' };
    const request = new CreateIngredientRequest(data);
    
    await handler.handle(request);

    expect(apiMock.createIngredient).toHaveBeenCalledWith({
      name: data.name,
      measurementUnit: data.measurementUnit
    });
  });
});
