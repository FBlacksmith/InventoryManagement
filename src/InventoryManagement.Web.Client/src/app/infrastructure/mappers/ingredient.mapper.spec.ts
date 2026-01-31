import { IngredientMapper } from './ingredient.mapper';
import { IngredientDTO } from '../dtos/ingredient.dto';
import { Ingredient } from '../../core/models/ingredient.model';

describe('IngredientMapper', () => {
  it('should transform IngredientDTO to Ingredient model correctly', () => {
    const dto: IngredientDTO = {
      id: '123-abc',
      name: 'Flour',
      measurementUnit: 'kg'
    };

    const model: Ingredient = IngredientMapper.fromDTO(dto);

    expect(model.id).toBe(dto.id);
    expect(model.name).toBe(dto.name);
    expect(model.measurementUnit).toBe(dto.measurementUnit);
    // Explicit check for potentially added logic in future
  });

  it('should transform array of DTOs to array of Models', () => {
    const dtos: IngredientDTO[] = [
      { id: '1', name: 'A', measurementUnit: 'g' },
      { id: '2', name: 'B', measurementUnit: 'ml' }
    ];

    const models = IngredientMapper.fromDTOs(dtos);

    expect(models.length).toBe(2);
    expect(models[0].id).toBe('1');
    expect(models[1].name).toBe('B');
  });
});
