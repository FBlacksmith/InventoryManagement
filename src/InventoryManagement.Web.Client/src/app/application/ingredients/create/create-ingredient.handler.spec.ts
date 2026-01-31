import { TestBed } from '@angular/core/testing';
import { CreateIngredientHandler, CreateIngredientRequest } from './create-ingredient.handler';
import { IngredientApiService } from '@infra/services/ingredient-api.service';
import { IngredientDTO } from '@infra/dtos/ingredient.dto';
import { vi } from 'vitest';

describe('CreateIngredientHandler', () => {
  let handler: CreateIngredientHandler;
  let apiMock: any;

  const mockDto: IngredientDTO = {
    id: '123',
    name: 'Flour',
    measurementUnit: 'Grams'
  };

  beforeEach(() => {
    apiMock = {
      createIngredient: vi.fn().mockResolvedValue(mockDto)
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

  it('should return IngredientDTO from API', async () => {
    const data = { name: 'Flour', measurementUnit: 'Grams' };
    const request = new CreateIngredientRequest(data);
    
    const result = await handler.handle(request);

    expect(result).toEqual(mockDto);
  });
});

